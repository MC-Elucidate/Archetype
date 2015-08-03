using UnityEngine;
using System.Collections;

public class Controller3 : Movement {

	// Use this for initialization
	protected void Start () {

		base.Start();
		verticalTag = "Vertical 3";
		horizontalTag = "Horizontal 3";
		mouseXTag = "Mouse X 3";
		jumpTag = "Jump 3";
		wallrunTag = "Wallrun 3";
		slideTag = "Slide 3";
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
