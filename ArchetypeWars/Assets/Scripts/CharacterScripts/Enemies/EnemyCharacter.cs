using UnityEngine;
using System.Collections;

public class EnemyCharacter : CharacterBase {

	public float visionField; //field angle of view
	public float visionRadius;
	public float targetOffset; //offset from target's feet
	
	//random shooting
	protected float sC, sT;
	//protected System.Random r;

	//Shooting distance variables
	public float shootingRange = 10;
	public float stoppingRange = 5;


	// Use this for initialization
	void Start () {
		base.Start ();
		sT = 0; //shoot time out
		sC = 0;
		//r = new System.Random();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	public void FixedUpdate(){
		base.FixedUpdate ();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "PlayerMelee") {
			MeleeStats ms = other.gameObject.GetComponent<MeleeStats> ();
			receiveDamage (ms.damage);
			receivePoiseDamage(ms.poisedmg);
		}
	}

	public virtual void ShootWeapon(Transform target)
	{
		if (weaponFireRateTimer <= 0) {

			System.Random r = new System.Random();
			RaycastHit hit;
			//introduce random shooting
			int shoot = r.Next(1,10);
			//if (sT < 1)
			if (shoot == 7)
			{
				sT += Time.deltaTime;
				//Physics.Raycast (shot_source.position, target.position + targetOffset * Vector3.up, out hit, weaponRange);
				Physics.Raycast (shot_source.position, (target.position + new Vector3(0,1,0)) - shot_source.position, out hit, weaponRange);
				Debug.DrawRay (shot_source.position, (target.position + new Vector3(0,1,0)) - shot_source.position, Color.green, 0.8f);
				weaponFireRateTimer = weaponFireRate;
				spreadCount++;
				spreadRateTimer = spreadRate;
				Debug.Log(hit.transform.gameObject);
				if(hit.transform.gameObject.tag == "Player" || hit.transform.gameObject.tag == "PlayerTeam"){
					hit.transform.gameObject.SendMessage ("receiveDamage", gunDamage, SendMessageOptions.DontRequireReceiver);
					hit.transform.gameObject.SendMessage ("receivePoiseDamage", poiseDamage, SendMessageOptions.DontRequireReceiver);
				}
				//print ("m shooting");
				sounds.pew();
			}

			//to reduce the occurance of shooting
			if (shoot < 6) 
			{
				sC ++;		
			}
			if (sC >12)
			{
				sT = 0;
				sC = 0;
			}
		} 
	}

	public virtual void OnDestroy() {
		
		//Not the neatest code, but this should work
		if (RoundManager.currentRound == RoundManager.Round.Survival) {
			RoundManager.enemyCount--;
			Debug.Log ("Enemy kill confirmed:" + RoundManager.enemyCount + " remaining");
		}

		else if (RoundManager.currentRound == RoundManager.Round.CTF) {
			//Reduce attacker or defender size
			Debug.Log ("Enemy kill confirmed");
		}
	}

}
