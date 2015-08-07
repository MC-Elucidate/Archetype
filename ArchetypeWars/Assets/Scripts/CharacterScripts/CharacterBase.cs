using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour {

	public int health, meleeMax, currentMelee = 0;
	public float runSpeed, characterRadius, floorcast = 0.05f;

	//Weapon stuff
	public float weaponRange = 100f, weaponFireRate, weaponFireRateTimer = 0f, spreadRate, spreadRateTimer = 0f, meleeFireRate, meleeFireRateTimer = 0f;
	public int ammoCount, maxAmmo, ammoPickup; 			//ammoPickup = how much ammo you get back from pickup
	public int spreadCount = 0, maxSpread;				//Accuracy value (pixel range from centre)

	public bool melee = false;
	public Camera cam;
	public Transform shot_source;
	RaycastHit hit;
	// Use this for initialization
	protected void Start () {
	
	}
	
	// Update is called once per frame
	protected void Update () {

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
		else {}
	}

	public virtual void special1()
	{}

	public virtual void special2()
	{}

	public virtual void super()
	{}

	public virtual void dash()
	{//generic dash code
	}

	public virtual void rotateCamera(float pitch)
	{
		if (pitch > 0) { // if we look up
			if(		(cam.transform.localEulerAngles.x > 320) 	|| 	(cam.transform.localEulerAngles.x < 90)		)
				cam.transform.RotateAround (transform.position, transform.right, -pitch);
		} else if (pitch < 0) { //if we look down
			if(		(cam.transform.localEulerAngles.x > 270) 	|| 	(cam.transform.localEulerAngles.x < 40)		)
				cam.transform.RotateAround (transform.position, transform.right, -pitch);
		}
	}

}
