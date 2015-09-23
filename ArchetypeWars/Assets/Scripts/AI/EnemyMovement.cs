using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	private NavMeshAgent agent;
	protected Animator anim;
	// Use this for initialization
	private bool sliding = false, isGrounded = false;
	public bool weaponHeld = true;
	public float slidespeed = 1.5f;

	private float verticalVel = 0f;
	private float slideTime = 1.1f, currentSlide = 0;

	protected float xMove = 0.0f;
	protected float zMove = 0.0f;
	
	protected EnemyCharacter character;

	//AI control unit for each agent
	protected AI_Logic logic;

	

	public bool fire_Move, special1_Move;
	protected float time,shootTimeOut; //attributes to keep track of time and time outs
	protected bool rotated = false; //check if rotation was set somewhere else

	//Melee cooldowns
	private float meleeCooldown = 3, currentMeleeTime = 0;

	void Start () 
	{

		anim = GetComponent<Animator>();
		character = gameObject.GetComponent<EnemyCharacter> ();
		logic = gameObject.GetComponent<AI_Logic> ();
		agent = gameObject.GetComponent<NavMeshAgent> ();
		time = 0.0f;
		shootTimeOut = 0.0f;

		agent.stoppingDistance = character.stoppingRange;
		fire_Move = false;

	}
	
	// Update is called once per frame
	void Update () {
	
		//Update melee cooldown
		if (currentMeleeTime > 0)
			currentMeleeTime -= Time.deltaTime;

		switch (logic.mainState) {
		case AI_Logic.FiniteState.Chase:
			{
				agent.SetDestination (logic.getTarget ());
			}
			break;
		
		case AI_Logic.FiniteState.Attack:
			{
				if (logic.attackState == AI_Logic.AttackState.InPosition)
				{
					lookAt(logic.threat.position);
					if (character.melee && !(anim.GetCurrentAnimatorStateInfo (1).IsTag ("MeleeAttack"))) //When the melee animation is finished
						character.meleeAttackEnd ();
					else if (Vector3.Distance(transform.position, logic.threat.position) < 3 && currentMeleeTime <= 0) //adjust this 
					{
					//closer to target
						character.meleeAttack ();
						currentMeleeTime = meleeCooldown;
						
					}
					else
					{
						character.ShootWeapon (logic.threat);
					}

				//agent.SetDestination (logic.getCover ());
				}

				

			}
			break;
		}
	
		//for animating movement
		Vector3 moveDir = agent.velocity;
		moveDir.y = 0;
		
		
		//determine vertical and horizontal floats relative to the transform
		float vert = 0.0f;
		float hor = 0.0f;
		
		
		if (moveDir.magnitude > 0.2f) //minimum velocity
		{
			moveDir.Normalize ();
			Vector2 moveDir2d = new Vector2(moveDir.x, moveDir.z);
			Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
			Vector2 right2d = new Vector2(transform.right.x, transform.right.z);
			
			vert = Vector3.Dot(forward2d, moveDir2d) ;
			hor = Vector3.Dot(right2d, moveDir2d) ;
			
			
			
		}

		xMove = hor;
		zMove = vert;


		anim.SetBool ("Sliding", sliding);
		anim.SetBool ("WeaponHeld", weaponHeld);
		anim.SetBool ("Alive", character.alive);
		anim.SetFloat ("Poise", character.currentPoise);
		anim.SetFloat ("Vertical", zMove);
		anim.SetFloat ("Horizontal", xMove);
		anim.SetBool ("Melee", character.melee);
		anim.SetInteger ("MeleeCount", character.currentMelee);
	}

	void FixedUpdate()
	{
	}

	/*
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
*/
	//action command methods invoked by AI logic
	public void Shoot(float duration)
	{
		fire_Move = true;
		shootTimeOut = time + duration;
	}

	//checks if navmesh agent has reached destination
	public bool agentDestReached()
	{
		bool atDestination = false;
		if (!agent.pathPending) {
						if (agent.remainingDistance <= agent.stoppingDistance) {
								if (!agent.hasPath || (agent.velocity.sqrMagnitude == 0f)) { //agent has reached the target waypoint destination
							atDestination = true;
								}
						}
				}
		return atDestination;
	}

	/*Changes the agent's rotation to look towards pos
		 */
	public void lookAt(Vector3 pos)
	{
		Transform targetOrientation = transform;
		targetOrientation.LookAt(pos);
		Vector3 rot = new Vector3(0, targetOrientation.localEulerAngles.y, 0);
		targetOrientation.localEulerAngles = rot;
		transform.rotation = Quaternion.Slerp(transform.rotation, targetOrientation.rotation, Time.deltaTime * 0.3f * 5.0f);

	}
}
