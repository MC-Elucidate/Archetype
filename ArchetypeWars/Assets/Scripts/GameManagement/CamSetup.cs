using UnityEngine;
using System.Collections;

public class CamSetup : MonoBehaviour {


	public Camera[] cam = new Camera[4];

	public GameStartup charSelection;
	public Transform heavy;
	public Transform commander;
	public Transform ninja;
	public Transform sniper;
	public GameObject heavyLRweapon;
	public GameObject sniperLRweapon;
	public GameObject commanderLRweapon;
	public GameObject ninjaLRweapon;

	// Use this for initialization
	void Start () {
		charSelection = GameObject.Find ("CharacterSelectManager").GetComponent<GameStartup>();
		for (int i=1; i<=4; i++) {

			if(charSelection.playerChoices[i] == 0)
				break;
			Transform playerChar;
			GameObject LRWeapon;
			switch (charSelection.playerChoices [i]) {
			case 1:
				{
					playerChar = Instantiate (heavy, new Vector3 (0, 1, 0), Quaternion.identity) as Transform;
					PlayerCharacter heavyscript = playerChar.GetComponent<HeavyScript>();
					LRWeapon = (GameObject) Instantiate (heavyLRweapon, heavyscript.RightHand.position, Quaternion.identity);
					GunStats gunstats = LRWeapon.GetComponentInChildren<GunStats>();
					LRWeapon.transform.parent = heavyscript.RightHand; //attach the weapon to the right hand
				
				
					//initialising hand IK targets
					heavyscript.LRWeapon = LRWeapon;
					heavyscript.LHandPos = gunstats.LHandPos;
					heavyscript.RHandPos = gunstats.RHandPos;
					heavyscript.shot_source = gunstats.bulletSpawn;
					
					
				}
				break;
			case 2:
				{
					playerChar = Instantiate (commander, new Vector3 (4, 1, 4), Quaternion.identity) as Transform;
					PlayerCharacter commanderscript = playerChar.GetComponent<CommanderScript>();
					LRWeapon = (GameObject) Instantiate (commanderLRweapon, commanderscript.RightHand.position, Quaternion.identity);
					GunStats gunstats = LRWeapon.GetComponentInChildren<GunStats>();
					LRWeapon.transform.parent = commanderscript.RightHand; //attach the weapon to the right hand
				
				
					//initialising hand IK targets
					commanderscript.LRWeapon = LRWeapon;
					commanderscript.LHandPos = gunstats.LHandPos;
					commanderscript.RHandPos = gunstats.RHandPos;
					commanderscript.shot_source = gunstats.bulletSpawn;

				}	
				break;
			case 3:
				{	
					playerChar = Instantiate (ninja, new Vector3 (-4, 1, -4), Quaternion.identity) as Transform;
					
				}
				break;
			case 4:
				{
					playerChar = Instantiate (sniper, new Vector3 (0, 1, 5), Quaternion.identity) as Transform;
					PlayerCharacter sniperscript = playerChar.GetComponent<SniperScript>();
					LRWeapon = (GameObject) Instantiate (sniperLRweapon, sniperscript.RightHand.position, Quaternion.identity);
					GunStats gunstats = LRWeapon.GetComponentInChildren<GunStats>();
					LRWeapon.transform.parent = sniperscript.RightHand; //attach the weapon to the right hand
				
				
					//initialising hand IK targets
					sniperscript.LRWeapon = LRWeapon;
					sniperscript.LHandPos = gunstats.LHandPos;
					sniperscript.RHandPos = gunstats.RHandPos;
					sniperscript.shot_source = gunstats.bulletSpawn;
				}
				break;

			default: playerChar = null; break;
			}

			playerChar.gameObject.AddComponent ("Controller" + i);
			cam[i-1] = playerChar.gameObject.GetComponentInChildren<Camera> ();

			if (cam[0] && cam[1] && cam[2] && cam[3]) {
				//Debug.Log ("Doing 4 cameras");
				cam[0].rect = new Rect (0f, 0.5f, 0.5f, 0.5f);
				cam[1].rect = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
				cam[2].rect = new Rect (0f, 0f, 0.5f, 0.5f);
				cam[3].rect = new Rect (0.5f,0f,0.5f,0.5f);
			}
			
			else if (cam[0] && cam[1] && cam[2]) {
				//Debug.Log ("Doing 3 cameras");
				cam[0].rect = new Rect (0f, 0.5f, 1f, 0.5f);
				cam[1].rect = new Rect (0f, 0f, 0.5f, 0.5f);
				cam[2].rect = new Rect (0.5f, 0f, 0.5f, 0.5f);
			} 
			
			else if (cam[0] && cam[1]) {
				//Debug.Log ("Doing 2 cameras");
				cam[1].rect = new Rect (0f, 0f, 1f, 0.5f);
				cam[0].rect = new Rect (0f, 0.5f, 1f, 0.5f);
			} 
			
			else{}
		}
	}
	
	// Update is called once per frame
	void Update () {

		/*
		if (cam[0] && cam[1] && cam[2] && cam[3]) {
			//Debug.Log ("Doing 4 cameras");
			cam[0].rect = new Rect (0f, 0.5f, 0.5f, 0.5f);
			cam[1].rect = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
			cam[2].rect = new Rect (0f, 0f, 0.5f, 0.5f);
			cam[3].rect = new Rect (0.5f,0f,0.5f,0.5f);
		}

		else if (cam[0] && cam[1] && cam[2]) {
			//Debug.Log ("Doing 3 cameras");
			cam[0].rect = new Rect (0f, 0.5f, 1f, 0.5f);
			cam[1].rect = new Rect (0f, 0f, 0.5f, 0.5f);
			cam[2].rect = new Rect (0.5f, 0f, 0.5f, 0.5f);
		} 

		else if (cam[0] && cam[1]) {
			//Debug.Log ("Doing 2 cameras");
			cam[1].rect = new Rect (0f, 0f, 1f, 0.5f);
			cam[0].rect = new Rect (0f, 0.5f, 1f, 0.5f);
		} 

		else{}
		*/
	}
}
