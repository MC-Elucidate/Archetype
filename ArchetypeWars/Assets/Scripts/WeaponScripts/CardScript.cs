using UnityEngine;
using System.Collections;

public class CardScript : MonoBehaviour {

	public float velocity;	//MoveSpeed
	public float lifetime; //Time before destruction
	public int damage = 60;
							//Range = velocity * lifetime, of course
	public float poiseDamage = 15;

	public Transform cardExplosion;
	// Use this for initialization
	void Start () {

		Physics.IgnoreCollision (this.collider, FindObjectOfType<NinjaScript>().gameObject.collider, true);	//Ignores collision with Ninja

	}
	
	// Update is called once per frame
	void Update () {
		this.rigidbody.velocity = velocity * transform.forward;
		Destroy (this.gameObject, lifetime);
	}

	void OnCollisionEnter(Collision other) {

		//When an object is hit: Deal damage if an enemy body or head
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyHead") {
			other.gameObject.SendMessage("receiveDamage", damage);//Do damage
			other.gameObject.SendMessage("receivePoiseDamage", poiseDamage);
			Destroy (this.gameObject);
		} 
		//Bounce off walls
		else if (other.gameObject.tag == "Wall") {
			ricochet ();
		}
		//Destroy if hits terrain
		else if (other.gameObject.tag == "Terrain") {
			Destroy (this.gameObject);
		}
		else {}
	}

	/*
	 * Ricochetes card off wall for cool trickshots.
	 * Refelects off of object hit to determine new direction, using Vector3 reflect method
	 * */
	public void ricochet() {

		RaycastHit hit;
		Physics.Raycast(new Ray(transform.position,transform.forward), out hit,5f);
		
		transform.forward = Vector3.Reflect (transform.forward, hit.normal);
	}

	/*
	 * Sets damage of card.
	 * Used to apply buffs.
	 * */
	public void setDamage(int dmg)
	{
		damage = dmg;
	}

	/*
	 * Destroy card object.
	 * */
	public void OnDestroy() {
		Instantiate (cardExplosion, transform.position, Quaternion.identity);
	}
}
