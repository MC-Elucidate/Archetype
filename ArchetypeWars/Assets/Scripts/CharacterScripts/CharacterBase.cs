using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour {

	protected int maxHealth;
	public int health;

	//Weapon stuff
	protected float weaponRange = 100f, weaponFireRate, weaponFireRateTimer = 0f, spreadRate, spreadRateTimer = 0f, meleeFireRate, meleeFireRateTimer = 0f;
	protected int ammoCount, maxAmmo, ammoPickup; 			//ammoPickup = how much ammo you get back from pickup
	protected int spreadCount = 0, maxSpread;				//Accuracy value (pixel range from centre)
	protected int meleeMax;
	public int currentMelee = 0;
	protected int gunDamage;
	public Transform shot_source;

	//IK stuff
	public bool useIK = false;
	public bool leftHandIK = true;
	public bool rightHandIK = true;
	public Transform LHandPos,RHandPos, RightHand;
	public GameObject LRWeapon;
	protected Animator anim;

	private bool an_Set = false;
	private bool weaponDrawn = false;
	private float an_dt = 0.0f;

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


	// Use this for initialization
	public void Start () {
		anim = gameObject.GetComponent<Animator> ();
		//anim.SetLayerWeight (0, 0.5f);
		sounds = gameObject.GetComponent<SoundPool> ();
	}
	
	// Update is called once per frame
	public void Update () {

		if (weaponFireRateTimer>0)
			weaponFireRateTimer -= Time.deltaTime;

		if (spreadRateTimer>0)
			spreadRateTimer -= Time.deltaTime;

		if (spreadCount > maxSpread)
			spreadCount = maxSpread;

		if (spreadRateTimer <= 0) {
			if (spreadCount>0)
			{
				//Debug.Log ("Counting down spread");
				spreadCount--;
				spreadRateTimer=spreadRate;
			}
		}


		if (currentPoise < 25.0 && !anim.GetCurrentAnimatorStateInfo (0).IsTag ("NoFreeMove")) {
			currentPoise = 75;
			freemove = true;
		} else if (currentPoise > 35.0 && alive)
			freemove = true;

		checkIK ();
	
	}

	public void FixedUpdate()
	{
		//Increase Poise over time. 1 second = 10 poise;
		currentPoise += Time.fixedDeltaTime*10;
		if (currentPoise > maxPoise)
			currentPoise = maxPoise;
	}

	public virtual void meleeAttack()
	{
		Debug.Log ("Hyaa!");
	}

	public virtual void endMeleeAttack()
	{
	}

	public virtual void shootWeapon() {
		//Get the gameObject that GunScript is attached to, then find the camera attached to that child.
		//Take the screen point that we want to use as the point we're going to shoot towards
		/*
		if (weaponFireRateTimer <= 0) {
		
			Ray camRay = cam.ScreenPointToRay (new Vector3 (Screen.width / 2 + Random.Range (-spreadCount, spreadCount),  Screen.height * 2 / 3 + Random.Range (-spreadCount, spreadCount), 0));
			Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
			Physics.Raycast (camRay, out hit, weaponRange);
		
			//Debug.Log ("Shooting at " + hit.transform.gameObject.name);
		
			Vector3 target = hit.point;
			Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
			Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);

			weaponFireRateTimer = weaponFireRate;
			spreadCount++;
			spreadRateTimer = spreadRate;

		} 
		else {}*/
	}



	public virtual void dash()
	{//generic dash code
	}



	public void receiveDamage(int dmg)
	{
		//Debug.Log ("ouch");
		health -= dmg;
		if (health <= 0) {
			alive = false;
			Destroy (this.gameObject, 2.0f);
			freemove = false;
		}
	}

	public void receivePoiseDamage(float poisedmg)
	{
		if(alive){
			currentPoise -= poisedmg;
			if(currentPoise <= 20)
			{
				freemove = false;
				Debug.Log("KD");
			}
			else if(currentPoise <= 30 && currentPoise > 20)
			{
				freemove = false;
				Debug.Log("Stunned");
			}
		}
	}

	public void receiveHealth(int up)
	{
		health += up;
		if (health >= maxHealth)
			health = maxHealth;
	}


	void OnAnimatorIK()
	{
		if (useIK)
		{
			if (leftHandIK)
			{
				anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
				anim.SetIKPosition(AvatarIKGoal.LeftHand, LHandPos.transform.position);
				
				
			}
			
			if (rightHandIK)
			{
				anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
				anim.SetIKPosition(AvatarIKGoal.RightHand, RHandPos.transform.position);
				
			}
			
		}
	}

	
	public void checkIK()
	{
		if (weaponHeld == true)
		{
			
			AnimatorStateInfo animPlayingState = anim.GetCurrentAnimatorStateInfo(1);
			if (animPlayingState.IsName("Aim"))
			{
				float playbackTime = animPlayingState.normalizedTime % 1;
				
				if ((an_dt > 0.01f) && (an_Set == false))
				{
					
					LRWeapon.SetActive (true);
					LRWeapon.transform.localPosition = Vector3.zero;
					LRWeapon.transform.localRotation = Quaternion.identity;
					LRWeapon.transform.Rotate(220, 0, -90);
					LRWeapon.transform.RotateAround (RHandPos.position, transform.right, 75);
					//print ("Gun withdrawn");
					
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
			if (weaponDrawn)
			{
				//anim.SetLayerWeight(1, 0f);
				LRWeapon.SetActive (false);
				useIK = false;
				an_Set = false;
				an_dt = 0f;
				weaponDrawn = false;
				//print ("Gun put away");
			}
			
		}
	}



}
