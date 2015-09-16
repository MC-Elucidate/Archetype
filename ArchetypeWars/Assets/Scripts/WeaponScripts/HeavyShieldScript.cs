using UnityEngine;
using System.Collections;

public class HeavyShieldScript : MonoBehaviour {

	public float velocity;
	public float lifetime = 20f;
	public GameObject[] players;

	//Collisions cause trouble with the shooting raycasts. 
	//We can rather store character positions here and measure distance between them in the update or once a second.
	//If they're less than the sphere of influence's distance, then give them a buff. 

	// Use this for initialization
	void Start () {
		this.rigidbody.AddForce (velocity * transform.forward);
		this.rigidbody.AddForce (100f * transform.up);
		players = GameObject.FindGameObjectsWithTag ("Player");
		Destroy (this.gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
		/*
		foreach (GameObject player in players) {
		
			//Debug.Log (player.name + " is " + (player.transform.position - this.transform.position).magnitude + " units away from the shield");
			if ((player.transform.position - this.transform.position).magnitude <= 8)
				Debug.Log ("Give shield buff");
		}*/
	}

	void OnDestroy()
	{
		foreach (GameObject p in players)
			p.SendMessage ("changeBuffs", "SOff");
	}
}
