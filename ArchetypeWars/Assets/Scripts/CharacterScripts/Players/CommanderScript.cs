using UnityEngine;
using System.Collections;

public class CommanderScript: PlayerCharacter {
	
	//Buff
	private float buffDuration = 30, currentBuff = 0;
	
	//Heal
	private int healAmount = 200;

	//Rage Attack
	private int rageDamage = 100, ragePoiseDamage = 15;
	private float rageRadius = 10f;

	// Use this for initialization
	protected void Start () {
		base.Start ();
		health = 150;
		maxHealth = 150;
		runSpeed = 12;
		meleeMax = 3;
		characterRadius = 0.4f;
		aggro = 150;

		//Character-specific weapon stats
		weaponRange = 200f;
		weaponFireRate = 0.1f;
		spreadRate = 0.07f;
		maxSpread = 18;
		gunDamage = 16;
		ammoCount = 120;
		maxAmmo = 120;
		ammoPickup = 40;
		poiseDamage = 20;

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

		//Uses FixedUpdate to countdown to the time where the Buff from her special will be turned off for all players.
		if (currentBuff > 0)
		{
			currentBuff -= Time.fixedDeltaTime;
			if(currentBuff <= 0)
			{
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
				foreach(GameObject player in players)
					player.SendMessage("changeBuffs", "COff");
			}
		}
	}

	/*
	 * Performs a melee attack. Uses base class method
	*/
	public override void meleeAttack()
	{
		base.meleeAttack ();
	}

	/*
	 * Fires weapon. Uses base class method
	*/
	public override void shootWeapon()
	{
		base.shootWeapon ();
	}


	/*
	 * Applies a buff to all players.
	 * Increases their damage and armour
	*/
	public override void special1()
	{
		//Checks cooldown.
		if (currentSpecial1 <= 0) {
			currentSpecial1 = special1CD;
			currentBuff = buffDuration;
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			foreach(GameObject player in players)
				player.SendMessage("changeBuffs", "COn");
			sounds.playSpecial1Sound();
		}
	}

	/*
	 * Heals all players.
	*/
	public override void special2()
	{
		//Checks cooldown.
		if (currentSpecial2 <= 0) {
			currentSpecial2 = special2CD;
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			foreach(GameObject player in players)
				player.SendMessage("receiveHealth", healAmount);
			sounds.playSpecial2Sound();
		}
	}

	/*
	 * Deals damage to all enemies in an area around the commander.
	 */
	public override void super()
	{
		if (currentSuper <= 0) {
			currentSuper = superCD;
			Collider[] hitColliders = Physics.OverlapSphere (transform.position, rageRadius);
			foreach(Collider coll in hitColliders)
			{
				if(coll.tag == "Enemy")
				{
					coll.gameObject.SendMessage("receiveDamage", rageDamage);
					coll.gameObject.SendMessage("receivePoiseDamage", ragePoiseDamage);
				}
			}
			sounds.playSpecial3Sound();
		}
	}

	/*
	 * Rotates camera. Uses base class method.
	*/
	public override void rotateCamera(float pitch)
	{
		base.rotateCamera (pitch);
	}
}
