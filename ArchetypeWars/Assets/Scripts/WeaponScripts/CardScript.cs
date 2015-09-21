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

		Physics.IgnoreCollision (this.collider, FindObjectOfType<NinjaScript>().gameObject.collider, true);	//I don't know how to do this any other way really.
		//Physics.IgnoreCollision (this.collider, GameObject.Find ("bigKATANA_saya").collider, true);
	}
	
	// Update is called once per frame
	void Update () {
		this.rigidbody.velocity = velocity * transform.forward;
		//Debug.DrawRay (transform.position,transform.forward*10, Color.green, 0.1f);
		Destroy (this.gameObject, lifetime);
	}

	void OnCollisionEnter(Collision other) {

		//Debug.Log ("Hit an object");
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyHead") {
			other.gameObject.SendMessage("receiveDamage", damage);//Do damage
			other.gameObject.SendMessage("receivePoiseDamage", poiseDamage);
			Destroy (this.gameObject);
		} 
		else if (other.gameObject.tag == "Wall") {
			ricochet ();
			//transform.forward = other.contacts[0].normal;
			//Debug.DrawRay(other.contacts[0].point,other.contacts[0].normal*5, Color.cyan, 0.5f);
		}
		else if (other.gameObject.tag == "Terrain") {
			Destroy (this.gameObject);
		}
		else {}
	}

	public void ricochet() {

		Debug.Log("Ricochet");
		RaycastHit hit;
		Physics.Raycast(new Ray(transform.position,transform.forward), out hit,5f);
		
		transform.forward = Vector3.Reflect (transform.forward, hit.normal);
		Debug.DrawRay (hit.point, hit.normal * 10, Color.cyan, 0.5f);
	}

	public void setDamage(int dmg)
	{
		damage = dmg;
	}

	public void OnDestroy() {
		Instantiate (cardExplosion, transform.position, Quaternion.identity);
	}
}
