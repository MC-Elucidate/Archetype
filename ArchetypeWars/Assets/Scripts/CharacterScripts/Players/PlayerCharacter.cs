using UnityEngine;
using System.Collections;

public class PlayerCharacter : CharacterBase {


	//Player Camera
	public Camera cam;
	public float defaultRotationSpeed = 7f;
	public float rotSpeed = 7f;

	//Player specific shooting logic
	protected RaycastHit hit;
	protected float spreadFactor = 0.003f;

	//Bullet Trail
	public GameObject bulletTrail;


	//Player movement variables
	public Vector3 velocity;
	public float globalGravity = -16f, currentGravity = -16f;
	private float maxForwardSpeed = 8f, maxBackSpeed = -6f, maxSideSpeed = 7f, groundAcc = 60f, airAcc = 40f, airFriction = 40f;

	//State booleans
	public bool doubleJumping = false, wallRunning  = false, wallrunUp = false, wallrunLeft = false, wallrunRight = false, sliding = false, isGrounded = false;

	//Jump variables
	protected float jumpPower = 3f, jumpGravity = 3f, jumpTime = 0.25f, currentJump = 0f;

	//Slide variables
	protected float slideSpeed = 12f, slideTime = 0.75f, currentSlide = 0f;

	//Wallrun variables
	protected float wallrunTime = 1f, currentWallrun = 0, verticalWallVelocity = 8f, diagonalWallVelocity = 15f;

	//Special ability variables
	protected float special1CD, special2CD, superCD, currentSpecial1 = 0, currentSpecial2 = 0, currentSuper = 0;

	//Buffs
	public float armourMod = 1.0f, damageMod = 1.0f;
	private bool shieldBuff = false, commanderBuff = false;

	// Use this for initialization
	public void Start () {
		base.Start ();
		SRWeapon.SetActive (false); //Hide melee weapon and turnoff collision
		rotSpeed = defaultRotationSpeed;
	}
	
	// Update is called once per frame
	public void Update () {
		base.Update ();
		checkStun ();
		addBuffs ();
		anim.SetBool ("Sliding", sliding);
		anim.SetBool ("WeaponHeld", weaponHeld);
		anim.SetBool ("Wallrunning", wallRunning);
		anim.SetBool ("Melee", melee);
		anim.SetBool ("Alive", alive);
		anim.SetInteger ("MeleeCount", currentMelee);
		anim.SetFloat ("Vertical", (velocity.z/maxForwardSpeed));
		anim.SetFloat ("Horizontal", (velocity.x/maxSideSpeed));
		anim.SetFloat ("UpDown", velocity.y);
		anim.SetFloat ("Poise", currentPoise);
	}

