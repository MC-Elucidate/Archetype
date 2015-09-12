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

		}

		time += Time.deltaTime;
	}

	public Vector3 LookForAmbushPoint(Transform agent, Transform threat)
	{

		Vector3 dP = Quaternion.AngleAxis(rand.Next(1,360), Vector3.up) * (threat.position - agent.position).normalized;
		dP = dP * 7.0f;
		Vector3 ambushPoint = agent.position + dP;
		return ambushPoint;
	}

	public void assignTargets()
	{
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

	public void getTarget(Transform enemy)
	{
		float minDistance = 999.0f;
		int index = 0;

		for (int i = 0;i < RoundManager.players.Count; i++)
		{
			Transform threat = RoundManager.players[i];
			float distance = Vector3.Distance(threat.position, enemy.position);
			if (threat == null)
				print("null threat");
			if (enemy == null)
				print("enemy threat");

			if (distance < minDistance)
			{
				minDistance = distance; //update minimum distance
				index = i;
			}

		}


		
		enemy.GetComponent<AI_Logic>().threat = RoundManager.players[index]; //choose the closest player to be the threat
	}
}

