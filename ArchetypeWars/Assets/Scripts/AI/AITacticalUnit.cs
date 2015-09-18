using UnityEngine;
using System.Collections;

public class AITacticalUnit : MonoBehaviour {

	private float time = 0.0f;
	private float commandTime,dCommandTime = 12.0f; //command agents when the time is right
	private System.Random rand;
	// Use this for initialization
	void Start () {
		commandTime = time - 1.0f;
		rand = new System.Random ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (time > commandTime)
		{
			assignTargets();
			Strategize();
		}

		time += Time.deltaTime;
	}

	public Vector3 LookForAmbushPoint(AI_Logic.Strategy strategy, Transform agent, Transform threat)
	{
		/*Generates a waypoint for the agent to go to in order to achieve a certain strategy/goal
		 * e.g. sneaking up on the threat;backing off to avoid more danger; approaching the threat to get better shooting accuracy
		 **/
		Vector3 ambushPoint = Vector3.zero;

		//generating an ambush point around the threat's radius
		if (strategy == AI_Logic.Strategy.Sneak) {
			Vector3 radius = agent.position - threat.position;
			float theta = 5 * rand.Next(1,9); //theta E [5, 45)
			int angleDir = rand.Next (1, 10); //random sign:- or +
			if (angleDir < 5)
				theta = theta * (-1);
			
			Vector3 dP = Quaternion.AngleAxis(theta, Vector3.up) * (-threat.forward.normalized); //find a point behind the threat
			float rLength = 5.09f + (float)rand.Next(0, Mathf.Max ((int)radius.magnitude - 5,1));
			dP = dP * rLength;
			print ("radius = " + radius.magnitude);
			ambushPoint = threat.position + dP;	

		} 
		else 
		{
			Vector3 radius = agent.position - threat.position;
			Vector3 dP = Quaternion.AngleAxis(15 * rand.Next(1,24), Vector3.up) * radius.normalized; //rotate radius by random angle theta E [15,360]
			float rLength = 1.0f;
			if (strategy == AI_Logic.Strategy.Approach)
			{
				rLength = 5.09f + (float)rand.Next(0, Mathf.Max ((int)radius.magnitude - 5,1)); //reduce the radius between the agent and threat
			}

			else if (strategy == AI_Logic.Strategy.StepBack)
			{
				rLength =  (float) rand.Next((int)(radius.magnitude + 5), (int) (radius.magnitude + 40));
			}
			dP = dP * rLength;
			print ("radius = " + radius.magnitude);
			ambushPoint = threat.position + dP;	
		}

		return ambushPoint;
	}

	public void assignTargets()
	{
		//assign targets to all enemies
		for (int i = 0;i < RoundManager.enemies.Count; i++)
		{

			if (RoundManager.enemies[i] != null)
			{
				getTarget(RoundManager.enemies[i]);
				RoundManager.enemies[i].GetComponent<AI_Logic>().mainState = AI_Logic.FiniteState.Chase;
				commandTime = time + dCommandTime;
			}
		}
	}

	//Assigns strategies to agents.this includes move styles and shooting rage for difficulty purposes
	public void Strategize()
	{
		for (int a = 0; a < RoundManager.enemies.Count; a++)
		{
			if (RoundManager.enemies[a] != null)
			{
				//strategies
				int option = rand.Next (0, 10); 
				if (option < 5)
					RoundManager.enemies[a].GetComponent<EnemyCharacter>().strategy = AI_Logic.Strategy.Approach;
				else
					RoundManager.enemies[a].GetComponent<EnemyCharacter>().strategy = AI_Logic.Strategy.Sneak;
			}


		}

	}

	public void getTarget(Transform enemy)
	{
		float priority = 999.0f;
		int index = 0;

		for (int i = 0;i < RoundManager.players.Count; i++)
		{

			Transform threat = RoundManager.players[i];
			int aggro = threat.gameObject.GetComponent<CharacterBase>().getAggro();
			if (aggro !=0)
			{
				float distance = Vector3.Distance(threat.position, enemy.position);
				float prio = aggro/distance;
				if (prio < priority)
				{
					priority = prio; //update priority
					index = i;
				}
			}


		}


		
		enemy.GetComponent<AI_Logic>().threat = RoundManager.players[index]; //choose the closest player to be the threat
	}
}

