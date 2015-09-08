﻿using UnityEngine;
using System.Collections;

public class CommanderScript: PlayerCharacter {

	// Use this for initialization
	protected void Start () {
		base.Start ();
		health = 150;
		maxHealth = 150;
		runSpeed = 12;
		meleeMax = 3;
		characterRadius = 0.4f;

		//Character-specific weapon stats
		weaponRange = 200f;
		weaponFireRate = 0.2f;
		spreadRate = 0.21f;
		maxSpread = 12;
		gunDamage = 30;
		ammoCount = 90;
		maxAmmo = 90;
		ammoPickup = 30;

		//Special cooldowns
		special1CD = 90f;
		special2CD = 90f;
		superCD = 90f;
	}
	
	// Update is called once per frame
	protected void Update () {
		base.Update ();
	}
	public void FixedUpdate(){
		base.FixedUpdate ();
	}
	public override void meleeAttack()
	{
		base.meleeAttack ();
	}

	public override void shootWeapon()
	{
		base.shootWeapon ();
	}

	public override void special1()
	{
		if (currentSpecial1 <= 0) {
			currentSpecial1 = special1CD;
			Debug.Log ("Doing special1");
			sounds.playSpecial1Sound();
		}
	}
	
	public override void special2()
	{
		if (currentSpecial2 <= 0) {
			currentSpecial2 = special2CD;
			Debug.Log ("Doing special2");
			sounds.playSpecial2Sound();
		}
	}
	
	public override void super()
	{
		if (currentSuper <= 0) {
			currentSuper = superCD;
			Debug.Log ("Doing super");
			sounds.playSpecial3Sound();
		}
	}

	
	public override void rotateCamera(float pitch)
	{
		base.rotateCamera (pitch);
	}
}
