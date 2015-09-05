using UnityEngine;
using System.Collections;

public class NinjaScript: PlayerCharacter {

	public Transform cardPrefab;
	// Use this for initialization
	protected void Start () {
		base.Start ();
		health = 150;
		maxHealth = 150;
		runSpeed = 16;
		meleeMax = 7;
		characterRadius = 0.4f;

		//Character-specific weapon stats
		weaponRange = 100f;
		weaponFireRate = 0.2f;
		spreadRate = 0.21f;
		maxSpread = 12;
		weaponHeld = false;
		ammoCount = 40;
		maxAmmo = 40;

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
				
				CardScript card = Instantiate (cardPrefab, shot_source.position, cardRotation) as CardScript;
				card.damage = (int)(card.damage*damageMod);
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
			Debug.Log ("Doing special2");
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
