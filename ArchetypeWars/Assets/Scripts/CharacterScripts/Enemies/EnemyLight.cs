using UnityEngine;
using System.Collections;

public class EnemyLight : EnemyCharacter {

	// Use this for initialization
	void Start () {
		base.Start();
		maxHealth = 120;
		health = 120;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
	public void FixedUpdate(){
		base.FixedUpdate ();
	}
}
