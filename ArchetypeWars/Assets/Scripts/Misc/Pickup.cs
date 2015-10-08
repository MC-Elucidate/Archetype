using UnityEngine;
using System.Collections;

/*
 * Added to pickup items so that they are able to be collected, and respawned after some time.
 * */
public class Pickup : MonoBehaviour {

	public bool active = true;
	public float respawnTime = 40f, respawnTimer = 0f;
	public float rpm = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Rotates object so it looks like a pickup
		transform.Rotate(new Vector3(0, 6*rpm*Time.deltaTime, 0));
	}

	void FixedUpdate()
	{
		//Counts down till respawn
		if (!active)
			respawnTimer += Time.fixedDeltaTime;

		//Respawns if time is met
		if (respawnTimer > respawnTime) {
			respawn();
		}
	}

	/*
 	* Turns on object's collider and renderer
 	* */
	void respawn()
	{
		active = true;
		collider.enabled = true;
		renderer.enabled = true;
	}

	/*
 	* Turns off object's collder and renderer
 	* */
	public void use()
	{
		active = false;
		collider.enabled = false;
		renderer.enabled = false;
		respawnTimer = 0f;
	}
}
