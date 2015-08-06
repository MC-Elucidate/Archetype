using UnityEngine;
using System.Collections;

public class NinjaScript: CharacterBase {

	// Use this for initialization
	protected void Start () {
		health = 150;
		runSpeed = 16;
		meleeMax = 7;
		characterRadius = 0.4f;
	}
	
	// Update is called once per frame
	protected void Update () {
	
	}

	public override void meleeAttack()
	{
		Debug.Log ("Chessuto!");
	}

	public override void shootWeapon()
	{
		base.shootWeapon ();
	}
}
