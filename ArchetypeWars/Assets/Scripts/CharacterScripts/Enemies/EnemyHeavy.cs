using UnityEngine;
using System.Collections;

public class EnemyHeavy : EnemyCharacter {

	// Use this for initialization
	void Start () {
		base.Start();
		maxHealth = 250;
		health = 250;

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
}
