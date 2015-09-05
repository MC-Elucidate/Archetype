using UnityEngine;
using System.Collections;

public class PlayerCharacter : CharacterBase {


	//Player Camera
	public Camera cam;

	//Player specific shooting logic
	protected RaycastHit hit;
	protected float spreadFactor = 0.003f;


	//Player movement variables
	public Vector3 velocity;
	public float globalGravity = -10f, currentGravity = -10f;
	private float maxForwardSpeed = 10f, maxBackSpeed = -5f, maxSideSpeed = 7f, groundAcc = 20f, airAcc = 10f, airFriction = 20f;

	//State booleans
	public bool doubleJumping = false, wallRunning  = false, wallrunUp = false, wallrunLeft = false, wallrunRight = false, sliding = false, isGrounded = false;

	//Jump variables
	protected float jumpPower = 3f, jumpGravity = 3f, jumpTime = 0.25f, currentJump = 0f;

	//Slide variables
	protected float slideSpeed = 15f, slideTime = 0.75f, currentSlide = 0f;

	//Wallrun variables
	protected float wallrunTime = 1f, currentWallrun = 0, verticalWallVelocity = 8f, diagonalWallVelocity = 15f;

	//Special ability variables
	protected float special1CD, special2CD, superCD, currentSpecial1 = 0, currentSpecial2 = 0, currentSuper = 0;

	//Buffs
	public float armourMod = 1.0f, damageMod = 1.0f;

	// Use this for initialization
	public void Start () {
		base.Start ();
		SRWeapon.SetActive (false); //Hide melee weapon and turnoff collision
	}
	
	// Update is called once per frame
	public void Update () {
		base.Update ();
		checkStun ();
	}
	public void FixedUpdate(){
		base.FixedUpdate ();
		if (currentSpecial1 > 0)
			currentSpecial1 -= Time.fixedDeltaTime;
		if (currentSpecial2 > 0)
			currentSpecial2 -= Time.fixedDeltaTime;
		if (currentSuper > 0)
			currentSuper -= Time.fixedDeltaTime;
	}

	/*
	 * Used to get the current health of the player character.
	 * Used for the HUD
	 */
	public int getHealth ()
	{return health;}

	/*
	 * Used to get the current ammo of the player character.
	 * Used for the HUD
	 */
	public int getAmmo()
	{return ammoCount;}

	/*
	 * Used to prevent the character from moving if they are stunned/knocked down.
	 * Sets velocity's x and z compenents to 0.
	 */
	private void checkStun()
	{
		if (!freemove) {
			velocity.x = 0;
			velocity.z = 0;
		}
	}

	/*
	 * Gives the character more resistence to damage, or a damage buff for their firearm.
	 * Uses parameter "buff" to determine which stat to buff
	 */
	private void giveBuff(char buff)
	{
		switch (buff) {
		case 'd': damageMod *= 1.5f;
			break;
		case 'a': armourMod *= 2f;
			break;
		}
	}

	/*
	 * Reduces the character's armour or damage.
	 * Uses parameter "buff" to determine which stat to debuff
	 */
	private void giveDebuff(char buff)
	{
		switch (buff) {
		case 'd': damageMod /= 1.5f;
			break;
		case 'a': armourMod /= 2f;
			break;
		}
	}

	/*
	 * Rotates the player's camera based on Y-axis input.
	 * Locks the camera to 40 degrees up and down.
	 * */
	public virtual void rotateCamera(float pitch)
	{
		if (pitch > 0) { // if we look up
			if(		(cam.transform.localEulerAngles.x > 320) 	|| 	(cam.transform.localEulerAngles.x < 90)		)
			{
				cam.transform.RotateAround (transform.position, transform.right, -pitch);
				LRWeapon.transform.RotateAround (RHandPos.position, transform.right, -pitch);
			}
		} else if (pitch < 0) { //if we look down
			if(		(cam.transform.localEulerAngles.x > 270) 	|| 	(cam.transform.localEulerAngles.x < 40)		)
			{
				cam.transform.RotateAround (transform.position, transform.right, -pitch);
				LRWeapon.transform.RotateAround (RHandPos.position, transform.right, -pitch);
			}
		}
	}

	/*
	 * Used to take damage;
	 * Decreases health by the amount received in dmg.
	 * If health reaches 0, sets the character to a "knocked out" state.
	 */
	public override void receiveDamage(int dmg)
	{
		//Debug.Log ("ouch");
		health -= (int)(dmg/armourMod);
		sounds.playHitSound();
		if (health <= 0) {
			alive = false;
			freemove = false;
			cam.transform.parent = null;
			currentGravity = globalGravity;
			velocity.x = 0;
			velocity.y = 0;
			velocity.z = 0;
			sounds.playDeathSound ();
		}
	}



