using UnityEngine;
using System.Collections;

public class NinjaScript: PlayerCharacter {

	public Transform cardPrefab;

	//Invis variables
	private int baseAggro = 170, invisAggro = 0;
	private float invisDuration = 15, currentInvis = 0;

	//Instakill variables
	private int instakillDamage = 1000;
	private float instakillRange = 5;

	//SecondMelee
	public GameObject SRWeapon2;

	// Use this for initialization
	protected void Start () {
		base.Start ();
		health = 150;
		maxHealth = 150;
		runSpeed = 16;
		meleeMax = 7;
		characterRadius = 0.4f;
		aggro = baseAggro;

		//Character-specific weapon stats
		weaponRange = 2000f;
		weaponFireRate = 0.2f;
		spreadRate = 0.21f;
		maxSpread = 12;
		weaponHeld = true;
		ammoCount = 60;
		maxAmmo = 60;
		ammoPickup = 20;
		SRWeapon2.SetActive (false);

		//Special cooldowns
		special1CD = 0f;
		special2CD = 90f;
		superCD = 7f;
	}
	
	// Update is called once per frame
	protected void Update () {
		base.Update ();
	}
	public void FixedUpdate(){
		base.FixedUpdate ();

		//Uses FixedUpdate to countdown to when invisibility deactivates.
		if (currentInvis > 0)
			currentInvis -= Time.fixedDeltaTime;
		else
			aggro = baseAggro;
	}

	/*
	 * Fires weapon. Overrides base method as the ninja throws card objects instead of firing bullets.
	 * Gets the direction in which the card will travel and then instantiates the card in that direction.
	*/
	public override void shootWeapon()
	{
		if(ammoCount>0)
		{
			if (weaponFireRateTimer <= 0) {

				//Determines direction to throw cards. Draws a ray from the crosshair on screen to straight ahead.
				//Then marks first object hit as the target destination of the card.
				RaycastHit hit;
				Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f + Random.Range (-spreadCount*spreadFactor,spreadCount*spreadFactor),  0.5f + Random.Range (-spreadCount*spreadFactor,spreadCount*spreadFactor), 0));
				Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
				Physics.Raycast (camRay, out hit, weaponRange);

				Vector3 target = hit.point;
				Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
				Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);
				
				Quaternion cardRotation = Quaternion.identity;
				cardRotation.SetLookRotation(target - shot_source.position, Vector3.up);

				//Instantiate card at the Ninja's hand, moving in the direction of the target.
				Transform card;
				card = Instantiate (cardPrefab, shot_source.position, cardRotation) as Transform;
				card.gameObject.SendMessage("setDamage",(int)(gunDamage*damageMod));

				//Handles fire rate and accuracy.
				weaponFireRateTimer = weaponFireRate;
				spreadCount++;
				spreadRateTimer = spreadRate;
				ammoCount--;


				sounds.pew();
			}
		} 

	}

	/*
	 * Ninja's first ability is a passive that grants her higher move speed.
	 * */
	public override void special1()
	{
		if (currentSpecial1 <= 0) {
			//sounds.playSpecial1Sound ();
		}
	}

	/*
	 * Makes the ninja "invisible".
	 * Lowers her aggro to 0 for a short time to make enemies ignore her.
	*/
	public override void special2()
	{
		//Checks cooldown
		if (currentSpecial2 <= 0) {
			currentSpecial2 = special2CD;
			aggro = invisAggro;
			currentInvis = invisDuration;
			sounds.playSpecial2Sound ();

			//Tells AI module to recheck targets immediately after turning invisible.
			RoundManager.AITactics.assignTargets();
			RoundManager.AITactics.Strategize();
		}
	}

	/*
	 * Allows the ninja to instantly execute an enemy.
	 * Ninja stands behind her target and presses the super ability button to perform execution.
	 * */
	public override void super()
	{
		//Checks cooldown
		if (currentSuper <= 0) {
			currentSuper = superCD;
			//Casts ray forward from the ninja. Performs an execution if the target is an enemy and the ninja is facing their back.
			RaycastHit hit;
			if(Physics.Raycast (transform.position + new Vector3(0, 0.5f, 0), transform.forward, out hit, instakillRange))
			{
				if(hit.transform.tag == "Enemy")
				{
					if(Vector3.Angle(transform.forward, hit.transform.forward) <= 70)
					{
						hit.transform.gameObject.SendMessage("receiveDamage", instakillDamage);
					}
				}
			}
			sounds.playSpecial3Sound ();
		}
	}

	/*
	 * Overrides IK methods because ninja doesn't use IK for her long range weapon.
	 * */
	public override void checkIK()
	{}

	/*
	 * Overrides IK methods because ninja doesn't use IK for her long range weapon.
	 * */
	public void OnAnimatorIK()
	{}

	/*
	 * Rotates camera according to input. Overrides base method because ninja does not use IK.
	 * Hence, she does not rotate her arms along with the camera.
	 * Locks the camera to a given range.
	 * */
	public override void rotateCamera(float pitch)
	{
		Vector3 cameraPivotPoint = transform.position;
		cameraPivotPoint.y += 0.8f;
		if (pitch > 0) { // if we look up
			if(		(cam.transform.localEulerAngles.x > 295) 	|| 	(cam.transform.localEulerAngles.x < 90)		)
			{
				cam.transform.RotateAround (cameraPivotPoint, transform.right, -pitch);
			}
		} else if (pitch < 0) { //if we look down
			if(		(cam.transform.localEulerAngles.x > 270) 	|| 	(cam.transform.localEulerAngles.x < 65)		)
			{
				cam.transform.RotateAround (cameraPivotPoint, transform.right, -pitch);
			}
		}
	}

	/*
	 * Performs melee attack.
	 * Overrides base method as ninja uses 2 melee weapons. (Meaning 2 weapons to make active)
	 * Actual logic is identical to base method.
	 * */
	public override void meleeAttack()
	{
		if (isGrounded) { //Can only melee on the ground
			if (currentMelee == 0) { //First melee attack in the combo
				currentMelee++;
				melee = true;
				weaponHeld = false;
				SRWeapon.SetActive (true);
				SRWeapon2.SetActive (true);
				sounds.meleeSound();
			} else if ((currentMelee < meleeMax) && (anim.GetCurrentAnimatorStateInfo (1).IsName ("Attack" + currentMelee))) { //Post-first melee attacks, can only transition into state n+1 if we're in state n
				currentMelee++;
				sounds.meleeSound ();
			}
		}
		
	}

	/*
	 * Ends melee attack.
	 * Overrides base method as ninja uses 2 melee weapons. (Meaning 2 weapons to deactivate)
	 * Actual logic is identical to base method.
	 * */
	public override void meleeAttackEnd()
	{
		melee = false;
		currentMelee = 0;
		SRWeapon.SetActive (false);
		SRWeapon2.SetActive (false);
		weaponHeld = true;
	}

}
