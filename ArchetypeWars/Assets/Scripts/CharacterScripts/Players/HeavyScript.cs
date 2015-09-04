using UnityEngine;
using System.Collections;

public class HeavyScript: PlayerCharacter {

	public Transform rocketPrefab;
	public Transform defensePrefab;

	// Use this for initialization
	protected void Start () {
		base.Start ();
		health = 200;
		maxHealth = 200;
		runSpeed = 10;
		meleeMax = 4;
		currentMelee = 0;
		characterRadius = 0.4f;

		//Character-specific weapon stats
		weaponRange = 100f;
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
	}
	public override void meleeAttack()
	{
		base.meleeAttack ();
	}

	public override void shootWeapon()
	{
		if (ammoCount > 0) {
			if (weaponFireRateTimer <= 0) {
				
				
				RaycastHit hit;
				Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f + Random.Range (-spreadCount * spreadFactor, spreadCount * spreadFactor), 0.666667f + Random.Range (-spreadCount * spreadFactor, spreadCount * spreadFactor), 0));
				
				Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
				Physics.Raycast (camRay, out hit, weaponRange);
				
				Debug.Log ("Heavy Shooting at " + hit.transform.gameObject.name);
				
				Vector3 target = hit.point;
				Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
				Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);
				
				Quaternion rocketRotation = Quaternion.identity;
				rocketRotation.SetLookRotation (target - shot_source.position, Vector3.up);

				Transform rocket;
				rocket = Instantiate (rocketPrefab, shot_source.position, rocketRotation) as Transform;
				rocket.gameObject.SendMessage("setDamage",(int)(gunDamage*damageMod));
				weaponFireRateTimer = weaponFireRate;
				spreadCount++;
				spreadRateTimer = spreadRate;
				ammoCount--;
				sounds.pew ();
			} 
		}
		
		else {}
	}


	/*
	 * Allows the heavy to deploy a force field that decreases the damage received by teammates inside the force field.
	 * Changes the active camera of the sniper.
	 */
	public override void special1()
	{
		if (currentSpecial1 <= 0) {
			currentSpecial1 = special1CD;
			RaycastHit hit;
			Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f, 0.666667f, 0));
			
			Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
			Physics.Raycast (camRay, out hit, weaponRange);
			
			//Debug.Log ("Heavy throwing special at " + hit.transform.gameObject.name);
			
			Vector3 target = hit.point;
			Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
			Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);
			
			Quaternion shieldRotation = Quaternion.identity;
			shieldRotation.SetLookRotation (target - shot_source.position, Vector3.up);
			
			HeavyShieldScript shield = Instantiate (defensePrefab, RHandPos.position, shieldRotation) as HeavyShieldScript;
		}
	}
	
	public override void special2()
	{
		if (currentSpecial2 <= 0) {
			currentSpecial2 = special2CD;
			Debug.Log ("Doing special2");
		}
	}
	
	public override void super()
	{
		if (currentSuper <= 0) {
			currentSuper = superCD;
			Debug.Log ("Doing super");
		}
	}

	public override void rotateCamera(float pitch)
	{
		base.rotateCamera (pitch);
	}
}
