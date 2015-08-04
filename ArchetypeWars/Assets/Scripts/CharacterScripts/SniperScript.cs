using UnityEngine;
using System.Collections;

public class SniperScript: CharacterBase {

	// Use this for initialization
	protected void Start () {
		health = 100;
		runSpeed = 10;
		meleeMax = 2;
		characterRadius = 0.4f;
	}
	
	// Update is called once per frame
	protected void Update () {
	
	}

	public override void meleeAttack()
	{
		Debug.Log ("Chessuto!");
	}
}
