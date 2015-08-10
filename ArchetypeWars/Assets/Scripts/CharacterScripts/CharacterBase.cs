using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour {

	protected int health, maxHealth;

	//Weapon stuff
	protected float weaponRange = 100f, weaponFireRate, weaponFireRateTimer = 0f, spreadRate, spreadRateTimer = 0f, meleeFireRate, meleeFireRateTimer = 0f;
	protected int ammoCount, maxAmmo, ammoPickup; 			//ammoPickup = how much ammo you get back from pickup
	protected int spreadCount = 0, maxSpread;				//Accuracy value (pixel range from centre)
	protected int meleeMax, currentMelee = 0;
	public Transform shot_source;


	//Stuff for movement controller
	public bool melee = false, alive = true;
	public float runSpeed, characterRadius, floorcast = 0.05f;


	// Use this for initialization
	public void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {

		if (weaponFireRateTimer>0)
			weaponFireRateTimer -= Time.deltaTime;

		if (spreadRateTimer>0)
			spreadRateTimer -= Time.deltaTime;

		if (spreadCount > maxSpread)
			spreadCount = maxSpread;

		if (spreadRateTimer <= 0) {
			if (spreadCount>0)
			{
				Debug.Log ("Counting down spread");
				spreadCount--;
				spreadRateTimer=spreadRate;
			}
		}
	
	}

	public virtual void meleeAttack()
	{
		Debug.Log ("Hyaa!");
	}

	public virtual void shootWeapon() {
		//Get the gameObject that GunScript is attached to, then find the camera attached to that child.
		//Take the screen point that we want to use as the point we're going to shoot towards
		/*
		if (weaponFireRateTimer <= 0) {
		
			Ray camRay = cam.ScreenPointToRay (new Vector3 (Screen.width / 2 + Random.Range (-spreadCount, spreadCount),  Screen.height * 2 / 3 + Random.Range (-spreadCount, spreadCount), 0));
			Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
			Physics.Raycast (camRay, out hit, weaponRange);
		
			//Debug.Log ("Shooting at " + hit.transform.gameObject.name);
		
			Vector3 target = hit.point;
			Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
			Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);

			weaponFireRateTimer = weaponFireRate;
			spreadCount++;
			spreadRateTimer = spreadRate;

		} 
		else {}*/
	}



	public virtual void dash()
	{//generic dash code
	}



	public void receiveDamage(int dmg)
	{
		health -= dmg;
		if (health <= 0)
			alive = false;
	}

	public void receiveHealth(int up)
	{
		health += up;
		if (health >= maxHealth)
			health = maxHealth;
	}

}
