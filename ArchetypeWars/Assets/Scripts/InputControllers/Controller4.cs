using UnityEngine;
using System.Collections;

public class Controller4 : Movement {

	// Use this for initialization
	protected void Start () {

		base.Start();
		verticalTag = "Vertical 4";
		horizontalTag = "Horizontal 4";
		mouseXTag = "Mouse X 4";
		jumpTag = "Jump 4";
		wallrunTag = "Wallrun 4";
		slideTag = "Slide 4";
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
