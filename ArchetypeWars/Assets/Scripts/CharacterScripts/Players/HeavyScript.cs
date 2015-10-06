using UnityEngine;
using System.Collections;

public class HeavyScript: PlayerCharacter {

	//Prefabs for objects that the heavy can spawn
	public Transform rocketPrefab;
	public Transform defensePrefab;

	//Taunt variables
	private int baseAggro = 200, tauntAggro = 400;
	private float tauntDuration = 15, currentTaunt = 0;

	//Ground pound variables
	private int gpDamage = 10, gpPoiseDamage = 90;

	// Use this for initialization
	protected void Start () {
		base.Start ();
		health = 200;
		maxHealth = 200;
		runSpeed = 10;
		meleeMax = 4;
		currentMelee = 0;
		characterRadius = 0.4f;
		aggro = baseAggro;

		//Character-specific weapon stats
		weaponRange = 500f;
		weaponFireRate = 0.8f;
		spreadRate = 0.21f;
		maxSpread = 12;
		ammoCount = 10;
		maxAmmo = 10;
		gunDamage = 80;
		ammoPickup = 5;

		//Special cooldowns
		special1CD = 90f;
		special2CD = 90f;
		superCD = 90f;
	}
	

	// Update is called once per frame
	protected void Update () {
		base.Update ();
	}

	public void FixedUpdate(){
		base.FixedUpdate ();

		//FixedUpdate used to end the taunt after a specific amount of time has passed
		if (currentTaunt > 0)
			currentTaunt -= Time.fixedDeltaTime;
		else
			aggro = baseAggro;
	}

	/*
	 * Performs a melee attack. uses base class method
	*/
	public override void meleeAttack()
	{
		base.meleeAttack ();
	}

	/*
	 * Fires a bullet.
	 * Overrides base method as the heavy fires explosive projectiles instead of rays.
	 * Calculates the initial direction that the projectile would be moving in and then instantiates the projectile.
	*/
	public override void shootWeapon()
	{
		if (ammoCount > 0) {
			if (weaponFireRateTimer <= 0) {
				
				//Determine direction that we're aiming in.
				RaycastHit hit;
				//Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f + Random.Range (-spreadCount * spreadFactor, spreadCount * spreadFactor), 0.666667f + Random.Range (-spreadCount * spreadFactor, spreadCount * spreadFactor), 0));
				Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f + Random.Range (-spreadCount * spreadFactor, spreadCount * spreadFactor), 0.5f + Random.Range (-spreadCount * spreadFactor, spreadCount * spreadFactor), 0));

				Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.2f);
				Physics.Raycast (camRay, out hit, weaponRange);

				Vector3 target = hit.point;
				Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
				
				Quaternion rocketRotation = Quaternion.identity;
				rocketRotation.SetLookRotation (target - shot_source.position, Vector3.up);

				//Instatiates the rocket projectile
				Transform rocket;
				rocket = Instantiate (rocketPrefab, shot_source.position, rocketRotation) as Transform;
				rocket.gameObject.SendMessage("setDamage",(int)(gunDamage*damageMod));
				Debug.DrawRay (shot_source.position, hit.point * 10f, Color.green, 0.2f);

				//Handles accuracy and fire rate
				weaponFireRateTimer = weaponFireRate;
				spreadCount++;
				spreadRateTimer = spreadRate;
				ammoCount--;
				sounds.pew ();
			} 
		}

	}


	/*
	 * Allows the heavy to deploy a force field that decreases the damage received by teammates inside the force field.
	 * Determines the direction to throw the object in and then instantiates the force field.
	 */
	public override void special1()
	{
		//Check cooldown
		if (currentSpecial1 <= 0) {
			currentSpecial1 = special1CD;

			//Determine direction to deploy the object.
			RaycastHit hit;
			//Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f, 0.666667f, 0));
			Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0));

			Physics.Raycast (camRay, out hit, weaponRange);

			Vector3 target = hit.point;
			Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
			Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);
			
			Quaternion shieldRotation = Quaternion.identity;
			shieldRotation.SetLookRotation (target - shot_source.position, Vector3.up);

			//Instantiate object
			HeavyShieldScript shield = Instantiate (defensePrefab, RHandPos.position, shieldRotation) as HeavyShieldScript;

			sounds.playSpecial1Sound();
		}
	}


	/*
	 * Allows the heavy to perform a taunt.
	 * The taunt lasts a set number of seconds and increases the Heavy's aggro value.
	 * The higher aggro value influences enemies to be more likely to target her.
	*/
	public override void special2()
	{
		//Check cooldown
		if (currentSpecial2 <= 0) {
			currentSpecial2 = special2CD;
			aggro = tauntAggro;
			currentTaunt = tauntDuration;
			sounds.playSpecial2Sound();
		}
	}

	/*
	 * Performs a ground pound.
	 * Deals slight damage and massive poise damage to all enemies on the map.
	*/
	public override void super()
	{
		//Check cooldown.
		if (currentSuper <= 0) {
			currentSuper = superCD;
			//Send damage and poise damage to all enemies in the scene.
			foreach (Transform enemy in RoundManager.enemies)
			{
				enemy.gameObject.SendMessage("receiveDamage", gpDamage);
				enemy.gameObject.SendMessage("receivePoiseDamage", gpPoiseDamage);
			}
			sounds.playSpecial3Sound();
		}
	}

	/*
	 * Uses base camera rotation method
	*/
	public override void rotateCamera(float pitch)
	{
		base.rotateCamera (pitch);
	}
}
