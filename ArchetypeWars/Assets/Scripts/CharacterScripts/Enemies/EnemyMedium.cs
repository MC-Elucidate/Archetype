using UnityEngine;
using System.Collections;

public class EnemyMedium : EnemyCharacter {

	// Use this for initialization
	void Start () {
		base.Start();
		maxHealth = 160;
		health = 160;

		weaponRange = 500f;
		weaponFireRate = 0.2f;
		spreadRate = 0.21f;
		maxSpread = 12;
		gunDamage = 10;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
	void FixedUpdate(){
		base.FixedUpdate ();
	}

	void OnDestroy(){
		base.OnDestroy();
	}
}
