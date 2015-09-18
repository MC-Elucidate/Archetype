using UnityEngine;
using System.Collections;

public class NinjaScript: PlayerCharacter {

	public Transform cardPrefab;

	//Invis variables
	private int baseAggro = 170, invisAggro = 0;
	private float invisDuration = 15, currentInvis = 0;


	// Use this for initialization
	protected void Start () {
		base.Start ();
		health = 150;
		maxHealth = 150;
		runSpeed = 16;
		meleeMax = 7;
		characterRadius = 0.4f;
		aggro = baseAggro;

		//Character-specific weapon stats
		weaponRange = 200f;
		weaponFireRate = 0.2f;
		spreadRate = 0.21f;
		maxSpread = 12;
		weaponHeld = false;
		ammoCount = 60;
		maxAmmo = 60;
		ammoPickup = 20;

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
		if (currentInvis > 0)
			currentInvis -= Time.fixedDeltaTime;
		else
			aggro = baseAggro;
	}
	public override void meleeAttack()
	{
		base.meleeAttack ();
	}

	public override void shootWeapon()
	{
		if(ammoCount>0)
		{
			if (weaponFireRateTimer <= 0) {
				
				RaycastHit hit;
				Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f + Random.Range (-spreadCount*spreadFactor,spreadCount*spreadFactor),  0.666667f + Random.Range (-spreadCount*spreadFactor,spreadCount*spreadFactor), 0));
				Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
				Physics.Raycast (camRay, out hit, weaponRange);
				
				Debug.Log ("Ninja Shooting at " + hit.transform.gameObject.name);
				
				Vector3 target = hit.point;
				Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
				Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);
				
				Quaternion cardRotation = Quaternion.identity;
				cardRotation.SetLookRotation(target - shot_source.position, Vector3.up);
				
				Transform card;
				card = Instantiate (cardPrefab, shot_source.position, cardRotation) as Transform;
				card.gameObject.SendMessage("setDamage",(int)(gunDamage*damageMod));

				weaponFireRateTimer = weaponFireRate;
				spreadCount++;
				spreadRateTimer = spreadRate;
				ammoCount--;
				sounds.pew();
			}
		} 
		
		else {}
	}

	public override void special1()
	{
		if (currentSpecial1 <= 0) {
			currentSpecial1 = special1CD;
			Debug.Log ("Doing special1");
			sounds.playSpecial1Sound ();
		}
	}
	
	public override void special2()
	{
		if (currentSpecial2 <= 0) {
			currentSpecial2 = special2CD;
			aggro = invisAggro;
			currentInvis = invisDuration;
			sounds.playSpecial2Sound ();
		}
	}
	
	public override void super()
	{
		if (currentSuper <= 0) {
			currentSuper = superCD;
			Debug.Log ("Doing super");
			sounds.playSpecial3Sound ();
		}
	}

	
	public override void rotateCamera(float pitch)
	{
		base.rotateCamera (pitch);
	}

	public override void checkIK()
	{}
}
