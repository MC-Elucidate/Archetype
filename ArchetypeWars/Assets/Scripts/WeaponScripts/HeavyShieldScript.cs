using UnityEngine;
using System.Collections;


/*
 * Acts as a trigger that, when entered, gives a buff to the players. Code for buff is in player class
 * When left, Buff is removed.
 * Buff is aso removed when the forcefield expires.
 * */
public class HeavyShieldScript : MonoBehaviour {

	public float velocity;
	public float lifetime = 20f;
	public GameObject[] players;


	// Use this for initialization
	void Start () {
		this.rigidbody.AddForce (velocity * transform.forward);
		this.rigidbody.AddForce (100f * transform.up);
		players = GameObject.FindGameObjectsWithTag ("Player");
		Destroy (this.gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnDestroy()
	{
		foreach (GameObject p in players)
			p.SendMessage ("changeBuffs", "SOff");
	}
}
