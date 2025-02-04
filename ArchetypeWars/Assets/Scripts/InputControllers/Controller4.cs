﻿using UnityEngine;
using System.Collections;

public class Controller4 : MovementController {

	// Use this for initialization
	protected void Start () {
		//Sets variables for player 4
		//Will allow the input controller to know which gamepad's input to use for this character
		base.Start();
		verticalTag = "Vertical 4";
		horizontalTag = "Horizontal 4";
		mouseXTag = "Mouse X 4";
		jumpTag = "Jump 4";
		wallrunTag = "Wallrun 4";
		slideTag = "Slide 4";
		mouseYTag = "Mouse Y 4";
		fireTag = "Fire 4";
		special1Tag = "Special1 4";
		special2Tag = "Special2 4";
		superTag = "Super 4";
		meleeTag = "Melee 4";
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
