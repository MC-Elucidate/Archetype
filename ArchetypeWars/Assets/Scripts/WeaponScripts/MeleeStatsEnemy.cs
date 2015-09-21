using UnityEngine;
using System.Collections;

public class MeleeStatsEnemy : MonoBehaviour {

	public int damage;
	public float poisedmg = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			other.gameObject.SendMessage ("receiveDamage", damage);
			other.gameObject.SendMessage ("receivePoiseDamage", poisedmg);
		}
	}
}
