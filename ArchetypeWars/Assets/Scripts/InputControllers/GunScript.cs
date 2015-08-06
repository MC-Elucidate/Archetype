using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {

	public float weapon_range;
	public Transform shot_source;
	RaycastHit hit;

	// Use this for initialization
	void Start () {

		weapon_range = 100f;
	
	}
	
	// Update is called once per frame
	void Update () {

		//Set the button you want to pick up for the thing
		//Probably do this somewhere else.

		if (Input.GetAxis("Fire 1") > 0) { //Axis > 0 = R2, Axis < 0 = L2 (when inverted)
			shootWeapon ();
		}

	}

	public void shootWeapon() {
		//Get the gameObject that GunScript is attached to, then find the camera attached to that child.
		//Take the screen point that we want to use as the point we're going to shoot towards

		Ray camRay = this.gameObject.GetComponentInChildren<Camera> ().ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height *2/3, 0));
		Debug.DrawRay (camRay.origin, camRay.direction*10f, Color.yellow, 0.1f);
		Physics.Raycast (camRay, out hit, weapon_range);

		//Debug.Log ("Shooting at " + hit.transform.gameObject.name);

		Vector3 target = hit.point;
		Physics.Raycast(shot_source.position, target-shot_source.position, out hit, weapon_range);
		Debug.DrawRay (shot_source.position, target-shot_source.position, Color.green,0.1f);
	}
}
