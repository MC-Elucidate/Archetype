using UnityEngine;
using System.Collections;

public class GunCamera : MonoBehaviour {

	public float rotSpeed = 7f;
	public float neckAngleLimit = 40f;

	public GameObject target;
	private Vector3 vec;

	protected string yAxis;
	// Use this for initialization
	void Start () {
		//vec = target.transform.position - transform.position;
	}
	
	// Update is called once per frame
	public void Update () {
		float pitch = Input.GetAxis (yAxis) * rotSpeed;

		if (pitch > 0) { // if we look up
			if(		(transform.localEulerAngles.x > 320) 	|| 	(transform.localEulerAngles.x < 90)		)
				transform.RotateAround (target.transform.position, target.transform.right, -pitch);
		} else if (pitch < 0) { //if we look down
			if(		(transform.localEulerAngles.x > 270) 	|| 	(transform.localEulerAngles.x < 40)		)
				transform.RotateAround (target.transform.position, target.transform.right, -pitch);
		}

	}

}
