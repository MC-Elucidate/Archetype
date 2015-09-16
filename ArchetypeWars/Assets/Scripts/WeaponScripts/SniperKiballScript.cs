using UnityEngine;
using System.Collections;

public class SniperKiballScript : MonoBehaviour {


	public float velocity;
	public float lifetime = 20f;
	public float damage;

	// Use this for initialization
	void Start () {
		this.rigidbody.AddForce (velocity * transform.forward);
		Destroy (this.gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Enemy")
			other.gameObject.SendMessage ("receiveDamage", damage);

	}
}
