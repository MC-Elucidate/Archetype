using UnityEngine;
using System.Collections;

public class MovementController: MonoBehaviour {

	//Wallrun restrictions
	private float wallrunTime = 0.7f, currentWallrun = 0;

	private CharacterController charCon;
	private Animator anim;
	private PlayerCharacter character;

	//Tags set by base class to handle which player is being used
	protected string verticalTag, horizontalTag, mouseXTag, jumpTag, wallrunTag, slideTag, mouseYTag, fireTag, special1Tag, special2Tag, superTag, meleeTag;

	// Use this for initialization
	protected void Start () {
		charCon = gameObject.GetComponent<CharacterController>();
		anim = gameObject.GetComponent<Animator>();
		character = gameObject.GetComponent<PlayerCharacter> ();
	}



	// Update is called once per frame
	protected void Update () {
		//Use custom method for checking if we are on the ground
		if(character.velocity.y <= 0)
			checkGrounded ();


		//If we are not wallrunning or sliding or in a stun (we have free movement)
		if (!character.wallRunning && !character.sliding && character.freemove) {
			float vert = Input.GetAxis (verticalTag);
			float hor = Input.GetAxis (horizontalTag);
			float yaw = Input.GetAxis (mouseXTag) * character.rotSpeed;


			//Reset wallrun timer if we're not wallrunning
			currentWallrun = 0;

			//ROTATE CHARACTER LEFT/RIGHT
			transform.Rotate (0f, yaw, 0f);
			character.velocity = Quaternion.AngleAxis(-yaw, Vector3.up) * character.velocity;
			character.movementUpdate (vert, hor);
		
			/*
			 * JUMP INPUT MANAGEMENT
			 */
			if (Input.GetButtonDown (jumpTag))  //Jump button pressed
				character.jump ();
			else if (Input.GetButton (jumpTag))  //Holding jump to accelerate upwards (platformer style)
				character.jumpHold ();
			else if (Input.GetButtonUp (jumpTag)) //Release jump button, return to normal gravity
				character.jumpEnd ();
			/*
			 * END OF JUMP SECTION
			 */

			/*
			 * OTHER MOVEMENT ACTIONS INPUT MANAGEMENT (MELEE AND SLIDE AND SPECIALS)
			 */
			if (Input.GetButtonDown (slideTag)) //Slide button pressed
				character.slide ();
			else if (Input.GetButtonDown (meleeTag))  //Melee button pressed
				character.meleeAttack ();
			//else if (character.melee && !(anim.GetCurrentAnimatorStateInfo (1).IsTag ("MeleeAttack"))) //When the melee animation is finished
			//	character.meleeAttackEnd ();
			else if (Input.GetButtonDown (special1Tag)) //Performing special 1
				character.specialMove (1);
			else if (Input.GetButtonDown (special2Tag)) //Performing special 2
				character.specialMove (2);
			else if (Input.GetButtonDown (superTag) || Input.GetAxis (fireTag) < 0) //Performing super move
				character.specialMove (3);
			else if (Input.GetAxis (fireTag) > 0) //Axis > 0 = R2, Axis < 0 = L2 (when inverted)
				character.shootWeapon ();


		} 

		/*
		* WE DON"T HAVE FREEMOVEMENT (EITHER WALLRUNNING, SLIDING, OR STUN/KNOCKDOWN)
		*/
		else if (character.wallRunning) { //while wallrunning

			currentWallrun += Time.deltaTime;
			if (character.wallrunUp) {

				character.wallrunVertical ();

				if (Input.GetButtonUp (wallrunTag)) { //Release wallrun button. Ends wallrun. Begins freefall
					character.wallrunEnd ();
					currentWallrun = 0;
				} else if (currentWallrun >= wallrunTime) { //pass wallrun time limit (drop down)
					character.wallrunEnd ();
					currentWallrun = 0;
				}
			} else { //wallrunning diagonally

				character.wallrunDiagonal ();


				if (Input.GetButtonUp (wallrunTag)) { //Release wallrun button. Ends wallrun. Begins freefall
					character.wallrunEnd ();
					currentWallrun = 0;
				} else if (currentWallrun >= wallrunTime) {
					character.wallrunEnd ();
					currentWallrun = 0;
				}
			}
		} else if (character.sliding) { //if we're sliding
			character.slide ();
		} else if (!character.alive) { //if we're dead
			character.movementUpdate(0, 0);
		}
		 
		//Move character corresponding to the velocity vector
		charCon.Move (transform.rotation * character.velocity * Time.deltaTime);

		//Camera Up/Down Movement
		if (character.alive) {
			float pitch = Input.GetAxis (mouseYTag) * character.rotSpeed;
			character.rotateCamera (pitch);
		}
	}

