using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour {

	//health
	protected int maxHealth;
	public int health;

	//Weapon stuff
	protected float weaponRange = 500f, weaponFireRate, weaponFireRateTimer = 0f, spreadRate, spreadRateTimer = 0f, meleeFireRate, meleeFireRateTimer = 0f;
	protected int ammoCount, maxAmmo, ammoPickup; 			//ammoPickup = how much ammo you get back from pickup
	protected int spreadCount = 0, maxSpread;				//Accuracy value (pixel range from centre)
	protected int meleeMax;
	public int currentMelee = 0;
	protected int gunDamage;
	public Transform shot_source;
	public Transform weaponFlashEffect;

	//IK stuff
	public bool useIK = false;
	public bool leftHandIK = true;
	public bool rightHandIK = true;
	public Transform LHandPos,RHandPos, RightHand;
	public GameObject LRWeapon;
	protected Animator anim;
	protected bool weaponDrawn = false;

	//Used to make drawing and concealing weapon look smooth. Not used at the moment
	protected bool an_Set = false;
	protected float an_dt = 0.0f;

	//Melee stuff
	public GameObject SRWeapon;

	//Sound Stuff
	public SoundPool sounds;

	
	//Poise
	protected float maxPoise = 100f;
	public float currentPoise = 100f;
	protected float poiseDamage = 15f;


	//Stuff for movement controller
	public bool freemove = true, melee = false, alive = true, weaponHeld = true;
	public float runSpeed, characterRadius, floorcast = 0.12f;

	//Aggro
	public int aggro = 0;


	// Use this for initialization
	public void Start () {
		anim = gameObject.GetComponent<Animator> ();
		sounds = gameObject.GetComponent<SoundPool> ();
	}
	
	// Update is called once per frame
	public void Update () {

		
		//Used to control when the next bullet can be fired. Sort of a cooldown for firing.
		if (weaponFireRateTimer>0)
			weaponFireRateTimer -= Time.deltaTime;

		//Used to control when bullet spread is decreased after releasing the fire button.
		if (spreadRateTimer>0)
			spreadRateTimer -= Time.deltaTime;

		//Prevents bullet spread from going too crazy
		if (spreadCount > maxSpread)
			spreadCount = maxSpread;

		//Decreases spread after some time of not firing.
		if (spreadRateTimer <= 0) {
			if (spreadCount>0)
			{
				spreadCount--;
				spreadRateTimer=spreadRate;
			}
		}

		//Restores poise and the ability to move after being stunned or knocked down.
		if (currentPoise < 5.0 && !anim.GetCurrentAnimatorStateInfo (0).IsTag ("NoFreeMove")) {
			currentPoise = 40;
			freemove = true;
		} else if (currentPoise > 40.0 && alive)
			freemove = true;

		checkIK ();
	
	}

	public void FixedUpdate()
	{
		//Increase Poise over time. 1 second = 15 poise;
		currentPoise += Time.fixedDeltaTime*15;
		if (currentPoise > maxPoise)
			currentPoise = maxPoise;
	}

	
	/*
	 * Performs melee attack.
	 * To be overridden.
	 * */
	public virtual void meleeAttack()
	{
		//Debug.Log ("Hyaa!");
	}

	/*
	 * Ends melee attack.
	 * To be overridden.
	 * */
	public virtual void meleeAttackEnd()
	{
	}

	/*
	 * Fires weapon.
	 * To be overridden.
	 * */
	public virtual void shootWeapon() {
	}

	/*
	 * Receives damage.
	 * Sets character to dead state if health reaches 0.
	 * Prevents character from moving if dead.
	 * */
	public virtual void receiveDamage(int dmg)
	{
		health -= dmg;
		if (health <= 0) {
			alive = false;
			Destroy (this.gameObject, 2.0f);
			freemove = false;
		}
	}

	/*
	 * Receives poise damage.
	 * Puts character in stunned or knocked down state if poise damage exceeds certain threshold.
	 * */
	public void receivePoiseDamage(float poisedmg)
	{
		if(alive){
			currentPoise -= poisedmg;
			//Knocked down
			if(currentPoise <= 20)
			{
				freemove = false;
				//Debug.Log("KD");
			}
			//Stunned
			else if(currentPoise <= 40 && currentPoise > 20)
			{
				freemove = false;
				//Debug.Log("Stunned");
			}
			anim.SetFloat("Poise", currentPoise);
		}
	}

	/*
	 * Receives health.
	 * Adds 'up' amount of health and prevents it from going over a limit.
	 * */
	public void receiveHealth(int up)
	{
		health += up;
		if (health >= maxHealth)
			health = maxHealth;
	}

	/*
	 * Returns aggro value.
	 * Used by AI to determine target.
	 * */
	public int getAggro()
	{
		return aggro;
	}

	/*
	 * Positions hands on gun so it appears that the character is holding the weapon.
	 * */
	void OnAnimatorIK()
	{
		if (useIK)
		{
			//Attach left hand
			if (leftHandIK)
			{
				anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
				anim.SetIKPosition(AvatarIKGoal.LeftHand, LHandPos.transform.position);
				
				
			}

			//Attach right hand
			if (rightHandIK)
			{
				anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
				anim.SetIKPosition(AvatarIKGoal.RightHand, RHandPos.transform.position);
				
			}
			
		}
	}

	/*
	 * Checks if the gun is held.
	 * Gun is not held when doing a melee, and held in every other circumstance.
	 * */
	public virtual void checkIK()
	{
		if (weaponHeld == true)
		{
			
			AnimatorStateInfo animPlayingState = anim.GetCurrentAnimatorStateInfo(1);
			if (animPlayingState.IsName("Aim"))
			{
				float playbackTime = animPlayingState.normalizedTime % 1;

				if ((an_dt > 0.01f) && (an_Set == false)) //Smoothing for when to equip the gun
				{
					//Positions gun correctly
					LRWeapon.SetActive (true);
					LRWeapon.transform.localPosition = Vector3.zero;
					LRWeapon.transform.localRotation = Quaternion.identity;
					LRWeapon.transform.Rotate(220, 0, -90);
					LRWeapon.transform.RotateAround (RHandPos.position, transform.right, 75);
					
					an_Set = true;
					weaponDrawn = true;
					useIK = true;
					anim.SetLayerWeight(1, 1.0f);
				}
				
				an_dt += Time.deltaTime;
			}
			
		}
		
		if (weaponHeld == false)
		{
			//hides weapon
			if (weaponDrawn)
			{
				LRWeapon.SetActive (false);
				useIK = false;
				an_Set = false;
				an_dt = 0f;
				weaponDrawn = false;
			}
			
		}
	}



}
