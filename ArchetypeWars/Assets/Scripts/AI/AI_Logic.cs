using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

public class AI_Logic : MonoBehaviour {

	// Use this for initialization
	public enum FiniteState
	{
		Patrol, Chase, Attack, Still
	}

	public enum AttackState
	{
		InPosition, InMotion
	}

	public enum WayPoint
	{
		ambush
	}

	public FiniteState mainState;
	public AttackState attackState;
	public WayPoint targetWayPointType;
	EnemyMovement enemyMovement;
	EnemyCharacter character;
	NavMeshAgent agent;
	public static List<Transform> threats = new List<Transform>();
	float time = 0.0f;
	float logicTime, visionTime, threatSpottedTimeOut, ambushTimeOut, motionTimeOut;
	public float dLogicTime, dVisionTime, dThreatSpottedTimeOut,dAmbushTimeOut, dMotionTimeOut; 
	//protected NavMeshAgent agent;
	//public Transform asset; //somethin to protect or patrol around
	//public Transform probe; //raycast point
	private Vector3 dir;
	public Transform threat; //the target player to attack
	float targetOffset;
	public AITacticalUnit tactics;
	private System.Random rand;

	void Start () 
	{
		mainState = FiniteState.Still;
		attackState = AttackState.InPosition;
		targetWayPointType = WayPoint.ambush;

		enemyMovement = GetComponent<EnemyMovement> ();
		character = GetComponent<EnemyCharacter>();
		agent = gameObject.GetComponent<NavMeshAgent> ();
		logicTime = dLogicTime;
		visionTime = dVisionTime;
		threatSpottedTimeOut = dThreatSpottedTimeOut;
		threat = null;
		//agent = enemyMovement.agent;
		//agent.speed = 1.5f;
		dir = Vector3.zero;
		//targetOffset = enemyChar.targetOffset;
		ambushTimeOut = 0.0f;
		dAmbushTimeOut = 2.0f;
		dMotionTimeOut = 8.0f;
		motionTimeOut = time + dMotionTimeOut;
		tactics = RoundManager.AITactics;
		rand = new System.Random ();
	}
	
	// Update is called once per frame
	void Update () 
	{

		//Debug.DrawRay (probe.position , enemyChar.visionRadius * dir.normalized , Color.red);
			logicTime += dLogicTime;


			switch (mainState)
			{
			case FiniteState.Patrol:

		/*		bool threatSpotted = false;
				if (time > visionTime)
				{
					visionTime += dVisionTime;

					threat = LookForThreat ();
					if (threat != null )//threat was found
					{
						mainState = FiniteState.Attack;
						agent.SetDestination(threat.position);
						print("time: " + time +" ;chasing the threat");
						threatSpotted = true;
						threatSpottedTimeOut = time + dThreatSpottedTimeOut;
					}
				}

				if (!threatSpotted)
				{
					Vector3 patrolPoint = asset.position;
					agent.SetDestination(patrolPoint);
					print("time: " + time +" ;patrolling around the asset");
				}*/
				break;
			case FiniteState.Chase:
		{
			
			if((transform.position - threat.position).magnitude < character.shootingRange)
			{
				mainState = FiniteState.Attack;
				agent.stoppingDistance = 0.01f;
				int dT = rand.Next((int)dAmbushTimeOut, 10);
				ambushTimeOut = time + dT; //randomise how long the agent stays at some ambush point
				agent.Stop();

			}
				
		}
				break;

			case FiniteState.Attack:
		{
			if((transform.position - threat.position).magnitude > character.shootingRange)
			{
				mainState = FiniteState.Chase;
				agent.stoppingDistance = character.stoppingRange;
			}

			if (attackState == AttackState.InPosition)
			{


				if ((time > ambushTimeOut) || (character.hitCount > character.endurance)) //agent has to move away from danger zone
				{
					print ("Got to move away");
					attackState = AttackState.InMotion;
					motionTimeOut = time + dMotionTimeOut; //allows the agent to stop after some time if it failed to reach the target point
					character.hitCount = 0;
					Vector3 targetWayPoint = tactics.LookForAmbushPoint(transform, threat);
					agent.SetDestination (targetWayPoint);
					targetWayPointType = WayPoint.ambush;
				}
			}

			else if (attackState == AttackState.InMotion)
			{
				print ("in motion");
				if ((enemyMovement.agentDestReached()) || (time > motionTimeOut))
				{
					agent.Stop ();
					if (targetWayPointType == AI_Logic.WayPoint.ambush) //reached an ambush point
					{
						attackState = AI_Logic.AttackState.InPosition;
						int dT = rand.Next((int)dAmbushTimeOut, 10);
						ambushTimeOut = time + dT;
					}

				}
			}


			time += Time.deltaTime;
		}
				//print("time " + time +" ;attacking");
			/*
				if (time > visionTime)
				{
					visionTime += dVisionTime;
					
					Transform threatT = LookForThreat ();
					if (threatT != null )//threat was found
					{
						threat = threatT; //update the threat transform
						threatSpottedTimeOut += dThreatSpottedTimeOut; //postpone time out
						print ("saw her");
						enemyMovement.Shoot(7.0f);
					}
					else
					{
						print ("couldnt see her");
					}

				}

				agent.SetDestination(threat.position); ///always follow the threat in the attack state
				if (time > threatSpottedTimeOut) //the threat has been out of sight for a while
				{
					mainState = FiniteState.Patrol;
					visionTime = time + dVisionTime;
					print("time: " + time +" ;time to patrol");
				}
*/
				break;
				
			}
		time += Time.deltaTime;



	}

	/*
	protected Transform LookForThreat() //Checks if any of the players are visible to the agent and returns the index of the closest player
	{
		foreach (Transform threat in threats)
		{
			dir = threat.position + targetOffset * Vector3.up - probe.position; //direction to the threat.offset from the threat's feet
			//print ("threat distance: " + dir.magnitude + " radius: " + enemyChar.visionRadius);
			if (dir.magnitude < enemyChar.visionRadius) //threat within vision radius
			{
				//print ("closer");
				if (Mathf.Abs(Vector3.Angle (dir.normalized, transform.forward)) < enemyChar.visionField) //threat within agent's field of vision
				{
					//print ("within the field");
					RaycastHit hit;
					if (Physics.Raycast(probe.position , dir, out hit, enemyChar.visionRadius))
					{
						//print ("hit something");
						if (hit.collider.gameObject.tag == threat.gameObject.tag)
						{
							//print ("saw the threat");
							return threat;

						}
					}
				}
			}

		}

		return null;
	}*/

	//Function to determine which character this AI chases and attacks
	public Vector3 getTarget()
	{
		return threat.position;
	}


	//Use this to calculate which character in the threats list to attack. 
	//Each character will have a CharacterBase component containing their aggro.
	//Threat level is directly proportional to a character's aggro, and inversely proportional to their distance from this AI


}
