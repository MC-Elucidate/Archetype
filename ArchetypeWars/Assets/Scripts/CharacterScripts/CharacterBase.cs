using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour {

	public int health, meleeMax, currentMelee = 0;
	public float runSpeed, characterRadius;
	public bool melee = false;
	// Use this for initialization
	protected void Start () {
	
	}
	
	// Update is called once per frame
	protected void Update () {
	
	}

	public virtual void meleeAttack()
	{
		Debug.Log ("Hyaa!");
	}
}
