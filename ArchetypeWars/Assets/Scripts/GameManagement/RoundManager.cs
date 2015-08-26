using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour {

	//OTHER SCRIPTS
	EnemySpawner spawner;

	//ROUND TYPES
	public enum Round	{Survival, CTF, PreRound};

	//ROUND AFFIXES - modifiers that affect each round
	//TODO: Add more over here once the basics are implemented. This can be pretty damn fun.
	enum Affix	{	Player_MeleeOnly, Player_HalfHealth, Player_HalfAmmo, 
				 	Player_DoubleDamage, Player_FasterMovement, Player_ExplosiveShots,
					Enemy_HalfHealth, Enemy_DoubleSpawn, Enemy_FasterMovement, 
					Enemy_DoubleHealth, Enemy_ExplosiveShots	};

	public static Round currentRound;

	//General round vars
	public bool alternateRound;			//alternateRound indicates that 3 affixes will be chosen for the round.
	public int alternateRoundChance = 10;
	public float roundTimer = 0f;
	public static int score = 0;

	public Transform[] spawnPoints;
	public float reinforcementDelay = 6f;
	public float reinforcementTimer = 0f;
	public int reinforcementSize;
	public int maxReinforcements = 15;

	//SURVIVAL VARS
	//========================================================================================
	public static int enemyCount = 10;			//Keep spawning enemies. On enemy kill, decrement this, and kill all enemies when the count is zero for MAX FUN!
	//Max enemies
	//========================================================================================
	//CTF VARS
	//========================================================================================
	//Transform[] FlagSpawns
	//Transform PlayerFlag
	//Transform EnemyFlag
	//Capture number
	//Capture limit
	//Max defenders, max attackers		//Can't flood the game with enemies, but keep spawning waves at some point
	//========================================================================================


	//========================================================================================
	//ROUND AFFIXES - Editing these variables should 
	//========================================================================================
	//Player Affixes
	public bool  player_meleeOnly = false;
	public float player_healthMultiplier = 1.00f;
	public float player_ammoMultiplier = 1.00f;
	public float player_damageMultiplier = 1.00f;
	public float player_movementMultiplier = 1.00f;
	public bool  player_explosiveShots = false;

	//Enemy Affixes
	public float enemy_healthMultiplier = 1.00f;
	public bool  enemy_doubleSpawns = false;
	public float enemy_movementMultiplier = 1.00f;
	public bool  enemy_explosiveShots = false;
	//========================================================================================

	// Use this for initialization
	void Start () {

		spawner = gameObject.GetComponent<EnemySpawner>();
		KillRound ();

	}
	
	// Update is called once per frame
	void Update () {

		//Round in progress
		if (roundTimer > 0 && !CheckVictory ()) {
			roundTimer -= Time.deltaTime;
			reinforcementTimer -= Time.deltaTime;

			//Which spawn logic do we want?

			if (reinforcementTimer <= 0 && currentRound == Round.Survival) {
				SpawnEnemies_Survival();
				reinforcementTimer = reinforcementDelay;
			}

			else if (reinforcementTimer <= 0 && currentRound == Round.CTF) {
				SpawnEnemies_CTF();
				reinforcementTimer = reinforcementDelay;
			}
		}

		//Pre-round Timer up, start new round
		else if (roundTimer <= 0 && currentRound == Round.PreRound)
			NewRound ();

		//Round ends
		else if ((currentRound!=Round.PreRound && roundTimer <= 0) || CheckVictory()) {
			KillRound ();
			RespawnPlayers();
		}
	}

	//========================================================================================
	//ROUND CONTROL METHODS
	//========================================================================================

	void KillRound(){
	
		currentRound = Round.PreRound;
		roundTimer = 10f;
		ResetConditions ();
		KillAllEnemies ();
		Debug.Log ("Killing round. 10 seconds to new round");
	}

	void NewRound(){

		//Spawns one enemy for testing purposes
		//spawner.spawnMediumEnemy (new Vector3(10, 1, 10f));

		reinforcementTimer = reinforcementDelay;	//Making sure this variable starts the same for

		if (Random.Range (0, 101) > (100 - alternateRoundChance))
			alternateRound = true;				//Use this variable for score later on

		if (alternateRound)
			SetAlternateRound ();

		switch (Random.Range (0, 0)) {			//Change this for when we have more rounds, for now it should always return zero

		case 0:	//SURVIVAL
			currentRound = Round.Survival;
			roundTimer = 120f;
			enemyCount = 10;
			reinforcementTimer = 0;
			Debug.Log ("New round initiated. Current round is Survival Mode. Get ready to die!");
			break;

		case 1:	//TODO: CTF
			break;
		}
	}

	bool CheckVictory() {

		if (currentRound == Round.Survival) {
			if (enemyCount <= 0)
				return true;
		} 

		//TODO: Implement this once CTF is working
		else if (currentRound == Round.CTF) {
			//Do stuff
			//return true;
		}

		return false;
	}

	void RespawnPlayers() {
		//Use this to revive all players if at least one player survives a round
		//TODO: Implement this shit
		
	}

	void ResetConditions() {

		//Use this to set all the round conditions to 0 in the pre-round
		alternateRound = false;

		//SURVIVAL
		enemyCount = 0;
		reinforcementTimer = 0;

		//TODO: CTF

		//Player Affixes
		player_meleeOnly = false;
		player_healthMultiplier = 1.00f;
		player_ammoMultiplier = 1.00f;
		player_damageMultiplier = 1.00f;
		player_movementMultiplier = 1.00f;
		player_explosiveShots = false;
		
		//Enemy Affixes
		enemy_healthMultiplier = 1.00f;
		enemy_doubleSpawns = false;
		enemy_movementMultiplier = 1.00f;
		enemy_explosiveShots = false;
	}
		
	public void AddScore(int points) {						//addScore for anything rewards, like round survival or kills
		score += points;
	}
	
	//========================================================================================
	//SURVIVAL MODE-SPECIFIC METHODS
	//========================================================================================

	/* //DEPRECATED
	public void KillConfirmEnemy() {
		//Use this method to decrement the enemyCount in Survival Mode
		enemyCount--;
	}*/

	void KillAllEnemies() {
		//TODO: Use this when the round ends to clear the battlefield.

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		foreach (GameObject enemy in enemies) {
		
			enemy.SendMessage("receiveDamage", 1000f);
		}
	}

	void SpawnEnemies_Survival() {

		//Debug.Log ("Spawning enemies");
		int currentForceSize = GameObject.FindGameObjectsWithTag("Enemy").Length;
		
		reinforcementSize = Random.Range (3, 7);	//3 to 6 enemies spawned at a time

		if (currentForceSize + reinforcementSize > 15)
			reinforcementSize = 15 - currentForceSize;

		for (int i = 0; i<reinforcementSize; i++) {
			switch(Random.Range (0,3)) {
			case 0:
				spawner.spawnMediumEnemy(spawnPoints[Random.Range(0,spawnPoints.Length)].position);	//Spawn light enemy
				break;
			case 1:
				spawner.spawnMediumEnemy(spawnPoints[Random.Range(0,spawnPoints.Length)].position);	//Spawn medium enemy
				break;
			case 2:
				spawner.spawnMediumEnemy(spawnPoints[Random.Range(0,spawnPoints.Length)].position); //Spawn heavy enemy
				break;
			}
		}

		Debug.Log("Current enemies on the battlefield: " + GameObject.FindGameObjectsWithTag("Enemy").Length);
	}

	//========================================================================================
	//CTF MODE-SPECIFIC METHODS
	//========================================================================================

	//TODO: Add this shit

	//ResetFlag
	//CaptureConfirmFlag

	void SpawnEnemies_CTF() {
	
		Debug.Log ("Later");
	}

	//========================================================================================



	//========================================================================================
	//ALTERNATE ROUND METHOD
	//========================================================================================

	void SetAlternateRound() {
		//This method will modify the affixes based on the Affixes drawn from the list

		Debug.Log ("Alternate Round selected!");

		//AFFIX 1
		Affix affix_1 = GetRandomEnum<Affix>();

		//AFFIX 2
		Affix affix_2 = GetRandomEnum<Affix>();

		if (affix_2 == affix_1) {
			while (affix_2 == affix_1)
				affix_2 = GetRandomEnum<Affix>();
		}

		//AFFIX 3
		Affix affix_3 = GetRandomEnum<Affix>();

		if (affix_3 == affix_1 || affix_3 == affix_2) {
			while (affix_3 == affix_1 || affix_3 == affix_2)
				affix_3 = GetRandomEnum<Affix>();
		}

		Debug.Log ("Affixes:");

		ModifyAffix (affix_1);
		ModifyAffix (affix_2);
		ModifyAffix (affix_3);
	}

	void ModifyAffix(Affix thisAffix) {

		//Modify the correct multiplier using this method
		switch (thisAffix) {
		case Affix.Player_MeleeOnly:
			player_meleeOnly = true;
			Debug.Log("Players have melee only.");
			break;

		case Affix.Player_HalfHealth:
			player_healthMultiplier = 0.5f;
			Debug.Log("Players have 50% life.");
			break;

		case Affix.Player_HalfAmmo:
			player_ammoMultiplier = 0.5f;
			Debug.Log("Players have 50% ammo.");
			break;

		case Affix.Player_DoubleDamage:
			player_damageMultiplier = 2.00f;
			Debug.Log("Players have 200% damage.");
			break;

		case Affix.Player_FasterMovement:
			player_movementMultiplier = 1.50f;
			Debug.Log("Players move 50% faster.");
			break;

		case Affix.Player_ExplosiveShots:
			player_explosiveShots = true;
			Debug.Log("Players' shots explode!!!");
			break;

		case Affix.Enemy_HalfHealth:
			enemy_healthMultiplier = 0.5f;
			Debug.Log("Enemies have 50% life.");
			break;

		case Affix.Enemy_DoubleSpawn:
			enemy_doubleSpawns = true;
			Debug.Log("Enemies spawn at double the rate.");
			break;

		case Affix.Enemy_FasterMovement:
			enemy_movementMultiplier = 1.5f;
			Debug.Log("Enemies move 50% faster");
			break;

		case Affix.Enemy_DoubleHealth:
			enemy_healthMultiplier = 2.00f;
			Debug.Log("Enemies have 200% life.");
			break;

		case Affix.Enemy_ExplosiveShots:
			enemy_explosiveShots = true;
			Debug.Log("Enemies' shots explode!");
			break;
		}
	}

	static T GetRandomEnum<T>()
	{
		System.Array A = System.Enum.GetValues(typeof(T));
		T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
		return V;
	}
}
