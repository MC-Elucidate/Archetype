using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	//NB: Changes made: update() - Only changed the content in the switch block and added a random generator as private attribute for the class
	
	private NavMeshAgent agent;
	protected Animator anim;
	// Use this for initialization
	private bool isGrounded = false;
	public bool weaponHeld = true;

	//private float verticalVel = 0f;
	private float meleeTime = 4f, currentMelee = 0;

	//change in position attributes
	protected float xMove = 0.0f;
	protected float zMove = 0.0f;
	
	protected EnemyCharacter character;
	protected AI_Logic logic; //AI control unit for each agent

	//utilities 
	private System.Random rand;

	


	protected float time,shootTimeOut; //attributes to keep track of time and time outs
	protected bool rotated = false; //check if rotation was set somewhere else

	void Start () 
	{
		/**Gets called by unity when the script is initialized
		 * */
		anim = GetComponent<Animator>();
		character = gameObject.GetComponent<EnemyCharacter> ();
		logic = gameObject.GetComponent<AI_Logic> ();
		agent = gameObject.GetComponent<NavMeshAgent> ();
		time = 0.0f;
		shootTimeOut = 0.0f;

		agent.stoppingDistance = character.stoppingRange;
		rand = new System.Random ();

	}

	void Update () {
		/*Gets called by unity once per frame
		 */
		if (currentMelee > 0)
			currentMelee -= Time.fixedDeltaTime;

		if (character.freemove && logic.threat != null) {
			switch (logic.mainState) {
			case AI_Logic.FiniteState.Chase:
				{
					if (agent.updateRotation == false) {
						agent.updateRotation = true;
					}
					agent.SetDestination (logic.threat.position);
					
				}
				break;
			
			case AI_Logic.FiniteState.Attack:
				{
					if (agent.updateRotation == true) {
						agent.updateRotation = false;
						
					}
					lookAt (logic.threat.position);
					float dist = (float)Vector3.Distance (transform.position, logic.threat.position);
					if (dist < AITacticalUnit.minimum_melee_distance && (currentMelee <= 0)) { //very close to target
						character.meleeAttack ();
						currentMelee = meleeTime;
					}
					else if (dist > AITacticalUnit.minimum_melee_distance){
					RaycastHit hit;
					if (Physics.Linecast(character.shot_source.position, logic.threat.position + new Vector3(0f,0.5f, 0f), out hit))
						{


						//print(hit.transform.gameObject.tag);
						if (hit.transform.gameObject.collider == logic.threat.collider && (hit.point - character.shot_source.position).magnitude <= character.shootingRange) //if the agent can see a player
							{
								character.ShootWeapon (logic.threat);
							}
							
						}
						
						
					}
	
					if (logic.attackState == AI_Logic.AttackState.InPosition) { //agent positioned at some waypoint
						//Nothing implemented
					} 
				else if (logic.attackState == AI_Logic.AttackState.InMotion) { //agent moving to some waypoint
						if (agent.velocity.magnitude == 0) {//agent has been paused
							int randResumingChoice = rand.Next (1, 60);
							if (randResumingChoice == 15) {
								agent.Resume ();
							}
	
						} else {
							int randStoppingChoice = rand.Next (1, 60);
							if (randStoppingChoice == 15) {
								agent.Stop ();
							}
	
						}
					}
				}
				break;
			}
		}
		else {agent.Stop();}
	
		//for animating movement
		Vector3 moveDir = agent.velocity;
		moveDir.y = 0;
		
		
		//determine vertical and horizontal change in movement relative to the transform
		float delta_vert = 0.0f;
		float delta_hor = 0.0f;
		
		
		if (moveDir.magnitude > 0.2f) //minimum velocity
		{
			moveDir.Normalize ();
			Vector2 moveDir2d = new Vector2(moveDir.x, moveDir.z);
			Vector2 forward2d = new Vector2(transform.forward.x, transform.forward.z);
			Vector2 right2d = new Vector2(transform.right.x, transform.right.z);
			
			delta_vert = Vector3.Dot(forward2d, moveDir2d) ;
			delta_hor = Vector3.Dot(right2d, moveDir2d) ;
			
			
			
		}

		xMove = delta_hor;
		zMove = delta_vert;



	}

	void FixedUpdate()
	{
		/*Gets called by unity for physics updates
		 */
		//setting animator parameters for animation 
		anim.SetBool ("WeaponHeld", weaponHeld);
		anim.SetBool ("Alive", character.alive);
		anim.SetFloat ("Poise", character.currentPoise);
		anim.SetFloat ("Vertical", zMove);
		anim.SetFloat ("Horizontal", xMove);
		anim.SetBool ("Melee", character.melee);
		anim.SetInteger ("MeleeCount", character.currentMelee);
	}
	

	public bool agentDestReached()
	{
		/*checks if navmesh agent has reached destination
		 * */
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

	public void lookAt(Vector3 pos)
	{
		/*Changes the agent's rotation to look towards pos
		 */
		Transform targetOrientation = transform;
		targetOrientation.LookAt(pos);
		Vector3 rotation_temp = new Vector3(0, targetOrientation.localEulerAngles.y, 0);//only rotate in the y azis
		targetOrientation.localEulerAngles = rotation_temp;
		transform.rotation = Quaternion.Slerp(transform.rotation, targetOrientation.rotation, Time.deltaTime * 0.3f * 5.0f);

	}
}
