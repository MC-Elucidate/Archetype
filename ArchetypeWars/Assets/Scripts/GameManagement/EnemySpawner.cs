using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {


	//Enemy Prefabs
	public Transform lightEnemy;
	public Transform mediumEnemy;
	public Transform heavyEnemy;


	//Enemy Weapon Prefabs
	public Transform lightEnemyGun;
	public Transform mediumEnemyGun;
	public Transform heavyEnemyGun;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Transform spawnLightEnemy(Vector3 spawnPoint)
	{
		Transform enemy = Instantiate(lightEnemy, spawnPoint, Quaternion.identity) as Transform;
		EnemyCharacter script = enemy.GetComponent<EnemyLight>();
		GameObject LRWeapon = (GameObject) Instantiate (lightEnemyGun, script.RightHand.position, Quaternion.identity);
		GunStats gunstats = LRWeapon.GetComponentInChildren<GunStats>();
		LRWeapon.transform.parent = script.RightHand; //attach the weapon to the right hand
		RoundManager.enemies.Add(enemy);
		
		//initialising hand IK targets
		script.LRWeapon = LRWeapon;
		script.LHandPos = gunstats.LHandPos;
		script.RHandPos = gunstats.RHandPos;
		script.shot_source = gunstats.bulletSpawn;
		return enemy;
	}

	public Transform spawnMediumEnemy(Vector3 spawnPoint)
	{
		Transform enemy = Instantiate(mediumEnemy, spawnPoint, Quaternion.identity) as Transform;
		EnemyCharacter script = enemy.GetComponent<EnemyMedium>();
		Transform LRWeapon = Instantiate (mediumEnemyGun, script.RightHand.position, Quaternion.identity) as Transform;
		GunStats gunstats = LRWeapon.gameObject.GetComponentInChildren<GunStats>();
		LRWeapon.transform.parent = script.RightHand; //attach the weapon to the right hand
		RoundManager.enemies.Add(enemy);
		
		//initialising hand IK targets
		script.LRWeapon = LRWeapon.gameObject;
		script.LHandPos = gunstats.LHandPos;
		script.RHandPos = gunstats.RHandPos;
		script.shot_source = gunstats.bulletSpawn;
		return enemy;
	}

	public Transform spawnHeavyEnemy(Vector3 spawnPoint)
	{
		Transform enemy = Instantiate(heavyEnemy, spawnPoint, Quaternion.identity) as Transform;
		EnemyCharacter script = enemy.GetComponent<EnemyHeavy>();
		GameObject LRWeapon = (GameObject) Instantiate (heavyEnemyGun, script.RightHand.position, Quaternion.identity);
		GunStats gunstats = LRWeapon.GetComponentInChildren<GunStats>();
		LRWeapon.transform.parent = script.RightHand; //attach the weapon to the right hand
		RoundManager.enemies.Add(enemy);
		
		//initialising hand IK targets
		script.LRWeapon = LRWeapon;
		script.LHandPos = gunstats.LHandPos;
		script.RHandPos = gunstats.RHandPos;
		script.shot_source = gunstats.bulletSpawn;
		return enemy;
	}
}
