using UnityEngine;
using System.Collections;

public class CommanderScript: CharacterBase {

	// Use this for initialization
	protected void Start () {
		health = 150;
		runSpeed = 12;
		meleeMax = 3;
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
