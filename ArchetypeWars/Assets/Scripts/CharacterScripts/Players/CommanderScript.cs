using UnityEngine;
using System.Collections;

public class CommanderScript: PlayerCharacter {
	
	//Buff
	private float buffDuration = 30, currentBuff = 0;
	
	//Heal
	private int healAmount = 200;
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
		weaponFireRate = 0.1f;
		spreadRate = 0.07f;
		maxSpread = 12;
		gunDamage = 16;
		ammoCount = 120;
		maxAmmo = 120;
		ammoPickup = 40;
		poiseDamage = 8;

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
		if (currentBuff > 0)
		{
			currentBuff -= Time.fixedDeltaTime;
			if(currentBuff <= 0)
			{
				foreach(Transform player in RoundManager.players)
					player.gameObject.SendMessage("changeBuffs", "COff");
			}
		}
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
			currentBuff = buffDuration;
			foreach(Transform player in RoundManager.players)
				player.gameObject.SendMessage("changeBuffs", "COn");
			sounds.playSpecial1Sound();
		}
	}
	
	public override void special2()
	{
		if (currentSpecial2 <= 0) {
			currentSpecial2 = special2CD;
			foreach(Transform player in RoundManager.players)
				player.gameObject.SendMessage("receiveHealth", healAmount);
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
