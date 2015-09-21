using UnityEngine;
using System.Collections;

public class MeleeStats : MonoBehaviour {

	public int damage;
	public float poisedmg = 90;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy") {
			//Debug.Log ("Smack");
			other.gameObject.SendMessage ("receiveDamage", damage);
			other.gameObject.SendMessage ("receivePoiseDamage", poisedmg);
		}
	}
}
