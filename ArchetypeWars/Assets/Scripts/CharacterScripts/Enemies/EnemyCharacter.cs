using UnityEngine;
using System.Collections;

public class EnemyCharacter : CharacterBase {

	public float visionField; //field angle of view
	public float visionRadius;
	public float targetOffset; //offset from target's feet
	
	//random shooting
	protected float sC, sT;
	protected System.Random r;


	// Use this for initialization
	void Start () {
		base.Start ();
		sT = 0; //shoot time out
		sC = 0;
		r = new System.Random();
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

			RaycastHit hit;
			//introduce random shooting
			int shoot = r.Next(1,10);
			if (sT < 1)
			{
				sT += Time.deltaTime;
				Physics.Raycast (shot_source.position, target.position + targetOffset * Vector3.up, out hit, weaponRange);
				
				weaponFireRateTimer = weaponFireRate;
				spreadCount++;
				spreadRateTimer = spreadRate;
				if(hit.transform.gameObject.tag == "Player"){
					hit.transform.gameObject.SendMessage ("receiveDamage", gunDamage, SendMessageOptions.DontRequireReceiver);
					hit.transform.gameObject.SendMessage ("receiveDamage", poiseDamage, SendMessageOptions.DontRequireReceiver);
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

}
