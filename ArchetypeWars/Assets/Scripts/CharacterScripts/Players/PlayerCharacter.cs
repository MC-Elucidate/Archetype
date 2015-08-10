using UnityEngine;
using System.Collections;

public class PlayerCharacter : CharacterBase {

	public Camera cam;
	protected int gunDamage, meleeDamage;
	protected RaycastHit hit;
	protected float spreadFactor = 0.003f;
	// Use this for initialization
	public void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public void Update () {
		base.Update ();
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

	public virtual void shootWeapon() {
		//Get the gameObject that GunScript is attached to, then find the camera attached to that child.
		//Take the screen point that we want to use as the point we're going to shoot towards
		
		if (weaponFireRateTimer <= 0) {
			
			//Ray camRay = cam.ScreenPointToRay (new Vector3 (Screen.width / 2 + Random.Range (-spreadCount, spreadCount),  Screen.height * 2 / 3 + Random.Range (-spreadCount, spreadCount), 0));
			Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f + Random.Range (-spreadCount*spreadFactor,spreadCount*spreadFactor),  0.666667f + Random.Range (-spreadCount*spreadFactor,spreadCount*spreadFactor), 0));
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
}