	protected void FixedUpdate()
	{
		if (transform.position.y < -10) {
			transform.position = new Vector3 (transform.position.x, 15, transform.position.z);
			character.velocity.y = 0;
		}
	}

	/*
	* Method called when the character collides with an object.
	* Used to determine if we should wallrun, and which direction we should wallrun in.
	* Checks first if we should wallrun straight up, then right, then left.
	* Only wallruns if we encounter an object tagged as "Wall".
	*/
	protected void OnControllerColliderHit(ControllerColliderHit collision) {
		if (collision.gameObject.tag == "Wall" && Input.GetButton(wallrunTag) && !character.wallRunning && character.velocity.z > 0 && !character.sliding && !character.doubleJumping && character.freemove){
			RaycastHit target;
			if(Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.forward, out target, character.characterRadius))
			{
				if(target.transform.gameObject.tag == "Wall")
				{
					character.wallRunning = character.wallrunUp = true;
					character.doubleJumping = true;
					character.isGrounded = false;
					character.velocity.y = 1f;
					float angle = Vector3.Angle(transform.forward, target.normal);
					angle -= 180;
					transform.Rotate(0, -angle, 0);
					angle = Vector3.Angle(transform.forward, target.normal) - 180;
					if(angle != 180)
					{
						transform.Rotate(0, angle, 0);
					}
				}
			}
			else if (Physics.Raycast(transform.position  + new Vector3(0, 0.3f, 0), transform.right, out target, character.characterRadius*2))
			{
				if(target.transform.gameObject.tag == "Wall")
				{
					character.wallRunning = character.wallrunRight = true;
					character.doubleJumping = true;
					character.isGrounded = false;
					character.velocity.y = 10f;
					float angle = Vector3.Angle(transform.forward, target.normal);
					angle -= 90;
					transform.Rotate(0, -angle, 0);
				}
			}
			else if (Physics.Raycast(transform.position  + new Vector3(0, 0.3f, 0), -transform.right, out target, character.characterRadius*2))
			{
				character.wallRunning = character.wallrunLeft = true;
				character.doubleJumping = true;
				character.isGrounded = false;
				character.velocity.y = 10f;
				float angle = Vector3.Angle(transform.forward, target.normal);
				angle -= 90;
				transform.Rotate(0, angle, 0);
			}
		}
	}

	/*
	* Method used to determine if we are on the ground.
	* Shoots a ray downwards to determine if the character is at the ground.
	* Only called if the character's Y velocity is less than or equal to zero. This is to prevent the method from stopping a jump.
	*/
	public void checkGrounded()
	{
		RaycastHit hit;
		//Debug.DrawRay(transform.position, -transform.up * character.floorcast, Color.blue, 0.8f);
		if (Physics.Raycast (transform.position, -transform.up, out hit, character.floorcast)) { //If shit be fucked up with the jump, decrease floorcast and raise the character controller capsule a tad
			character.isGrounded = true;
			character.doubleJumping = false;
			character.wallrunEnd();
		}
		else
			character.isGrounded = false;
	}


}
