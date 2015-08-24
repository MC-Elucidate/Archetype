using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	public NavMeshAgent agent;
	protected Animator anim;
	public float speed = 10f;
	protected CharacterController charCon;
	// Use this for initialization
	private bool doubleJumping = false, wallRunning  = false, wallrunUp = false, wallrunLeft = false, wallrunRight = false, sliding = false, isGrounded = false;
	public bool weaponHeld = true;
	public float rotSpeed = 7.0f;
	public float slidespeed = 1.5f;
	public float jumpPower = 3f, jumpGravity = 3f;
	
	
	
	private float verticalVel = 0f;
	private float slideTime = 1.1f, currentSlide = 0;
	private float gravity = -9.8f;
	private float jumpTime = .25f, currentJump = 0;
	private float zMove = 0, xMove = 0;
	private float wallrunTime = 1.5f, currentWallrun = 0, wallrunCooldown = 1.5f, currentWallrunCooldown = 0;
	private float characterRadius;
	
	protected EnemyCharacter character;
	protected AI_Logic logic;
	

	public bool vertical_Move, horizontal_Move, jump_Move, wallrun_Move, slide_Move, fire_Move, special1_Move;
	protected float time,shootTimeOut; //time and timeout floats
	protected bool rotated = false; //check if rotation was set somewhere else

	void Start () 
	{
		anim = GetComponent<Animator>();
		charCon = gameObject.GetComponent<CharacterController>();
		character = gameObject.GetComponent<EnemyCharacter> ();
		characterRadius = character.characterRadius;
		logic = gameObject.GetComponent<AI_Logic> ();

		//AI control conditions
		vertical_Move = false;
		horizontal_Move = false;
		jump_Move = false;
		wallrun_Move = false;
		slide_Move = false;
		fire_Move = false;
		special1_Move = false;

		time = 0.0f;
		shootTimeOut = 0.0f;


	}
	
	// Update is called once per frame
	void Update () {
	

		//Debug.DrawRay (transform.position + Vector3.up , 30 *transform.forward, Color.red);
		if (rotated == false) //avoid rotation fighting among code sections changing the rotation
		{
			//match the nav agent's rotation
			transform.rotation = Quaternion.Slerp (transform.rotation, agent.transform.rotation, 2.0f * Time.deltaTime);
		}
		/*Vector3 moveDir = agent.transform.position - transform.position;
		moveDir.y = 0;
		moveDir.Normalize ();
		//print (moveDir);
		charCon.Move (speed * moveDir * Time.deltaTime);
		*/


		//Use custom method for checking if we are on the ground
		checkGrounded ();
		
		
		//If we are not wallrunning or sliding (we have free movement)
		if (!wallRunning && !sliding) {
			//determine the direction vector as to follow the navmesh agent
			Vector3 moveDir = agent.transform.position - transform.position;
			moveDir.y = 0;


			//determine vertical and horizontal floats relative to the transform
			float vert = 0.0f;
			float hor = 0.0f;
			float yaw = 0.0f;


			if (moveDir.magnitude > 0.6f) //minimum distance
			{
				moveDir.Normalize ();
				Vector2 moveDir2d = new Vector2(moveDir.x, moveDir.z);
				Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
				Vector2 right2d = new Vector2(transform.right.x, transform.right.z);

				vert = Vector3.Dot(forward2d, moveDir2d) ;
				hor = Vector3.Dot(right2d, moveDir2d) ;



			}
		
			currentWallrunCooldown+=Time.deltaTime;
			wallRunning = wallrunLeft = wallrunRight = wallrunUp = false;

			//If we are in the air
			if (!isGrounded) 
			{
				verticalVel += (gravity  * Time.deltaTime); //gravity doing its thing
				if(jump_Move && !doubleJumping) //Double jump if we haven't already done so
				{
					verticalVel = jumpPower;
					currentJump = 0f;
					gravity = jumpGravity;
					doubleJumping = true;
				}
				
				if(jump_Move) //Holding jump to accelerate upwards (platformer style)
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
			else if(jump_Move)
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
				
				if (slide_Move) //can only slide when on ground
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
				
				if(!wallrun_Move) //Release wallrun button. Ends wallrun. Begins freefall
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
				
				if(jump_Move && !doubleJumping) //Jump off wall. Allows player to leap in direction being held. Uses double jump
				{
					//determine the direction vector as to follow the navmesh agent
					Vector3 moveDir = agent.transform.position - transform.position;
					moveDir.y = 0;
					
					
					//determine vertical and horizontal floats relative to the transform
					float vert = 0.0f;
					float hor = 0.0f;
					float yaw = 0.0f;
					
					
					if (moveDir.magnitude > 0.6f) //minimum distance
					{
						moveDir.Normalize ();
						Vector2 moveDir2d = new Vector2(moveDir.x, moveDir.z);
						Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
						Vector2 right2d = new Vector2(transform.right.x, transform.right.z);
						
						vert = Vector3.Dot(forward2d, moveDir2d) ;
						hor = Vector3.Dot(right2d, moveDir2d) ;
					}

					zMove = vert * jumpPower * speed;
					xMove = hor * jumpPower * speed;
					verticalVel = jumpPower;
					wallRunning = wallrunLeft = wallrunRight = false;
					currentWallrun = 0f;
					doubleJumping = true;
					currentWallrunCooldown = 0;
				}
				
				if(!wallrun_Move) //Release wallrun button. Ends wallrun. Begins freefall
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

		/*
		if (special1_Move) {
			character.special1();
			Debug.Log ("special1");
		}*/
		
		
		//FIRE!
		if (fire_Move) 
		{ 
			character.ShootWeapon (logic.threat);
		}

		//resetting move commands
		rotated = false;
		if (time > shootTimeOut)
		{
			fire_Move = false;
		}

		time += Time.deltaTime;
	}

	void FixedUpdate()
	{
		anim.SetBool ("Sliding", sliding);
		anim.SetBool ("WeaponHeld", weaponHeld);
		anim.SetBool ("Wallrunning", wallRunning);
		anim.SetFloat ("Vertical", zMove);
		anim.SetFloat ("Horizontal", xMove);
	}

	protected void OnControllerColliderHit(ControllerColliderHit collision) {
		if (collision.gameObject.tag == "Wall" && wallrun_Move && !wallRunning && zMove > 0 && !sliding && !doubleJumping && (wallrunCooldown <= currentWallrunCooldown)) {
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
						rotated = true;
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
					rotated = true;
				}
			}
			else if (Physics.Raycast(transform.position, -transform.right, out target, characterRadius*2))
			{
				wallRunning = wallrunLeft = true;
				float angle = Vector3.Angle(transform.forward, target.normal);
				angle -= 90;
				transform.Rotate(0, angle, 0);
				rotated = true;
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

	//action command methods invoked by AI logic
	public void Shoot(float duration)
	{
		fire_Move = true;
		shootTimeOut = time + duration;
	}

}
