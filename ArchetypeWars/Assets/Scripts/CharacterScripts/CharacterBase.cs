using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour {

	public int health, meleeMax, currentMelee = 0;
	public float runSpeed, characterRadius, floorcast = 0.05f;
	public bool melee = false;
	public Camera cam;
	public float weapon_range = 100f;
	public Transform shot_source;
	RaycastHit hit;
	// Use this for initialization
	protected void Start () {
	
	}
	
	// Update is called once per frame
	protected void Update () {
	
	}

	public virtual void meleeAttack()
	{
		Debug.Log ("Hyaa!");
	}

	public virtual void shootWeapon() {
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
