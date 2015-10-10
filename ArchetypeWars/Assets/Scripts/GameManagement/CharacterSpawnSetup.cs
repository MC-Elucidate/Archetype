using UnityEngine;
using System.Collections;

public class CharacterSpawnSetup : MonoBehaviour {


	public Camera[] cam = new Camera[4];

	public GameStartup charSelection;

	//Player Character Prefabs
	public Transform heavy;
	public Transform commander;
	public Transform ninja;
	public Transform sniper;

	//Player Character Gun Prefabs
	public GameObject heavyLRweapon;
	public GameObject sniperLRweapon;
	public GameObject commanderLRweapon;
	public GameObject ninjaLRweapon;


	//gui
	public RectTransform gui;
	public RectTransform sniperscope;

	// Use this for initialization
	void Start () {

		//Spawns all characters that have been selected, as the players in the order they were selected.
		charSelection = GameObject.Find ("CharacterSelectManager").GetComponent<GameStartup>();
		for (int i=1; i<=4; i++) {

			if(charSelection.playerChoices[i] <= 0)
				break;

			Transform playerChar;
			GameObject LRWeapon;
			Transform playergui;
			playergui = Instantiate(gui, new Vector3(0,0,0), Quaternion.identity) as RectTransform;


			switch (charSelection.playerChoices [i]) {
			case 1: //Heavy
				{
					playerChar = Instantiate (heavy, GameObject.Find ("Player_HeavySpawnPoint").transform.position, Quaternion.identity) as Transform;
					PlayerCharacter heavyscript = playerChar.GetComponent<HeavyScript>();
					LRWeapon = (GameObject) Instantiate (heavyLRweapon, heavyscript.RightHand.position, Quaternion.identity);
					GunStats gunstats = LRWeapon.GetComponentInChildren<GunStats>();
					LRWeapon.transform.parent = heavyscript.RightHand; //attach the weapon to the right hand

					//initialising hand IK targets
					heavyscript.LRWeapon = LRWeapon;
					heavyscript.LHandPos = gunstats.LHandPos;
					heavyscript.RHandPos = gunstats.RHandPos;
					heavyscript.shot_source = gunstats.bulletSpawn;

					//Sets up GUI
					playergui.gameObject.GetComponent<PlayerHUD>().character = heavyscript;
					playergui.gameObject.GetComponent<PlayerHUD>().setPortraitHeavy();
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(1, "Shield Buff");
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(2, "Taunt");
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(3, "Ground Pound");
					
				}
				break;
			case 2: //Commander
				{
					playerChar = Instantiate (commander, GameObject.Find ("Player_CommanderSpawnPoint").transform.position, Quaternion.identity) as Transform;
					PlayerCharacter commanderscript = playerChar.GetComponent<CommanderScript>();
					LRWeapon = (GameObject) Instantiate (commanderLRweapon, commanderscript.RightHand.position, Quaternion.identity);
					GunStats gunstats = LRWeapon.GetComponentInChildren<GunStats>();
					LRWeapon.transform.parent = commanderscript.RightHand; //attach the weapon to the right hand
				
				
					//initialising hand IK targets
					commanderscript.LRWeapon = LRWeapon;
					commanderscript.LHandPos = gunstats.LHandPos;
					commanderscript.RHandPos = gunstats.RHandPos;
					commanderscript.shot_source = gunstats.bulletSpawn;

					//Sets up GUI
					playergui.gameObject.GetComponent<PlayerHUD>().character = commanderscript;
					playergui.gameObject.GetComponent<PlayerHUD>().setPortraitCommander();
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(1, "Team Buff");
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(2, "Team Heal");
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(3, "Rage Attack");

				}	
				break;
			case 3: //Ninja
				{	
					playerChar = Instantiate (ninja, GameObject.Find ("Player_NinjaSpawnPoint").transform.position, Quaternion.identity) as Transform;
					PlayerCharacter ninjascript = playerChar.GetComponent<NinjaScript>();
					
					// Sets up GUI
					playergui.gameObject.GetComponent<PlayerHUD>().character = ninjascript;
					playergui.gameObject.GetComponent<PlayerHUD>().setPortraitNinja();
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(1, "Dodge");
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(2, "Invisibility");
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(3, "Coup de Graçe");
					
				}
				break;
			case 4: //Sniper
				{
					playerChar = Instantiate (sniper, GameObject.Find ("Player_SniperSpawnPoint").transform.position, Quaternion.identity) as Transform;
					PlayerCharacter sniperscript = playerChar.GetComponent<SniperScript>();
					LRWeapon = (GameObject) Instantiate (sniperLRweapon, sniperscript.RightHand.position, Quaternion.identity);
					GunStats gunstats = LRWeapon.GetComponentInChildren<GunStats>();
					LRWeapon.transform.parent = sniperscript.RightHand; //attach the weapon to the right hand
				
				
					//initialising hand IK targets
					sniperscript.LRWeapon = LRWeapon;
					sniperscript.LHandPos = gunstats.LHandPos;
					sniperscript.RHandPos = gunstats.RHandPos;
					sniperscript.shot_source = gunstats.bulletSpawn;

					//Sets up GUI
					playergui.gameObject.GetComponent<PlayerHUD>().character = sniperscript;
					playergui.gameObject.GetComponent<PlayerHUD>().setPortraitSniper();
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(1, "Sniper Zoom");
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(2, "Evil Laugh");
					playergui.gameObject.GetComponent<PlayerHUD>().setCooldownName(3, "M3GA B34M");
					Transform scope = Instantiate(sniperscope, new Vector3(0,0,0), Quaternion.identity) as RectTransform;
					scope.GetComponent<Canvas>().worldCamera = GameObject.Find("Scopecam").camera;
				}
				break;

			default: playerChar = null; break;
			}

			//Adds controller to character for the appropriate player number
			playerChar.gameObject.AddComponent ("Controller" + i);
			cam[i-1] = playerChar.gameObject.GetComponentInChildren<Camera> ();


			playergui.GetComponent<Canvas>().worldCamera = cam[i-1];

			//Sets up split screen depending on how many players are playing.
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

	}
}
