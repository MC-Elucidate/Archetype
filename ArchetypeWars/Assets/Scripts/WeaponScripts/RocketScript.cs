using UnityEngine;
using System.Collections;

public class RocketScript : MonoBehaviour {

	//Rocket variables
	public float velocity;
	public int damage = 90;
	public float poiseDamage = 70;
	public float explosionRadius = 5;
	public Transform explosionParticles;


	// Use this for initialization
	void Start () {
		Physics.IgnoreCollision (this.collider, FindObjectOfType<HeavyScript>().gameObject.collider, true);	//Prevent from colliding with Heavy.

		//Set starting velocity
		this.rigidbody.AddForce (velocity * transform.forward);
		this.rigidbody.AddForce (100f * transform.up);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision other) {

		//Find all enemies in the explosion radius and apply damage and poise damage to them.
		Collider[] hitColliders = Physics.OverlapSphere (transform.position, explosionRadius);
		foreach(Collider coll in hitColliders)
		{
			if(coll.tag == "Enemy")
			{
				if (!Physics.Linecast(transform.position, coll.transform.position))
				{
					coll.gameObject.SendMessage("receiveDamage", damage);
					coll.gameObject.SendMessage("receivePoiseDamage", poiseDamage);
				}
			}
		}
		Destroy (this.gameObject);
	}

	/*
	 * Used to change the damage of the rocket.
	 * Used when buffs to damage are applied.
	 * */
	public void setDamage(int dmg)
	{
		damage = dmg;
	}

	public void OnDestroy() {
		//create explosion particle effect
		Transform explosion = Instantiate (explosionParticles, this.transform.position, Quaternion.identity) as Transform; //Edit the quaternion here to rotate the particle effect
	}

}
