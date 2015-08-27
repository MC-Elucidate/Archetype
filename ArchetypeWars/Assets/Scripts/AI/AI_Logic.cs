using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

public class AI_Logic : MonoBehaviour {

	// Use this for initialization
	public enum FiniteState
	{
		Patrol, Chase, Attack
	}

	public FiniteState mainState;
	EnemyMovement enemyMovement;
	EnemyCharacter character;
	public static List<Transform> threats = new List<Transform>();
	float time = 0.0f;
	float logicTime, visionTime, threatSpottedTimeOut;
	public float dLogicTime, dVisionTime, dThreatSpottedTimeOut; 
	//protected NavMeshAgent agent;
	//public Transform asset; //somethin to protect or patrol around
	//public Transform probe; //raycast point
	private Vector3 dir;
	public Transform threat; //the target player to attack
	float targetOffset;


	void Start () 
	{
		mainState = FiniteState.Chase;

		enemyMovement = GetComponent<EnemyMovement> ();
		character = GetComponent<EnemyCharacter>();
		logicTime = dLogicTime;
		visionTime = dVisionTime;
		threatSpottedTimeOut = dThreatSpottedTimeOut;
		threat = null;
		//agent = enemyMovement.agent;
		//agent.speed = 1.5f;
		dir = Vector3.zero;
		//targetOffset = enemyChar.targetOffset;
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
				//to be coded
			calculateThreat();
			if((transform.position - threat.position).magnitude < character.shootingRange)
				mainState = FiniteState.Attack;
		}
				break;

			case FiniteState.Attack:
		{
			if((transform.position - threat.position).magnitude > character.shootingRange)
				mainState = FiniteState.Chase;
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
	private void calculateThreat()
	{
		threat = threats [0];
	}

}
