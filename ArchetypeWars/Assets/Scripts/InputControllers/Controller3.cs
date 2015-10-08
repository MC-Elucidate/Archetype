using UnityEngine;
using System.Collections;

public class Controller3 : MovementController {

	// Use this for initialization
	protected void Start () {
		//Sets variables for player 3
		//Will allow the input controller to know which gamepad's input to use for this character
		base.Start();
		verticalTag = "Vertical 3";
		horizontalTag = "Horizontal 3";
		mouseXTag = "Mouse X 3";
		jumpTag = "Jump 3";
		wallrunTag = "Wallrun 3";
		slideTag = "Slide 3";
		mouseYTag = "Mouse Y 3";
		fireTag = "Fire 3";
		special1Tag = "Special1 3";
		special2Tag = "Special2 3";
		superTag = "Super 3";
		meleeTag = "Melee 3";
	}
	
	// Update is called once per frame
	protected void Update () {
		base.Update ();
	}

	protected void FixedUpdate()
	{
		base.FixedUpdate ();
	}

	protected void OnControllerColliderHit(ControllerColliderHit collision) {
		base.OnControllerColliderHit (collision);
	}

}
