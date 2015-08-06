using UnityEngine;
using System.Collections;

public class Controller2 : Movement {

	// Use this for initialization
	protected void Start () {

		base.Start();
		verticalTag = "Vertical 2";
		horizontalTag = "Horizontal 2";
		mouseXTag = "Mouse X 2";
		jumpTag = "Jump 2";
		wallrunTag = "Wallrun 2";
		slideTag = "Slide 2";
		mouseYTag = "Mouse Y 2";
		fireTag = "Fire 2";
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
