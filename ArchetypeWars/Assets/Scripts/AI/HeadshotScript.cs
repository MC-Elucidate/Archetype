using UnityEngine;
using System.Collections;

public class HeadshotScript : MonoBehaviour {

	public float multiplier = 1.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void receiveDamage(int dmg)
	{
		int newDamage = (int)(dmg * multiplier);
		transform.parent.SendMessage ("receiveDamage", newDamage);
	}

	public void receivePoiseDamage(int dmg)
	{
		int newDamage = (int)(dmg * multiplier);
		transform.parent.SendMessage ("receivePoiseDamage", newDamage);
	}
}
