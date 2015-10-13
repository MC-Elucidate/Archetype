using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

public class AI_Logic : MonoBehaviour {
	
	//enums
	public enum FiniteState
	{
		Patrol, Chase, Attack, Still
	}

	public enum Strategy
	{
		Approach, StepBack, Sneak
	}

	public enum AttackState
	{
		InPosition, InMotion, Confused
	}

	public enum WayPoint
	{
		ambush
	}

	//attributes
	public FiniteState mainState;
	public AttackState attackState;
	public WayPoint targetWayPointType;
	EnemyMovement enemyMovement;
	EnemyCharacter character;
	NavMeshAgent agent;
	public static List<Transform> threats = new List<Transform>();
	float time = 0.0f;
	float logicTime, visionTime, threatSpottedeltaTimeimeOut, ambushTimeOut, motionTimeOut;
	public float delta_LogicTime, delta_VisionTime, deltaTime_ThreatSpottedTimemeOut,delta_AmbushTimeOut, delta_MotionTimeOut; 
	private Vector3 dir;
	public Transform threat; //the target player to attack
	float targetOffset;
	public AITacticalUnit tactics;
	private System.Random rand;

	void Start () 
	{
		/**Gets called by unity when the script is initialized
		 * */

		//initialization
		mainState = FiniteState.Still;
		attackState = AttackState.InPosition;
		targetWayPointType = WayPoint.ambush;

		enemyMovement = GetComponent<EnemyMovement> ();
		character = GetComponent<EnemyCharacter>();
		agent = gameObject.GetComponent<NavMeshAgent> ();
		logicTime = delta_LogicTime;
		visionTime = delta_VisionTime;
		threatSpottedeltaTimeimeOut = deltaTime_ThreatSpottedTimemeOut;
		threat = null;
		dir = Vector3.zero;
		ambushTimeOut = 0.0f;
		delta_AmbushTimeOut = 2.0f;
		delta_MotionTimeOut = 8.0f;
		motionTimeOut = time + delta_MotionTimeOut;
		tactics = RoundManager.AITactics;
		rand = new System.Random ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*Gets called by unity once per frame
		 */
		logicTime += delta_LogicTime;
			
		//the threats are in cloak mode
		if ((threat == null) && (attackState != AttackState.Confused))
		{
			attackState = AttackState.Confused;
		}
			
		//there's at least one threat not in cloak mode
		if ((threat != null) && (attackState == AttackState.Confused))
		{
			attackState = AttackState.InPosition;
			int deltaTime = rand.Next((int)delta_AmbushTimeOut, (int)delta_AmbushTimeOut + 7);
			ambushTimeOut = time + deltaTime;
		}

		if (threat != null)
		{
			switch (mainState)
			{
				case FiniteState.Chase:
				{
					//add if attackstate = confused
					if((transform.position - threat.position).magnitude < character.shootingRange) //threat is within shooting range
					{
						mainState = FiniteState.Attack;
						agent.stoppingDistance = 0.01f;
						int deltaTime = rand.Next((int)delta_AmbushTimeOut, (int)delta_AmbushTimeOut + 2);
						ambushTimeOut = time + deltaTime; //randomise how long the agent stays at some ambush point
						agent.Stop();
		
					}
					
				}
				break;

				case FiniteState.Attack:
				{
					if((transform.position - threat.position).magnitude > character.shootingRange) //threat is too far to shoot at
					{
						mainState = FiniteState.Chase;
						agent.stoppingDistance = character.stoppingRange;
					}

					if (attackState == AttackState.InPosition)
					{


						if ((time > ambushTimeOut) || (character.hitCount > character.endurance)) //agent has to move away from danger zone or it's time to re position
						{
			
							attackState = AttackState.InMotion;
							motionTimeOut = time + delta_MotionTimeOut; //allows the agent to stop after some time if it failed to reach the target point
							character.hitCount = 0;
							Vector3 targetWayPoint;
							if (character.hitCount > character.endurance) //agent shot more times than it can tolerate
								targetWayPoint = tactics.LookForAmbushPoint(Strategy.StepBack,transform, threat); //agent has to move away from danger
							else //ambush timeout
								targetWayPoint = tactics.LookForAmbushPoint(character.strategy,transform, threat); //find a point to allow the agent's strategy
							agent.SetDestination (targetWayPoint);
							targetWayPointType = WayPoint.ambush;
						}
					}

					else if (attackState == AttackState.InMotion)
					{

						if ((enemyMovement.agentDestReached()) || (time > motionTimeOut))
						{
							agent.Stop ();
							if (targetWayPointType == AI_Logic.WayPoint.ambush) //reached an ambush point
							{
								attackState = AI_Logic.AttackState.InPosition;
								int deltaTime = rand.Next((int)delta_AmbushTimeOut, (int)delta_AmbushTimeOut + 7);//how long should the agent hold the ambush position
								ambushTimeOut = time + deltaTime;
							}
		
						}
					}
		

					time += Time.deltaTime;
				}
				break;
					
			}
		}
		time += Time.deltaTime;

	}

	public Vector3 getTarget()
	{
		/*Determines which character this AI chases and attacks
		 * */
		return threat.position;
	}
	
}
