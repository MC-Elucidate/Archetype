using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour {

	//OTHER SCRIPTS
	EnemySpawner spawner;
	public static AITacticalUnit AITactics;

	//ROUND TYPES
	public enum Round	{Survival, CTF, PreRound};

	//ROUND AFFIXES - modifiers that affect each round
	public enum Affix	{	Player_MeleeOnly, Player_HalfHealth, Player_HalfAmmo, 
				 	Player_DoubleDamage, Player_FasterMovement, Player_ExplosiveShots,
					Enemy_HalfHealth, Enemy_DoubleSpawn, Enemy_FasterMovement, 
					Enemy_DoubleHealth, Enemy_ExplosiveShots	};

	public bool gameInProgress;
	public bool deadPlayers;
	public static Round currentRound;

	//collections
	public static List<Transform> players = new List<Transform>();
	public static List<Transform> enemies = new List<Transform>();

	//General round vars
	public bool alternateRound;			//alternateRound indicates that 3 affixes will be chosen for the round.
	public int alternateRoundChance = 10;
	public Affix affixOne;
	public Affix affixTwo;
	public Affix affixThree;

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
		AITactics = gameObject.GetComponent<AITacticalUnit>();
		KillRound ();
		gameInProgress = true;

	}
	
	// Update is called once per frame
	void Update () {

		//Round in progress
		if (roundTimer > 0 && !CheckVictory ()) {
			roundTimer -= Time.deltaTime;
			reinforcementTimer -= Time.deltaTime;


			if (reinforcementTimer <= 0 && currentRound == Round.Survival) {
				SpawnEnemies_Survival();
				AITactics.assignTargets();
				AITactics.Strategize();
				reinforcementTimer = reinforcementDelay;
			}

			else if (reinforcementTimer <= 0 && currentRound == Round.CTF) {
				SpawnEnemies_CTF();
				AITactics.assignTargets();
				AITactics.Strategize();
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

		foreach (Transform player in players) {
			deadPlayers = true;
			if (player.GetComponent<PlayerCharacter>().alive) {
				deadPlayers = false;
				break;
			}
			else
				continue;
		}

		if (deadPlayers)
			gameInProgress = false;

	}

	//========================================================================================
	//ROUND CONTROL METHODS
	//========================================================================================

	/*
	 * Starts a break round between survival rounds.
	 * */
	void KillRound(){
	
		currentRound = Round.PreRound;
		roundTimer = 10f;
		ResetConditions ();
		KillAllEnemies ();
		//Debug.Log ("Killing round. 10 seconds to new round");
	}

	/*
	 * Starts a new round. Currently on survival implemented.
	 * */
	void NewRound(){


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
			//Debug.Log ("New round initiated. Current round is Survival Mode. Get ready to die!");
			break;

		case 1:	//TODO: CTF
			break;
		}
	}

	/*
	 * Checks if the victory conditions for the round have been met
	 * */
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

	/*
	 * Respawns all player characters.
	 * */
	void RespawnPlayers() {
		//Use this to revive all players if at least one player survives a round
		//TODO: Implement this

		/*
		foreach (Transform player in players) {
			deadPlayers = true;
			if (!player.GetComponent<PlayerCharacter>().alive) {

			}
			else
				continue;
		}*/
		
	}

	/*
	 * Resets all conditions between rounds.
	 * Resets affixes and timers.
	 * */
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
		
	/*
	 * Add points to the total score.
	 * */
	public void AddScore(int points) {						//addScore for anything rewards, like round survival or kills
		score += points;
	}
	
	//========================================================================================
	//SURVIVAL MODE-SPECIFIC METHODS
	//========================================================================================


	/*
	 * Kill all enemies in the scene.
	 * Used at the end of a round.
	 * */
	void KillAllEnemies() {
		//Use this when the round ends to clear the battlefield.

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		foreach (GameObject enemy in enemies) {
		
			enemy.SendMessage("receiveDamage", 1000f);
		}
	}

	/*
	 * Spawns enemies for survival mode.
	 * */
	void SpawnEnemies_Survival() {

		int currentForceSize = GameObject.FindGameObjectsWithTag("Enemy").Length;
		
		reinforcementSize = Random.Range (3, 7);	//3 to 6 enemies spawned at a time

		if (currentForceSize + reinforcementSize > 15)
			reinforcementSize = 15 - currentForceSize;

		for (int i = 0; i<reinforcementSize; i++) {
			switch(Random.Range (0,3)) {
			case 0:
				spawner.spawnLightEnemy(spawnPoints[Random.Range(0,spawnPoints.Length)].position);	//Spawn light enemy
				break;
			case 1:
				spawner.spawnMediumEnemy(spawnPoints[Random.Range(0,spawnPoints.Length)].position);	//Spawn medium enemy
				break;
			case 2:
				spawner.spawnMediumEnemy(spawnPoints[Random.Range(0,spawnPoints.Length)].position); //Spawn heavy enemy
				break;
			}
		}

		//Debug.Log("Current enemies on the battlefield: " + GameObject.FindGameObjectsWithTag("Enemy").Length);
	}

	//========================================================================================
	//CTF MODE-SPECIFIC METHODS
	//========================================================================================

	//TODO: Add this shit

	//ResetFlag
	//CaptureConfirmFlag

	void SpawnEnemies_CTF() {
	
		//Debug.Log ("Later");
	}

	//========================================================================================



	//========================================================================================
	//ALTERNATE ROUND METHOD
	//========================================================================================

	/*
	 * Changes affixes for a new round.
	 * */
	void SetAlternateRound() {
		//This method will modify the affixes based on the Affixes drawn from the list

		//Debug.Log ("Alternate Round selected!");

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

		affixOne = affix_1;
		affixTwo = affix_2;
		affixThree = affix_3;
	}

	/*
	 * Makes changes to appropriate actors in the scene based on affixes selected.
	 * */
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

	/*
	 * Chooses random affix.
	 * */
	static T GetRandomEnum<T>()
	{
		System.Array A = System.Enum.GetValues(typeof(T));
		T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
		return V;
	}

	/*
	 * returns current round type
	 * */
	public Round getRound() {
		return currentRound;
	}

	/*
	 * returns current score
	 * */
	public int getScore() {
		return score;
	}

	/*
	 * Ends game. Returns to main menu
	 * */
	public void ExitGame() {
		Destroy (GameObject.Find ("CharacterSelectManager"));
		Screen.lockCursor = false;
		Screen.showCursor = true;
		Application.LoadLevel("menu");
	}
}
