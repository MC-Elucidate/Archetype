using UnityEngine;
using System.Collections;

public class EnemyCharacter : CharacterBase {

	public float visionField; //field angle of view
	public float visionRadius;
	public float targetOffset; //offset from target's feet
	
	//random shooting
	protected int shootSuccessCount = 0; //counts the number of successful shooting attempts (before shooting can be done)

	//Shooting distance variables
	public float shootingRange = 25;
	public float stoppingRange = 8;

	//agent attack/retreat attributes
	public int rage; //how aggressive the agent is.max = 10 for now
	public int endurance; //low endurance agent run away after a small amount of damage wss taken
	public int hitCount; //counts how many shots the character has received
	public float errFactor;
	public AI_Logic.Strategy strategy;

	public string enemyType;

	// Use this for initialization
	void Start () {
		base.Start ();
		SRWeapon.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	public void FixedUpdate(){
		base.FixedUpdate ();
	}

	/*
	 * Used to receive damage.
	 * Increases hitcount for the AI to respond to taking damage and then calls base method to apply damage.
	 * */
	public override void receiveDamage(int dmg)
	{
		hitCount++;
		base.receiveDamage (dmg);
	}

	public virtual void ShootWeapon(Transform target)
	{
		if (weaponFireRateTimer <= 0) {

			System.Random r = new System.Random();
			RaycastHit hit;
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
				Physics.Raycast (shot_source.position, (target.position + new Vector3(0,1,0)) + accuracyError - shot_source.position, out hit, weaponRange);
				//Debug.DrawRay (shot_source.position, (target.position + new Vector3(0,1,0)) + accuracyError - shot_source.position, Color.green, 0.8f);
				weaponFireRateTimer = weaponFireRate;
				spreadCount++;
				spreadRateTimer = spreadRate;
				//Debug.Log(hit.transform.gameObject);
				if(hit.transform.gameObject.tag == "Player" || hit.transform.gameObject.tag == "PlayerTeam"){
					hit.transform.gameObject.SendMessage ("receiveDamage", gunDamage, SendMessageOptions.DontRequireReceiver);
					hit.transform.gameObject.SendMessage ("receivePoiseDamage", poiseDamage, SendMessageOptions.DontRequireReceiver);
				}

				sounds.pew();
				Transform fireParticle = Instantiate (weaponFlashEffect, shot_source.transform.position, Quaternion.identity) as Transform;
				fireParticle.parent = this.transform;
			}

		} 
	}

	public virtual void OnDestroy() {
		
		//Not the neatest code, but this should work
		if (RoundManager.currentRound == RoundManager.Round.Survival) {
			RoundManager.enemyCount--;
			//Debug.Log ("Enemy kill confirmed:" + RoundManager.enemyCount + " remaining");
		}

		else if (RoundManager.currentRound == RoundManager.Round.CTF) {
			//Reduce attacker or defender size
			//Debug.Log ("Enemy kill confirmed");
		}
	}

	public override void meleeAttack ()
	{

			if (currentMelee == 0) { //First melee attack in the combo
				currentMelee++;
				melee = true;
				weaponHeld = false;
				SRWeapon.SetActive (true);
				sounds.meleeSound();
			} else if ((currentMelee < meleeMax) && (anim.GetCurrentAnimatorStateInfo (1).IsName ("Attack" + currentMelee))) { //Post-first melee attacks, can only transition into state n+1 if we're in state n
				currentMelee++;
				sounds.meleeSound ();
			}
		
	}

	public override void meleeAttackEnd ()
	{
		melee = false;
		currentMelee = 0;
		SRWeapon.SetActive (false);
		weaponHeld = true;
	}

	public override void enableFreemove()
	{
		gameObject.SendMessage ("Resume", SendMessageOptions.DontRequireReceiver);
		freemove = true;
		weaponHeld = true;
		
		currentPoise = 100;
		anim.SetFloat ("Poise", currentPoise);
	}

}
