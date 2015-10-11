using UnityEngine;
using System.Collections;

public class EnemyLight : EnemyCharacter {

	// Use this for initialization
	void Start () {
		base.Start();
		maxHealth = 120;
		health = 120;
		
		weaponRange = 500f;
		weaponFireRate = 0.2f;
		spreadRate = 0.21f;
		maxSpread = 12;
		gunDamage = 3;
		
		//Melee
		meleeMax = 1;
		shootingRange = 2;
		stoppingRange = 1;

		enemyType = "Melee";

		SRWeapon.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
	public void FixedUpdate(){
		base.FixedUpdate ();
	}

	public void OnDestroy(){
		base.OnDestroy();
	}

	/*
	 * Overrides all IK methods as the melee class requires no IK
	 * */
	public override void checkIK()
	{}
	
	public void OnAnimatorIK()
	{}

	/*
	 * Overrides the shoot method as the melee class doesn't fire projectiles.
	 * */
	public override void shootWeapon ()
	{

	}
}
