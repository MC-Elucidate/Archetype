using UnityEngine;
using System.Collections;

public class AITacticalUnit : MonoBehaviour {

	//initialization
	private float time = 0.0f;
	private float commandTime,delta_CommandTime = 3.0f; //command agents when the time is right
	private System.Random rand;

	//static attributes
	public static int maximum_rage = 20;
	public static float minimum_melee_distance = 2;

	public int neighboringDistance = 25;

	void Start () {
		/**Gets called by unity when the script is initialized
		 * */ 
		//more initialization
		commandTime = time - 1.0f;
		rand = new System.Random ();
	}

	void Update () 
	{
		/**Gets called by unity when the script is initialized
		 * */
		if (time > commandTime) //time to command agents to work together
		{
			assignTargets();
			Strategize();
		}

		time += Time.deltaTime;
	}

	//Ambush Point determination function for Gun-based classes
	public Vector3 LookForAmbushPoint(AI_Logic.Strategy strategy, Transform agent, Transform threat)
	{
		if (agent.GetComponent<EnemyCharacter> ().enemyType == "Gun") {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			Vector2 computationVector = Vector3.zero;
			//separation part of the boid algorithm
			for (int a = 0; a < enemies.Length; a++) {
				if ((enemies[a].transform.position - agent.transform.position).magnitude < neighboringDistance)
				{
					computationVector.x += agent.transform.position.x - enemies [a].transform.position.x;
					computationVector.y += agent.transform.position.z - enemies [a].transform.position.z;
				}

			}
	
			computationVector.x /= enemies.Length;
			computationVector.y /= enemies.Length;
	
			Vector3 waypoint = new Vector3 (computationVector.x, computationVector.y, agent.transform.position.z) * -5;
			return waypoint;
		} else {
			Vector3 ambushPoint = Vector3.zero;
			
			//generating an ambush point around the threat's radius
			if (strategy == AI_Logic.Strategy.Sneak) {
				Vector3 radius = agent.position - threat.position;
				float theta = 5 * rand.Next(1,9); //theta is in range [5, 45)
				int angleDir = rand.Next (1, 10); //random sign:- or +
				if (angleDir < 5)
					theta = theta * (-1);
				
				Vector3 delta_Position = Quaternion.AngleAxis(theta, Vector3.up) * (-threat.forward.normalized); //find a point behind the threat and rotate by theta
				float radius_Length = 5.09f + (float)rand.Next(0, Mathf.Max ((int)radius.magnitude - 5,1)); //randomize the radius length
				delta_Position = delta_Position * radius_Length; //scale the change in position
				ambushPoint = threat.position + delta_Position;	//offset threat's position by delta_Position
				
			} 
			else 
			{
				Vector3 radius = agent.position - threat.position;
				Vector3 delta_Position = Quaternion.AngleAxis(15 * rand.Next(1,24), Vector3.up) * radius.normalized; //rotate radius by random angle theta E [15,360]
				float radius_Length = 1.0f;
				if (strategy == AI_Logic.Strategy.Approach)
				{
					radius_Length = 5.09f + (float)rand.Next(0, Mathf.Max ((int)radius.magnitude - 5,1)); //reduce the radius between the agent and threat
				}
				
				else if (strategy == AI_Logic.Strategy.StepBack)
				{
					radius_Length =  (float) rand.Next((int)(radius.magnitude + 5), (int) (radius.magnitude + 40));
				}
				delta_Position = delta_Position * radius_Length;
				ambushPoint = threat.position + delta_Position;	
			}
			
			return ambushPoint;
		}

	}

