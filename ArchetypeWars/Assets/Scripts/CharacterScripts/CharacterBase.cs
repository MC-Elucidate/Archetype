using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour {

	protected int health, maxHealth;

	//Weapon stuff
	protected float weaponRange = 100f, weaponFireRate, weaponFireRateTimer = 0f, spreadRate, spreadRateTimer = 0f, meleeFireRate, meleeFireRateTimer = 0f;
	protected int ammoCount, maxAmmo, ammoPickup; 			//ammoPickup = how much ammo you get back from pickup
	protected int spreadCount = 0, maxSpread;				//Accuracy value (pixel range from centre)
	protected int meleeMax, currentMelee = 0;
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


	//Stuff for movement controller
	public bool melee = false, alive = true, weaponHeld = true, damaged = false;
	public float runSpeed, characterRadius, floorcast = 0.05f;


	// Use this for initialization
	public void Start () {
		anim = gameObject.GetComponent<Animator> ();
		anim.SetLayerWeight (0, 0.5f);
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
				Debug.Log ("Counting down spread");
				spreadCount--;
				spreadRateTimer=spreadRate;
			}
		}

		checkIK ();
	
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
		Debug.Log ("ouch");
		health -= dmg;
		if (health <= 0) {
			alive = false;
			Destroy (this.gameObject, 3f);
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
				
				if ((an_dt > 0.44f) && (an_Set == false))
				{
					
					LRWeapon.SetActive (true);
					LRWeapon.transform.localPosition = Vector3.zero;
					LRWeapon.transform.localRotation = Quaternion.identity;
					LRWeapon.transform.Rotate(220, 0, -90);
					LRWeapon.transform.RotateAround (RHandPos.position, transform.right, 75);
					print ("Gun withdrawn");
					
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
				print ("Gun put away");
			}
			
		}
	}



}
