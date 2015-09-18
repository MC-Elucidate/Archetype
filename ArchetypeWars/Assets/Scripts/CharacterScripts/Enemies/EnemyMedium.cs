using UnityEngine;
using System.Collections;

public class EnemyMedium : EnemyCharacter {

	// Use this for initialization
	void Start () {
		base.Start();
		maxHealth = 160;
		health = 160;

		weaponRange = 500f;
		weaponFireRate = 0.2f;
		spreadRate = 0.21f;
		maxSpread = 12;
		gunDamage = 3;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
	void FixedUpdate(){
		base.FixedUpdate ();
	}

	void OnDestroy(){
		base.OnDestroy();
	}

	public override void checkIK()
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
					LRWeapon.transform.localEulerAngles = new Vector3(350, 70,137);
					//LRWeapon.transform.Rotate(220, 0, 180);
					//LRWeapon.transform.RotateAround (RHandPos.position, Vector3.up, 90); 
					//LRWeapon.transform.Rotate(0, 20, 0);
					//LRWeapon.transform.RotateAround (RHandPos.position, stransform.right, 90); 
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
