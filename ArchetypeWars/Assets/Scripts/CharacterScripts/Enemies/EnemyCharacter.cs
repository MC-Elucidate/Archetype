using UnityEngine;
using System.Collections;

public class EnemyCharacter : CharacterBase {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "PlayerMelee") {
			receiveDamage (other.gameObject.GetComponent<MeleeStats> ().damage);
			Debug.Log ("hit by heavy");
		}
	}

}
