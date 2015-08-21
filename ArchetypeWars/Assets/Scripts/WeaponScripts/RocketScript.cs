using UnityEngine;
using System.Collections;

public class RocketScript : MonoBehaviour {

	public float velocity;
	public int damage = 300;

	// Use this for initialization
	void Start () {
		Physics.IgnoreCollision (this.collider, FindObjectOfType<HeavyScript>().gameObject.collider, true);	//I don't know how to do this any other way really.
		this.rigidbody.AddForce (velocity * transform.forward);
		this.rigidbody.AddForce (100f * transform.up);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.DrawRay (transform.position,transform.forward*10, Color.green, 0.1f);
	}

	void OnCollisionEnter(Collision other) {

		//Uncomment this when we have an explosion to instantiate

		//Instantiate (Explosion, transform.position, Quaternion.identity);
		Collider[] hitColliders = Physics.OverlapSphere (transform.position, 5.0f);
		foreach(Collider coll in hitColliders)
		{
			if(coll.tag == "Enemy")
			{
				coll.gameObject.SendMessage("receiveDamage", damage);
				coll.rigidbody.AddExplosionForce(600f, transform.position, 8.0f, 40.0f);
			}
		}
		Destroy (this.gameObject);
	}

}