	//Ambush Point determination function for Melee-based classes
	public Vector3 LookForAmbushPoint(AI_Logic.Strategy strategy, Transform agent, Transform threat, int a)
	{
		/*Generates a waypoint for the agent to go to in order to achieve a certain strategy/goal
		 * e.g. sneaking up on the threat;backing off to avoid more danger; approaching the threat to get better shooting accuracy
		 **/
		Vector3 ambushPoint = Vector3.zero;

		//generating an ambush point around the threat's radius
		if (strategy == AI_Logic.Strategy.Sneak) {
			Vector3 radius = agent.position - threat.position;
			float theta = 5 * rand.Next(1,9); //theta is in range [5, 45)
			int angleDir = rand.Next (1, 10); //random sign:- or +
			if (angleDir < 5)
				theta = theta * (-1);
			
			Vector3 delta_Position = Quaternion.AngleAxis(theta, Vector3.up) * (-threat.forward.normalized); //find a point behind the threat and rotate by theta
			float radius_Length = 5.09f + (float)rand.Next(0, Mathf.Max ((int)radius.magnitude - 5,1)); //randomize the radius length
			delta_Position = delta_Position * radius_Length; //scale the change in position
			ambushPoint = threat.position + delta_Position;	//offset threat's position by delta_Position

		} 
		else 
		{
			Vector3 radius = agent.position - threat.position;
			Vector3 delta_Position = Quaternion.AngleAxis(15 * rand.Next(1,24), Vector3.up) * radius.normalized; //rotate radius by random angle theta E [15,360]
			float radius_Length = 1.0f;
			if (strategy == AI_Logic.Strategy.Approach)
			{
				radius_Length = 5.09f + (float)rand.Next(0, Mathf.Max ((int)radius.magnitude - 5,1)); //reduce the radius between the agent and threat
			}

			else if (strategy == AI_Logic.Strategy.StepBack)
			{
				radius_Length =  (float) rand.Next((int)(radius.magnitude + 5), (int) (radius.magnitude + 40));
			}
			delta_Position = delta_Position * radius_Length;
			ambushPoint = threat.position + delta_Position;	
		}

		return ambushPoint;
	}

	public void assignTargets()
	{
		/*Assigns targets to all the enemies in the scene
		 * */
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0;i < enemies.Length; i++)
		{

			if (enemies[i] != null)
			{
				getTarget(enemies[i].transform);
				enemies[i].GetComponent<AI_Logic>().mainState = AI_Logic.FiniteState.Chase;

			}
		}
		commandTime = time + delta_CommandTime;
	}


	public void Strategize()
	{
		/*Assigns strategies to agents.this includes move styles and shooting rage for difficulty purposes
		 * */
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int a = 0; a < enemies.Length; a++)
		{
			if (enemies[a] != null)
			{
				//rage
				enemies[a].GetComponent<EnemyCharacter>().rage = 14;
				//strategies
				int option = rand.Next (0, 10); 
				if (option < 5)
					enemies[a].GetComponent<EnemyCharacter>().strategy = AI_Logic.Strategy.Approach;
				else
					enemies[a].GetComponent<EnemyCharacter>().strategy = AI_Logic.Strategy.Sneak;
			}


		}

	}

	public void getTarget(Transform enemy)
	{
		/*Assigns a target to a specific enemy agent according to the aggro values of the threas(players) and the the relative position of the agent and the closest threat(player)
		 */

		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		float priority = 999.0f;
		int index = -1;

		for (int i = 0;i < players.Length; i++)
		{
			//searching for the most suitable threat.priority is directly proportional to the threat's aggro and inversely proportional to the distance between threat and agent
			Transform threat = players[i].transform;
			int aggro = threat.gameObject.GetComponent<CharacterBase>().getAggro();
			if (aggro !=0)//aggro == 0 is when the threat is in cloak mode,so the agent does not follow or attack it
			{
				float distance = Vector3.Distance(threat.position, enemy.position);
				float priority_temp = aggro/distance;
				if (priority_temp < priority)
				{
					priority = priority_temp; //update priority
					index = i;
				}
			}


		}

		if (index == -1) {  //no target was found
			enemy.GetComponent<AI_Logic>().threat = null;
		} 
		else 
		{
			enemy.GetComponent<AI_Logic>().threat = players[index].transform; //choose the most suitable player to be the threat
		}
		

	}
}

