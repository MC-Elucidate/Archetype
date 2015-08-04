using UnityEngine;
using System.Collections;

public class HeavyScript: CharacterBase {

	// Use this for initialization
	protected void Start () {
		health = 200;
		runSpeed = 10;
		meleeMax = 4;
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
