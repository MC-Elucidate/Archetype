using UnityEngine;
using System.Collections;

public class EnemyHeavy : EnemyCharacter {

	// Use this for initialization
	void Start () {
		base.Start();
		maxHealth = 250;
		health = 250;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
	public void FixedUpdate(){
		base.FixedUpdate ();
	}
}
