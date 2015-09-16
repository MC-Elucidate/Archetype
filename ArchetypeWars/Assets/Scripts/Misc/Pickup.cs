using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public bool active = true;
	public float respawnTime = 40f, respawnTimer = 0f;
	public float rpm = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, 6*rpm*Time.deltaTime, 0));
	}

	void FixedUpdate()
	{
		if (!active)
			respawnTimer += Time.fixedDeltaTime;

		if (respawnTimer > respawnTime) {
			respawn();
		}
	}

	void respawn()
	{
		active = true;
		collider.enabled = true;
		renderer.enabled = true;
	}

	public void use()
	{
		active = false;
		collider.enabled = false;
		renderer.enabled = false;
		respawnTimer = 0f;
	}
}
