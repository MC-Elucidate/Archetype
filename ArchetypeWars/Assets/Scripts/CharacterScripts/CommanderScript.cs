using UnityEngine;
using System.Collections;

public class CommanderScript: CharacterBase {

	// Use this for initialization
	protected void Start () {
		health = 150;
		runSpeed = 12;
		meleeMax = 3;
		characterRadius = 0.4f;

		//Character-specific weapon stats
		weaponRange = 100f;
		weaponFireRate = 0.2f;
		spreadRate = 0.21f;
		maxSpread = 12;
	}
	
	// Update is called once per frame
	protected void Update () {
		base.Update ();
	}

	public override void meleeAttack()
	{
		Debug.Log ("Chessuto!");
	}

	public override void shootWeapon()
	{
		base.shootWeapon ();
	}

	public override void special1()
	{
		Debug.Log ("Doing special1");
	}
	
	public override void special2()
	{
		Debug.Log ("Doing special2");
	}
	
	public override void super()
	{
		Debug.Log ("Doing super");
	}
	
	public override void dash()
	{//generic dash code
		Debug.Log ("Doing dash");
	}
	
	public override void rotateCamera(float pitch)
	{
		base.rotateCamera (pitch);
	}
}