	/*
	 * Function called when player presses the Fire button.
	 * Simulates weapon recoil and accuracy, and fire rate of weapons.
	 * */
	public virtual void shootWeapon() {
		//Get the gameObject that GunScript is attached to, then find the camera attached to that child.
		//Take the screen point that we want to use as the point we're going to shoot towards

		if (ammoCount > 0) {
			if (weaponFireRateTimer <= 0) {
				
				Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f + Random.Range (-spreadCount * spreadFactor, spreadCount * spreadFactor), 0.666667f + Random.Range (-spreadCount * spreadFactor, spreadCount * spreadFactor), 0));
				Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
				Physics.Raycast (camRay, out hit, weaponRange);
				
				//Debug.Log ("Shooting at " + hit.transform.gameObject.name);
				
				Vector3 target = hit.point;
				Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
				Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);
				if (hit.transform.gameObject.tag == "Enemy") {
					hit.transform.gameObject.SendMessage ("receiveDamage", (int)(gunDamage*damageMod), SendMessageOptions.DontRequireReceiver);
					hit.transform.gameObject.SendMessage ("receivePoiseDamage", (int)(poiseDamage*damageMod), SendMessageOptions.DontRequireReceiver);
				}
				weaponFireRateTimer = weaponFireRate;
				spreadCount++;
				spreadRateTimer = spreadRate;
				ammoCount--;
				sounds.pew ();
			} 
		}
	}


	/*
	 * Function called when player presses the Melee button.
	 * Allows the player character to perform a melee attack, hiding the firearm and showing the melee weapon.
	 * Allows the player to transition into a new melee move from the previous one.
	 * */
	public override void meleeAttack()
	{
		if (isGrounded) { //Can only melee on the ground
			if (currentMelee == 0) { //First melee attack in the combo
				currentMelee++;
				melee = true;
				weaponHeld = false;
				SRWeapon.SetActive (true);
				sounds.meleeSound();
			} else if ((currentMelee < meleeMax) && (anim.GetCurrentAnimatorStateInfo (1).IsName ("Attack" + currentMelee))) { //Post-first melee attacks, can only transition into state n+1 if we're in state n
				currentMelee++;
				sounds.meleeSound ();
			}
		}

	}


	/*
	 * Function called when the character is no longer in a melee attck animation.
	 * Hides the melee weapon and restores the firearm.
	 * */
	public override void meleeAttackEnd()
	{
		melee = false;
		currentMelee = 0;
		SRWeapon.SetActive (false);
		weaponHeld = true;
	}

	/*
	 * Function called when the player presses the special 1 button.
	 * To be overridden in character specific classes
	 * */
	public virtual void special1()
	{}

	/*
	 * Function called when the player presses the special 2 button.
	 * To be overridden in character specific classes
	 * */
	public virtual void special2()
	{}

	/*
	 * Function called when the player presses the super button.
	 * To be overridden in character specific classes
	 * */
	public virtual void super()
	{}

	/*
	 * Function called when the player presses the slide button.
	 * Sets forward velocity high for a set time.
	 * */
	public virtual void slide()
	{

		if (!sliding && isGrounded)
			sliding = true;

		velocity.x = 0;
		velocity.y = 0;
		velocity.z = slideSpeed;


		currentSlide += Time.deltaTime;
		if (currentSlide >= slideTime) {
			sliding = false;
			currentSlide = 0;
		}
	}

	/*
	 * Function called when the player presses the jump button.
	 * Handles jumping and double jumping
	 * */
	public virtual void jump()
	{
		if (isGrounded) {
			velocity.y = jumpPower;
			currentJump = 0f;
			currentGravity = jumpGravity;
			isGrounded = false;
			sounds.playJumpSound();
		} 
		else if (!doubleJumping) {
			velocity.y = jumpPower;
			currentJump = 0f;
			currentGravity = jumpGravity;
			doubleJumping = true;
			sounds.playJumpSound();
		}
	}

	/*
	 * Function called when the player holds the jump button while jumping.
	 * Handles accelrated vertical movement. (Also used for sniper's parasol glide)
	 * */
	public virtual void jumpHold()
	{
		if (currentJump >= jumpTime) {
			currentGravity = globalGravity;
		} 
		else
			currentJump += Time.deltaTime;
	 
	}

	public virtual void jumpEnd()
	{
		currentGravity = globalGravity;
	}

	/*
	 * Function called when the player is diagonally wallrunning.
	 * Used to determine vertical velocity
	 * */
	public virtual void wallrunDiagonal()
	{
		currentGravity = globalGravity;
		velocity.y += currentGravity * Time.deltaTime;
		velocity.z = diagonalWallVelocity;
		if (wallrunRight) { //Check if wall ends.
			if (!Physics.Raycast (transform.position, transform.right, characterRadius)) {
				wallrunEnd();
			}
		} else { //Check if left wall ends
			if (!Physics.Raycast (transform.position, -transform.right, characterRadius)) {
				wallrunEnd();
			}
		}
	}

	/*
	 * Function called when the player is vertically wallrunning.
	 * Used to determine vertical velocity
	 * */
	public virtual void wallrunVertical()
	{
		velocity.x = 0f;
		velocity.y = verticalWallVelocity;
		velocity.z = 0f;

		if (!Physics.Raycast (transform.position, transform.forward, characterRadius)) { //reach top of wall
			wallRunning = wallrunUp = false;
			velocity.y = jumpPower*2;
			//currentWallrun = 0;
		}
	}

	/*
	 * Function called when a wallrun ends.
	 * Sets all wallrun booleans to false.
	 * */
	public virtual void wallrunEnd()
	{
		wallRunning = wallrunLeft = wallrunRight = wallrunUp = false;
		//currentWallrun = 0;
	}












	/*
	 * Function called when the player is moving.
	 * Calculates velocity based on input and gravity.
	 * Adjusts velocity to give a sense of momentum.
	 * */
	public virtual void movementUpdate(float vert, float hor)
	{
		float dx = 0, dy = 0, dz = 0;

		if (vert != 0)
			vert = vert / Mathf.Abs (vert);
		if (hor != 0)
			hor = hor / Mathf.Abs (hor);

		//We have control over the character.
		if (freemove) {

			//Set Z Component
			if (velocity.z >= 0) {
				if (velocity.z > maxForwardSpeed) {
					dz -= airFriction * Time.deltaTime;
					velocity.z += dz;
				} else {
					if (isGrounded) {
						if (vert == 0 && velocity.z != 0)
						{
							dz -= airFriction * Time.deltaTime;
							if (velocity.z + dz <= 0)
							{dz = 0; velocity.z = 0;}
						}
						else
							dz = (vert * groundAcc) * Time.deltaTime;

					} else {
						if (vert == 0)
							dz -= airFriction * Time.deltaTime;
						else
							dz = (vert * airAcc) * Time.deltaTime;
					}
					
	
					velocity.z += dz;
					if (velocity.z > maxForwardSpeed)
						velocity.z = maxForwardSpeed;
				}

			} else if (velocity.z < 0) {
				if (velocity.z < maxBackSpeed) {
					dz += airFriction * Time.deltaTime;
					velocity.z += dz;
				} else {
					if (isGrounded) {
						if (vert == 0 && velocity.z != 0)
						{
							dz += airFriction * Time.deltaTime;
							if (velocity.z + dz >= 0)
							{dz = 0; velocity.z = 0;}
						}
						else
							dz = (vert * groundAcc) * Time.deltaTime;
					} else {
						if (vert == 0)
							dz += airFriction * Time.deltaTime;
						else
							dz = (vert * airAcc) * Time.deltaTime;
					}

					velocity.z += dz;
					if (velocity.z < maxBackSpeed)
						velocity.z = maxBackSpeed;
				}
			}

			//End Z Change

			//Set X Component
			if (velocity.x >= 0) {
				if (velocity.x > maxSideSpeed) {
					dx -= airFriction * Time.deltaTime;
					velocity.x += dx;
				} else {
					if (isGrounded) {
						if (hor == 0 && velocity.x != 0)
						{
							dx -= airFriction * Time.deltaTime;
							if (velocity.x + dx <= 0)
							{dx = 0; velocity.x = 0;}
						}
						else
							dx = (hor * groundAcc) * Time.deltaTime;
					} else {
						if (hor == 0)
							dx -= airFriction * Time.deltaTime;
						else
							dz = (hor * airAcc) * Time.deltaTime;
					}
					
					
					velocity.x += dx;
					if (velocity.x > maxSideSpeed)
						velocity.x = maxSideSpeed;
				}
				
			} else if (velocity.x < 0) {
				if (velocity.x < -maxSideSpeed) {
					dx += airFriction * Time.deltaTime;
					velocity.x += dx;
				} else {
					if (isGrounded) {
						if (hor == 0 && velocity.x != 0)
						{
							dx += airFriction * Time.deltaTime;
							if (velocity.x + dx >= 0)
							{dx = 0; velocity.x = 0;}
						}
						else
							dx = (hor * groundAcc) * Time.deltaTime;
					} else {
						if (hor == 0)
							dx += airFriction * Time.deltaTime;
						else
							dx = (hor * airAcc) * Time.deltaTime;
					}
					
					velocity.x += dx;
					if (velocity.x < -maxSideSpeed)
						velocity.x = -maxSideSpeed;
				}
			}

			//End X Change

			//Set Y Component
			if (isGrounded) {
				velocity.y = 0;
				doubleJumping = false;
				currentJump = 0;
			} else {
				velocity.y += currentGravity * Time.deltaTime;
			}

			//End Y Change
		} else {
			//Set Y Component
			if (isGrounded) {
				velocity.y = 0;
				doubleJumping = false;
				currentJump = 0;
			} else {
				velocity.y += currentGravity * Time.deltaTime;
			}
		}
	}

	public void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Forcefield")
			giveBuff ('a');
	}

	public void OnTriggerExit(Collider coll)
	{
		if (coll.tag == "Forcefield")
			giveDebuff ('a');
	}
}
