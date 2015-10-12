using UnityEngine;
using System.Collections;

public class SniperScript: PlayerCharacter {

	//Scope variables
	private float rotScopeSpeed = 1f;
	private float neckAngleLimit = 50f;
	private float neckAngle = 0f;
	private bool scoped = false;
	public Camera tpcam;
	public Camera fpcam;

	//Ki-ball variables
	public Transform kiballPrefab;

	//Laugh variables
	private float cooldownReduction = 5f;


	// Use this for initialization
	protected void Start () {
		base.Start ();
		health = 150;
		maxHealth = 150;
		runSpeed = 10;
		meleeMax = 2;
		characterRadius = 0.4f;
		aggro = 300;

		maxForwardSpeed = 6f;
		maxBackSpeed = -5f;
		maxSideSpeed = 5f;
		slideSpeed = 9f;

		//Character-specific weapon stats
		weaponRange = 2000f;
		weaponFireRate = 1.6f;
		spreadRate = 0.2f;
		maxSpread = 8;
		gunDamage = 120;
		ammoCount = 20;
		maxAmmo = 20;
		ammoPickup = 5;

		//Special cooldowns
		special1CD = 0f;
		special2CD = 10f;
		superCD = 60f;
	}
	
	// Update is called once per frame
	protected void Update () {
		base.Update ();
	}
	public void FixedUpdate(){
		base.FixedUpdate ();
	}

	/*
	 * Performs melee attack. Calls base method.
	 * */
	public override void meleeAttack()
	{
		base.meleeAttack ();
	}

	/*
	 * Shoots weapon.
	 * If scoped, will fire from the centre of the screen.
	 * If not scoped, calls the base method.
	 * */
	public override void shootWeapon()
	{
		if (ammoCount > 0) {
			if (scoped) {
				if (weaponFireRateTimer <= 0) {

					//Determines target to shoot.
					//Draws ray from centre of the screeen to the first target hit.
					RaycastHit hit;
					Ray camRay = cam.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
					Physics.Raycast (camRay, out hit, weaponRange);
				
					//Then draws ray from gun to the target hit
					Vector3 target = hit.point;
					Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
					Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);

					//Deals damage if it is an enemy
					if (hit.transform.gameObject.tag == "Enemy" || hit.transform.gameObject.tag == "EnemyHead") {
						hit.transform.gameObject.SendMessage ("receiveDamage", (int)(gunDamage*damageMod), SendMessageOptions.DontRequireReceiver);
						hit.transform.gameObject.SendMessage ("receivePoiseDamage", (int)(poiseDamage*damageMod), SendMessageOptions.DontRequireReceiver);
					}

					//Handles fire rate, fire particle, and accuracy
					weaponFireRateTimer = weaponFireRate;
					spreadCount++;
					spreadRateTimer = spreadRate;
					ammoCount--;
					sounds.pew ();
					Transform fireParticle = Instantiate (weaponFlashEffect, shot_source.transform.position, Quaternion.identity) as Transform;
					fireParticle.parent = this.transform;

				}
			} else {
				base.shootWeapon ();
				spreadCount = maxSpread;
			}
		}
	}

	/*
	 * Allows the sniper to look thorugh her scope.
	 * Changes the active camera of the sniper.
	 * Also changes the speed at which the camera rotates.
	 */
	public override void special1()
	{
		if (currentSpecial1 <= 0) {
			currentSpecial1 = special1CD;
			sounds.playSpecial1Sound();
			if (!scoped) {
				scoped = !scoped;
				fpcam.enabled = true;
				tpcam.enabled = false;
				fpcam.rect = tpcam.rect;
				//neckAngle = 0f;
				fpcam.gameObject.transform.localRotation = tpcam.gameObject.transform.localRotation;
				cam = fpcam;
				rotSpeed = rotScopeSpeed;
			} else {
				scoped = !scoped;
				fpcam.enabled = false;
				tpcam.enabled = true;
				tpcam.gameObject.transform.localRotation = fpcam.gameObject.transform.localRotation;
				cam = tpcam;
				rotSpeed = defaultRotationSpeed;
			}
		}
	}

	/*
	 * Just a laugh. Slightly reduces the cooldown of her super move.
	 * */
	public override void special2()
	{
		if (currentSpecial2 <= 0) {
			currentSpecial2 = special2CD;
			if(currentSuper > 0)
				currentSuper-=cooldownReduction;
			sounds.playSpecial2Sound();
		}
	}

	/*
	 * Fires a powerful projectile that decimates all enemies it comes into contact with.
	 * */
	public override void super()
	{
		if (currentSuper <= 0 && !scoped) {
			currentSuper = superCD;

			//Determine direction to travel in.
			//Casts ray from crosshair to first target hit.
			RaycastHit hit;
			Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0));
			Physics.Raycast (camRay, out hit, weaponRange);
			Vector3 target = hit.point;
			Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);


			//Instantiates projectile at gun, moving in the direction of the target
			Quaternion kiballRotation = Quaternion.identity;
			kiballRotation.SetLookRotation (target - shot_source.position, Vector3.up);
			
			SniperKiballScript shield = Instantiate (kiballPrefab, RHandPos.position, kiballRotation) as SniperKiballScript;

			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			foreach (GameObject p in players) {
				p.GetComponent<PlayerCharacter>().shaker.shake = .4f;					//Lasts 0.2 seconds
				p.GetComponent<PlayerCharacter>().shaker.shakeAmount = 3.6f;			//Normal shake?
			}

			sounds.playSpecial3Sound();
		}
	}

	/*
	 * Rotates camera.
	 * If not scoped, uses the base method.
	 * If scoped, rotates the camera based on input.
	 * Keeps track of the current angle the neck is looking at and modifies it based on input.
	 * Then sets the rotation of the camera to be the rotation of the neck.
	 * */
	public override void rotateCamera(float pitch)
	{
		/*if (scoped) {
			neckAngle -= pitch; //Change the angle the camera is looking at based on input
			if (neckAngle > neckAngleLimit) //Clamp the camera angle so we don't break out necks
				neckAngle = neckAngleLimit;
			else if (neckAngle < -neckAngleLimit)
				neckAngle = -neckAngleLimit;
			cam.transform.localRotation = Quaternion.Euler (neckAngle, 0f, 0f);
		} 
		else*/
		base.rotateCamera (pitch);
		fpcam.transform.localPosition = new Vector3 (0f, 57f, 6.8f);
	}

	public override void specialMove(int move)
	{
		switch (move) {
		case 1:
			special1();
			break;
		case 2:
		{
			if (currentSpecial2 <= 0) {
				freemove = false;
				weaponHeld = false; /*special2();*/
				anim.SetTrigger ("Special2Trigger");
			}
		}
			break;
		case 3:
		{
			if (currentSuper <= 0) {
				freemove = false;
				weaponHeld = false; /*super();*/
				anim.SetTrigger ("SuperTrigger");
			}
			break;
		}
		}
	}
}
