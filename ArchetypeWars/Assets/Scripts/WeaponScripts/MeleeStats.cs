using UnityEngine;
using System.Collections;

public class MeleeStats : MonoBehaviour {

	public int damage;
	public float poisedmg = 30;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		//Deals damage to enemy actors if the melee weapon collides with them.
		if (other.tag == "Enemy") {
			other.gameObject.SendMessage ("receiveDamage", damage);
			other.gameObject.SendMessage ("receivePoiseDamage", poisedmg);
		}
	}
}
