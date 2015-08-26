using UnityEngine;
using System.Collections;

public class HeavyShieldScript : MonoBehaviour {

	public float velocity;
	public float lifetime = 20f;

	//Collisions cause trouble with the shooting raycasts. 
	//We can rather store character positions here and measure distance between them in the update or once a second.
	//If they're less than the sphere of influence's distance, then give them a buff. 

	// Use this for initialization
	void Start () {
		this.rigidbody.AddForce (velocity * transform.forward);
		this.rigidbody.AddForce (100f * transform.up);
		Destroy (this.gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: Implement this
	}
}
