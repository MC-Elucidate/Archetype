using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {


	private bool doubleJumping = false, wallRunning  = false, wallrunUp = false, wallrunLeft = false, wallrunRight = false, sliding = false, isGrounded = false;


	public float rotSpeed = 7f, slidespeed = 1.5f, speed = 10f, jumpPower = 3f, jumpGravity = 3f;



	private float verticalVel = 0f;
	private float slideTime = 1.1f, currentSlide = 0;
	private float gravity = -9.8f;
	private float jumpTime = .25f, currentJump = 0;
	private float zMove = 0, xMove = 0;
	private float wallrunTime = 1.5f, currentWallrun = 0, wallrunCooldown = 1.5f, currentWallrunCooldown = 0;
	private float characterRadius;

	private CharacterController charCon;
	private Animator anim;
	private CharacterBase character;

	protected string verticalTag, horizontalTag, mouseXTag, jumpTag, wallrunTag, slideTag, mouseYTag, fireTag;

	// Use this for initialization
	protected void Start () {
		charCon = gameObject.GetComponent<CharacterController>();
		anim = gameObject.GetComponent<Animator>();
		character = gameObject.GetComponent<CharacterBase> ();
		characterRadius = character.characterRadius;
	}



	// Update is called once per frame
	protected void Update () {
		//Use custom method for checking if we are on the ground
		checkGrounded ();


		//If we are not wallrunning or sliding (we have free movement)
		if (!wallRunning && !sliding) {
			float vert = Input.GetAxis (verticalTag);
			float hor = Input.GetAxis (horizontalTag);
			float yaw = Input.GetAxis (mouseXTag) * rotSpeed;

			currentWallrunCooldown+=Time.deltaTime;
			wallRunning = wallrunLeft = wallrunRight = wallrunUp = false;

			transform.Rotate (0f, yaw, 0f);


			//If we are in the air
			if (!isGrounded) 
			{
				verticalVel += (gravity  * Time.deltaTime); //gravity doing its thing
				if(Input.GetButtonDown(jumpTag) && !doubleJumping) //Double jump if we haven't already done so
				{
					verticalVel = jumpPower;
					currentJump = 0f;
					gravity = jumpGravity;
					doubleJumping = true;
				}

				if(Input.GetButton(jumpTag)) //Holding jump to accelerate upwards (platformer style)
				{
					if(currentJump >= jumpTime)
					{
						gravity = -9.8f;
					}
					else
						currentJump += Time.deltaTime;
				}

				else
				{
					gravity = -9.8f;
				}

			}


			//When we're on the ground and trying to jump
			else if(Input.GetButtonDown(jumpTag))
			{
				verticalVel = jumpPower;
				currentJump = 0f;
				gravity = jumpGravity;
			}

			//on ground, not trying to jump
			else
			{
				verticalVel = 0;
				doubleJumping = false; currentJump = 0;

				if(Input.GetButtonDown(slideTag)) //can only slide when on ground
					sliding = true;
			}
	
			if(vert >= 0) //running forwards
			{
				xMove = hor;
				zMove = vert;
			}
			else //backpedaling
			{
				zMove = 0.5f*vert;
				xMove = 0.5f*hor;
			}

		}


		//We don't have free movement (wallrunning or sliding)
		else if (wallRunning) { //while wallrunning

			currentWallrun += Time.deltaTime;
			if(wallrunUp)
			{
				xMove = 0;
				zMove = 0f;
				verticalVel = 4f;

				if(Input.GetButtonUp(wallrunTag)) //Release wallrun button. Ends wallrun. Begins freefall
				{
					wallRunning = wallrunUp = false;
					currentWallrun = 0;
					currentWallrunCooldown = 0;
				}

				else if(currentWallrun >= wallrunTime) //pass wallrun time limit (drop down)
				{
					wallRunning = wallrunUp = false;
					currentWallrun = 0;
					currentWallrunCooldown = 0;
				}

				if(!Physics.Raycast(transform.position, transform.forward, characterRadius)) //reach top of wall
				{
					wallRunning = wallrunUp = false;
					verticalVel += jumpPower;
					currentWallrun = 0;
					currentWallrunCooldown = 0;
				}
			}
			else //wallrunning diagonally
			{

				verticalVel = 3f;
				xMove = 0;
				zMove = 1f;

				if(currentWallrun >= wallrunTime)
				{
					verticalVel = -3f;
				}

				if(Input.GetButtonDown(jumpTag) && !doubleJumping) //Jump off wall. Allows player to leap in direction being held. Uses double jump
				{
					zMove = Input.GetAxis("Vertical") * jumpPower * speed;
					xMove = Input.GetAxis("Horizontal") * jumpPower * speed;
					verticalVel = jumpPower;
					wallRunning = wallrunLeft = wallrunRight = false;
					currentWallrun = 0f;
					doubleJumping = true;
					currentWallrunCooldown = 0;
				}

				if(Input.GetButtonUp(wallrunTag)) //Release wallrun button. Ends wallrun. Begins freefall
				{
					wallRunning = wallrunLeft = wallrunRight = false;
					currentWallrun = 0f;
					currentWallrunCooldown = 0;
				}

				if(wallrunRight) //Check if wall ends.
				{
					if(!Physics.Raycast(transform.position, transform.right, characterRadius))
					{
						wallrunRight = wallRunning = false;
						currentWallrun = 0;
						currentWallrunCooldown = 0;
					}
				}

				else //Check if left wall ends
				{
					if(!Physics.Raycast(transform.position, -transform.right, characterRadius))
					{
						wallrunLeft = wallRunning = false;
						currentWallrun = 0;
						currentWallrunCooldown = 0;
					}
				}
			}
		} else if (sliding) {
			verticalVel = 0;
			xMove = 0;
			zMove = slidespeed;
			currentSlide+=Time.deltaTime;
			if(currentSlide >= slideTime)
			{
				currentSlide = 0;
				sliding = false;
			}
		}


		//Move character
		xMove = xMove * speed;
		zMove = zMove * speed;
		charCon.Move (transform.rotation * new Vector3 (xMove, verticalVel, zMove) * Time.deltaTime);





		//Camera Up/Down Movement
		float pitch = Input.GetAxis (mouseYTag) * rotSpeed;
		
		if (pitch > 0) { // if we look up
			if(		(character.cam.transform.localEulerAngles.x > 320) 	|| 	(character.cam.transform.localEulerAngles.x < 90)		)
				character.cam.transform.RotateAround (transform.position, transform.right, -pitch);
		} else if (pitch < 0) { //if we look down
			if(		(character.cam.transform.localEulerAngles.x > 270) 	|| 	(character.cam.transform.localEulerAngles.x < 40)		)
				character.cam.transform.RotateAround (transform.position, transform.right, -pitch);
		}




		//FIRE!
		if (Input.GetAxis(fireTag) > 0) { //Axis > 0 = R2, Axis < 0 = L2 (when inverted)
			character.shootWeapon ();
		}
	}

	protected void FixedUpdate()
	{
		//checkGrounded ();
		anim.SetBool ("Sliding", sliding);
		anim.SetBool ("Wallrunning", wallRunning);
		anim.SetFloat ("Vertical", zMove);
		anim.SetFloat ("Horizontal", xMove);
	}

	protected void OnControllerColliderHit(ControllerColliderHit collision) {
		if (collision.gameObject.tag == "Wall" && Input.GetButton(wallrunTag) && !wallRunning && zMove > 0 && !sliding && !doubleJumping && (wallrunCooldown <= currentWallrunCooldown)) {
			RaycastHit target;
			if(Physics.Raycast(transform.position, transform.forward, out target, characterRadius))
			{
				if(target.transform.gameObject.tag == "Wall")
				{
					wallRunning = wallrunUp = true;
					float angle = Vector3.Angle(transform.forward, target.normal);
					//Debug.Log(wallRunning);
					angle -= 180;
					transform.Rotate(0, -angle, 0);
					angle = Vector3.Angle(transform.forward, target.normal) - 180;
					if(angle != 180)
					{
						transform.Rotate(0, angle, 0);
					}
				}
			}
			else if (Physics.Raycast(transform.position, transform.right, out target, characterRadius*2))
			{
				if(target.transform.gameObject.tag == "Wall")
				{
					wallRunning = wallrunRight = true;
					float angle = Vector3.Angle(transform.forward, target.normal);
					angle -= 90;
					transform.Rotate(0, -angle, 0);
				}
			}
			else if (Physics.Raycast(transform.position, -transform.right, out target, characterRadius*2))
			{
					wallRunning = wallrunLeft = true;
					float angle = Vector3.Angle(transform.forward, target.normal);
					angle -= 90;
					transform.Rotate(0, angle, 0);
			}

		}
	}

	public void checkGrounded()
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position, -transform.up, out hit, character.floorcast)) { //If shit be fucked up with the jump, decrease floorcast and raise the character controller capsule a tad
			isGrounded = true;
		}
		else
			isGrounded = false;
	}

}