	public void FixedUpdate(){
		base.FixedUpdate ();

		//Updates cooldowns every fixed time step
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
	 * Used to get the cooldown for special ability 1.
	 * Used for the HUD
	 */
	public float getCooldownOne()
	{return currentSpecial1;}

	/*
	 * Used to get the cooldown for special ability 2.
	 * Used for the HUD
	 */
	public float getCooldownTwo()
	{return currentSpecial2;}

	/*
	 * Used to get the cooldown for special ability 3 (super).
	 * Used for the HUD
	 */
	public float getCooldownThree()
	{return currentSuper;}

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
	 * Checks cooldown of special move.
	 * Plays special animation if cooldown is 0.
	 * 
	 * */
	public void specialMove(int move)
	{
		switch (move) {
		case 1: if(currentSpecial1 <= 0) special1(); break;
		case 2: if(currentSpecial2 <= 0) special2(); break;
		case 3: if(currentSuper <= 0){ weaponHeld = false; super(); anim.SetTrigger("SuperTrigger"); }break;
		}
	}


	/*
	 * Determines which buffs are active and increases the modifier appropriately.
	 */
	private void addBuffs()
	{
		damageMod = 1;
		armourMod = 1;

		if (shieldBuff)
			armourMod += 1;

		if (commanderBuff) {
			armourMod += 1;
			damageMod += 1;
		}

	}

	/*
	 * Changes buff booleans to active or inactive.
	 * Called by other game objects.
	 * Uses "source" to determine which buff is being applied/removed.
	 * 'S' = Heavy's Shield
	 */
	public void changeBuffs(string source)
	{

		switch (source) {
		case "SOff": shieldBuff = false;
			break;
		case "COn": commanderBuff = true;
			break;
		case "COff": commanderBuff = false;
			break;

		}
	}

	/*
	 * Rotates the player's camera based on Y-axis input.
	 * Locks the camera to 40 degrees up and down.
	 * Rotates the character's weapon and hands up and down as well.
	 * Allows some error margin so the camera would not get stuck if it were to exceed the boundaries.
	 * Necessary because of high mouse sensitivity.
	 * */
	public virtual void rotateCamera(float pitch)
	{
		Vector3 cameraPivotPoint = transform.position;
		cameraPivotPoint.y += 0.8f;
		if (pitch > 0) { // if we look up
			if(		(cam.transform.localEulerAngles.x > 295) 	|| 	(cam.transform.localEulerAngles.x < 90)		)
			{
				cam.transform.RotateAround (cameraPivotPoint, transform.right, -pitch);
				LRWeapon.transform.RotateAround (RHandPos.position, transform.right, -pitch);
			}
		} else if (pitch < 0) { //if we look down
			if(		(cam.transform.localEulerAngles.x > 270) 	|| 	(cam.transform.localEulerAngles.x < 65)		)
			{
				cam.transform.RotateAround (cameraPivotPoint, transform.right, -pitch);
				LRWeapon.transform.RotateAround (RHandPos.position, transform.right, -pitch);
			}
		}
	}

	/*
	 * Used to take damage;
	 * Decreases health by the amount received in dmg. Applies armour modifier as well.
	 * If health reaches 0, sets the character to a "knocked out" state.
	 */
	public override void receiveDamage(int dmg)
	{
		if(health>0)
			sounds.playHitSound();

		health -= (int)(dmg/armourMod);

		if (health <= 0) {
			health = 0;
			aggro = 0;
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
	 * Increases the player character's current ammo.
	 * Caps it off at maxAmmo.
	 */
	private void receiveAmmo(int ammo)
	{
		ammoCount += ammo;
		if (ammoCount > maxAmmo)
			ammoCount = maxAmmo;
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
				
				Ray camRay = cam.ViewportPointToRay (new Vector3 (0.5f + Random.Range (-spreadCount * spreadFactor, spreadCount * spreadFactor), 0.5f + Random.Range (-spreadCount * spreadFactor, spreadCount * spreadFactor), 0));
				Debug.DrawRay (camRay.origin, camRay.direction * 10f, Color.yellow, 0.1f);
				Physics.Raycast (camRay, out hit, weaponRange);

				Vector3 target = hit.point;
				Physics.Raycast (shot_source.position, target - shot_source.position, out hit, weaponRange);
				Debug.DrawRay (shot_source.position, target - shot_source.position, Color.green, 0.1f);
				//Debug.Log ("Shooting: " + hit.transform.gameObject.name);

				//Deal damage if we hit an enemy's body or head
				if (hit.transform.gameObject.tag == "Enemy" || hit.transform.gameObject.tag == "EnemyHead") {
					Debug.Log ("Shooting: " + hit.transform.gameObject.tag);
					hit.transform.gameObject.SendMessage ("receiveDamage", (int)(gunDamage*damageMod), SendMessageOptions.DontRequireReceiver);
					hit.transform.gameObject.SendMessage ("receivePoiseDamage", (int)(poiseDamage*damageMod), SendMessageOptions.DontRequireReceiver);
				}


				//Create bullet trail
				Quaternion trailRotation = Quaternion.identity;
				trailRotation.SetLookRotation (hit.point - shot_source.position, Vector3.up);
				Transform trail;
				trail = Instantiate (bulletTrail, shot_source.position, trailRotation) as Transform;


				weaponFireRateTimer = weaponFireRate;
				spreadCount++;
				spreadRateTimer = spreadRate;
				ammoCount--;
				sounds.pew ();
				Transform fireParticle = Instantiate (weaponFlashEffect, shot_source.transform.position, Quaternion.identity) as Transform;
				fireParticle.parent = this.transform;
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

		if (sliding) {
			velocity.x = 0;
			velocity.y = 0;
			velocity.z = slideSpeed;

			currentSlide += Time.deltaTime;

			if (currentSlide >= slideTime) {
				sliding = false;
				currentSlide = 0;
			}
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
	 * Handles accelrated vertical movement.
	 * */
	public virtual void jumpHold()
	{
		if (currentJump >= jumpTime) {
			currentGravity = globalGravity;
		} 
		else
			currentJump += Time.deltaTime;
	 
	}

	/*
	 * Function called when the player releases the jump button.
	 * Sets gravity to default value.
	 * */
	public virtual void jumpEnd()
	{
		currentGravity = globalGravity;
	}

	/*
	 * Function called when the player is diagonally wallrunning.
	 * Used to determine vertical and horizontal velocity.
	 * */
	public virtual void wallrunDiagonal()
	{
		currentGravity = globalGravity;
		velocity.y += currentGravity * Time.deltaTime;
		velocity.z = diagonalWallVelocity;
		if (wallrunRight) { //Check if right wall ends.
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
	 * Used to determine vertical velocity.
	 * */
	public virtual void wallrunVertical()
	{
		velocity.x = 0f;
		velocity.y = verticalWallVelocity;
		velocity.z = 0f;
			
		currentGravity = globalGravity;


		//If the top of the wall is reached while wall running, give a little boost.
		if (!Physics.Raycast (transform.position, transform.forward, characterRadius)) {
			wallRunning = wallrunUp = false;
			velocity.y = jumpPower*2;
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
	 * Based on the rigidbody component in unity, but not nearly as complex and modified to give controlled characters a better feel.
	 * Allows the user to walk by slightly tilting an analogue stick, or accelerate to max velocity by holding the analogue stick fully.
	 * Uses friction to slow the character down when nothing is pressed.
	 * Gives a slower acceleration in the air than on the ground.
	 * Allows for different forward, back, and sideways speeds.
	 * Gets vertical input vert and horizontal input hor to determine the velocity.
	 * */
	public virtual void movementUpdate(float vert, float hor)
	{
		float dx = 0, dy = 0, dz = 0;
		
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
						if (vert == 0 && velocity.z != 0)
						{
							dz -= airFriction * Time.deltaTime;
							if (velocity.z + dz <= 0)
							{dz = 0; velocity.z = 0;}
						}
						else
							dz = (vert * airAcc) * Time.deltaTime;
					}
					
	
					velocity.z += dz;
					if (velocity.z > maxForwardSpeed*vert)
						velocity.z = maxForwardSpeed*vert;
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
						if (vert == 0 && velocity.z != 0)
						{
							dz += airFriction * Time.deltaTime;
							if (velocity.z + dz >= 0)
							{dz = 0; velocity.z = 0;}
						}
						else
							dz = (vert * airAcc) * Time.deltaTime;
					}

					velocity.z += dz;
					if (velocity.z < maxBackSpeed*-vert)
						velocity.z = maxBackSpeed*-vert;
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
						if (hor == 0 && velocity.x != 0)
						{
							dx -= airFriction * Time.deltaTime;
							if (velocity.x + dx <= 0)
							{dx = 0; velocity.x = 0;}
						}
						else
							dx = (hor * airAcc) * Time.deltaTime;
					}
					
					
					velocity.x += dx;
					if (velocity.x > maxSideSpeed*hor)
						velocity.x = maxSideSpeed*hor;
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
						if (hor == 0 && velocity.x != 0)
						{
							dx += airFriction * Time.deltaTime;
							if (velocity.x + dx >= 0)
							{dx = 0; velocity.x = 0;}
						}
						else
							dx = (hor * airAcc) * Time.deltaTime;
					}
					
					velocity.x += dx;
					if (velocity.x < -maxSideSpeed*-hor)
						velocity.x = -maxSideSpeed*-hor;
				}
			}

			//End X Change

			//Set Y Component (Gravity)
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


	/*
	 * Used when interacting with various triggers in the world.
	 * Detects Heavy's forcefield, health pickups, and ammo pickups.
	 * */
	public void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Forcefield")
			shieldBuff = true;
		else if (coll.tag == "HealthPickup") {
			receiveHealth(50);
			coll.gameObject.SendMessage("use");
		}
		else if (coll.tag == "AmmoPickup") {
			receiveAmmo(ammoPickup);
			coll.gameObject.SendMessage("use");
		}
	}

	/*
	 * Called when a trigger is left.
	 * Used to detect when we leave the forcefield.
	 * */
	public void OnTriggerExit(Collider coll)
	{
		if (coll.tag == "Forcefield")
			shieldBuff = false;
	}
}
