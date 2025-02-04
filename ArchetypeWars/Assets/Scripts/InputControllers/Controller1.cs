﻿using UnityEngine;
using System.Collections;

public class Controller1 : MovementController {

	// Use this for initialization
	protected void Start () {
		//Sets variables for player 1
		//Will allow the input controller to know which gamepad's input to use for this character.
		base.Start();
		verticalTag = "Vertical 1";
		horizontalTag = "Horizontal 1";
		mouseXTag = "Mouse X 1";
		jumpTag = "Jump 1";
		wallrunTag = "Wallrun 1";
		slideTag = "Slide 1";
		mouseYTag = "Mouse Y 1";
		fireTag = "Fire 1";
		special1Tag = "Special1 1";
		special2Tag = "Special2 1";
		superTag = "Super 1";
		meleeTag = "Melee 1";
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
