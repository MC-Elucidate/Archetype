using UnityEngine;
using System.Collections;

public class EnemyHeavy : EnemyCharacter {

	public Transform rocketPrefab;

	// Use this for initialization
	void Start () {
		base.Start();
		maxHealth = 250;
		health = 250;
		maxPoise = 150;

		weaponRange = 500f;
		weaponFireRate = 2.0f;
		spreadRate = 0.21f;
		maxSpread = 12;
		gunDamage = 40;

		rifleOffSet_pos =  Vector3.up * -2;
		rifleOffSet_rot = new Vector3 (230, 0, -90);
		offSet_rev = 110;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
	public void FixedUpdate(){
		base.FixedUpdate ();
	}

	public void onDestroy(){
		base.OnDestroy();
	}

	public override void meleeAttack ()
	{}

	public override void ShootWeapon(Transform target) {

		if (weaponFireRateTimer <= 0) {
			
			System.Random r = new System.Random();
			//RaycastHit hit;
			//introduce random shooting
			int shoot = r.Next(1,20);
			if (shoot < rage) //successful shooting attempt
				shootSuccessCount++;
			
			if (shootSuccessCount > 2) //control the occurance of shooting
			{
				shootSuccessCount = 0;
				//Physics.Raycast (shot_source.position, target.position + targetOffset * Vector3.up, out hit, weaponRange);
				float distance = Vector3.Distance(target.position, transform.position);
				float error = ((float)r.NextDouble()) * errFactor * distance/shootingRange;
				Vector3 accuracyError =  new Vector3( error, 0, 0);//by how much does the agent miss the target.Directly proportional to the distance from target
					
				Quaternion rocketRotation = Quaternion.identity;
				rocketRotation.SetLookRotation ((target.position + new Vector3(0,1,0)) + accuracyError - shot_source.position, Vector3.up);
				
				//Instatiates the rocket projectile
				Transform rocket;
				rocket = Instantiate (rocketPrefab, shot_source.position, rocketRotation) as Transform;
				//Debug.DrawRay (shot_source.position, hit.point * 10f, Color.green, 0.2f);

				weaponFireRateTimer = weaponFireRate;
				spreadCount++;
				spreadRateTimer = spreadRate;

				sounds.pew();
				rocket.parent = this.transform;
			}
			
		} 
	}
}
