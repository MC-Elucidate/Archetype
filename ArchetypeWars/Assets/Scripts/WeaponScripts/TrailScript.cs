using UnityEngine;
using System.Collections;

public class TrailScript : MonoBehaviour {

	public float velocity = 200, lifetime = 0.2f;

	// Use this for initialization
	void Start () {
		this.rigidbody.AddForce (velocity * transform.forward);
		Destroy (gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
