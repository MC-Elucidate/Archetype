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


	// Use this for initialization
	protected void Start () {
		base.Start ();
		health = 100;
		maxHealth = 100;
		runSpeed = 10;
		meleeMax = 2;
		characterRadius = 0.4f;
		aggro = 150;

		//Character-specific weapon stats
		weaponRange = 200f;
		weaponFireRate = 1.6f;
		spreadRate = 0.2f;
		maxSpread = 8;
		gunDamage = 120;
		ammoCount = 20;
		maxAmmo = 20;
		ammoPickup = 5;

		//Special cooldowns
		special1CD = 0f;
		special2CD = 90f;
		superCD = 90f;
	}
	
	// Update is called once per frame
	protected void Update () {
		base.Update ();
	}
	public void FixedUpdate(){
		base.FixedUpdate ();
	}
	public override void meleeAttack()
	{
		base.meleeAttack ();
	}

	public override void shootWeapon()
	{
		if (ammoCount > 0) {
			if (scoped) {
				if (weaponFireRateTimer <= 0) {
					RaycastHit hit;
					Ray camRay = cam.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
					Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
					Physics.Raycast (camRay, out hit, weaponRange);
				
					//Debug.Log ("Shooting at " + hit.transform.gameObject.name);
				
					Vector3 target = hit.point;
					Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
					Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);
					if (hit.transform.gameObject.tag == "Enemy") {
						hit.transform.gameObject.SendMessage ("receiveDamage", (int)(gunDamage*damageMod), SendMessageOptions.DontRequireReceiver);
						hit.transform.gameObject.SendMessage ("receivePoiseDamage", (int)(poiseDamage*damageMod), SendMessageOptions.DontRequireReceiver);
					}
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
				neckAngle = 0f;
				cam = fpcam;
				rotSpeed = rotScopeSpeed;
			} else {
				scoped = !scoped;
				fpcam.enabled = false;
				tpcam.enabled = true;
				cam = tpcam;
				rotSpeed = 7f;
			}
		}
	}

	public override void special2()
	{
		if (currentSpecial2 <= 0) {
			currentSpecial2 = special2CD;
			Debug.Log ("Doing special2");
			sounds.playSpecial2Sound();
		}
	}

	public override void super()
	{
		if (currentSuper <= 0) {
			currentSuper = superCD;
			//Debug.Log ("Doing super");

			RaycastHit hit;
			Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f, 0.666667f, 0));
			
			//Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
			Physics.Raycast (camRay, out hit, weaponRange);

			
			Vector3 target = hit.point;
			Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
			Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);

			Quaternion kiballRotation = Quaternion.identity;
			kiballRotation.SetLookRotation (target - shot_source.position, Vector3.up);
			
			SniperKiballScript shield = Instantiate (kiballPrefab, RHandPos.position, kiballRotation) as SniperKiballScript;


			sounds.playSpecial3Sound();
		}
	}

	public override void rotateCamera(float pitch)
	{
		if (scoped) {
			neckAngle -= pitch; //Change the angle the camera is looking at based n input
			if (neckAngle > neckAngleLimit) //Clamp the camera angle so we don't break out necks
				neckAngle = neckAngleLimit;
			else if (neckAngle < -neckAngleLimit)
				neckAngle = -neckAngleLimit;
			cam.transform.localRotation = Quaternion.Euler (neckAngle, 0f, 0f);
		} 
		else
			base.rotateCamera (pitch);
	}
}
