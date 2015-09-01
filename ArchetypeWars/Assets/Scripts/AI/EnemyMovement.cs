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
	
	protected EnemyCharacter character;
	protected AI_Logic logic;
	

	//public bool vertical_Move, horizontal_Move, jump_Move, wallrun_Move, slide_Move, 
	public bool fire_Move, special1_Move;
	protected float time,shootTimeOut; //time and timeout floats
	protected bool rotated = false; //check if rotation was set somewhere else

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
	
		/*
		//Debug.DrawRay (transform.position + Vector3.up , 30 *transform.forward, Color.red);
		if (rotated == false) //avoid rotation fighting among code sections changing the rotation
		{
			//match the nav agent's rotation
			transform.rotation = Quaternion.Slerp (transform.rotation, agent.transform.rotation, 2.0f * Time.deltaTime);
		}
		/*
		 * Vector3 moveDir = agent.transform.position - transform.position;
		moveDir.y = 0;
		moveDir.Normalize ();
		//print (moveDir);
		charCon.Move (speed * moveDir * Time.deltaTime);
		*/

		switch (logic.mainState) {
		case AI_Logic.FiniteState.Chase:
			{
				if (!character.freemove)
					agent.Stop ();
				else {
					agent.SetDestination (logic.getTarget ());
				}
			}
			break;
		
		case AI_Logic.FiniteState.Attack:
			{
				character.ShootWeapon (logic.threat);
				//agent.SetDestination (logic.getCover ());
			}
			break;
		}
	

	}

	void FixedUpdate()
	{

		anim.SetBool ("Sliding", sliding);
		anim.SetBool ("WeaponHeld", weaponHeld);
		anim.SetBool ("Alive", character.alive);
		//anim.SetBool ("Stunned", character.stunned);
		//anim.SetBool ("KnockedDown", character.knockedDown);
		anim.SetFloat ("Poise", character.currentPoise);
		//anim.SetFloat ("Vertical", zMove);
		//anim.SetFloat ("Horizontal", xMove);
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

}
