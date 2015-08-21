using UnityEngine;
using System.Collections;

public class HeavyScript: PlayerCharacter {

	public Transform rocketPrefab;
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
	}
	

	// Update is called once per frame
	protected void Update () {
		base.Update ();
	}

	public override void meleeAttack()
	{
		base.meleeAttack ();
	}

	public override void shootWeapon()
	{
		if (weaponFireRateTimer <= 0) {
			
			
			RaycastHit hit;
			Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f + Random.Range (-spreadCount*spreadFactor,spreadCount*spreadFactor),  0.666667f + Random.Range (-spreadCount*spreadFactor,spreadCount*spreadFactor), 0));
			
			//Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
			Physics.Raycast (camRay, out hit, weaponRange);
			
			//Debug.Log ("Heavy Shooting at " + hit.transform.gameObject.name);
			
			Vector3 target = hit.point;
			Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
			//Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);
			
			Quaternion rocketRotation = Quaternion.identity;
			rocketRotation.SetLookRotation(target - shot_source.position, Vector3.up);
			
			RocketScript card = Instantiate (rocketPrefab, shot_source.position, rocketRotation) as RocketScript;
			
			weaponFireRateTimer = weaponFireRate;
			spreadCount++;
			spreadRateTimer = spreadRate;
			
		} 
		
		else {}
	}

	public override void special1()
	{
		Debug.Log ("Doing special1");
	}
	
	public override void special2()
	{
		Debug.Log ("Doing special2");
	}
	
	public override void super()
	{
		Debug.Log ("Doing super");
	}
	
	public override void dash()
	{//generic dash code
		Debug.Log ("Doing dash");
	}
	
	public override void rotateCamera(float pitch)
	{
		base.rotateCamera (pitch);
	}
}
