using UnityEngine;
using System.Collections;

public class CamSetup_M : MonoBehaviour {


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
				/*Rigidbody [] rbodies =  playerChar.gameObject.GetComponentsInChildren<Rigidbody>();
				Transform rHand = playerChar.transform;

				for (int a = 0; a < rbodies.Length; a++)
				{
					if (rbodies[a].gameObject.tag == "RightHand") //locating right hand position
					{
						rHand = rbodies[a].gameObject.transform;
						break;

					}
				}*/

				PlayerCharacter heavyscript = playerChar.GetComponent<HeavyScript>();
				LRWeapon = (GameObject) Instantiate (heavyLRweapon, heavyscript.RightHand.position, Quaternion.identity);
				GunStats gunstats = LRWeapon.GetComponent<GunStats>();
				LRWeapon.transform.parent = heavyscript.RightHand; //attach the weapon to the right hand

				
				//initialising hand IK targets
				heavyscript.LRWeapon = LRWeapon;
				heavyscript.LHandPos = gunstats.LHandPos;
				heavyscript.RHandPos = gunstats.RHandPos;
				heavyscript.shot_source = gunstats.bulletSpawn;
				/*Transform [] weaponParts = LRWeapon.GetComponentsInChildren<Transform>();
				print (weaponParts.Length);
				for (int a = 0; a < weaponParts.Length; a++)
				{
					print ("looping");
					if (weaponParts[a].gameObject.tag == "RHandHandle")
					{
						playerChar.gameObject.GetComponent<HeavyScript>().RHandPos = weaponParts[a];
						print ("found");		

					}
					else if (weaponParts[a].gameObject.tag == "LHandHandle")
					{
						playerChar.gameObject.GetComponent<HeavyScript>().LHandPos = weaponParts[a];
					}

					else if (weaponParts[a].gameObject.tag == "FirePoint")
					{
						playerChar.gameObject.GetComponent<CharacterBase>().shot_source = weaponParts[a];
					}

				}*/

				//heavyscript.LRWeapon.SetActive(false);
				//playerChar.gameObject.AddComponent ("Controller" + i);
					//cam[i-1] = playerChar.gameObject.GetComponentInChildren<Camera> ();
					//GunCamera gc = cam[i-1].gameObject.AddComponent ("GunController" + i) as GunCamera;
					//gc.target = playerChar.gameObject;
				}
				break;
			case 2:
				{
					playerChar = Instantiate (commander, new Vector3 (4, 1, 4), Quaternion.identity) as Transform;
				Rigidbody [] rbodies =  playerChar.gameObject.GetComponentsInChildren<Rigidbody>();
				Transform rHand = playerChar.transform;
				
				for (int a = 0; a < rbodies.Length; a++)
				{
					if (rbodies[a].gameObject.tag == "RightHand") //locating right hand position
					{
						rHand = rbodies[a].gameObject.transform;
						break;
						
					}
				}
				
				LRWeapon = (GameObject) Instantiate (commanderLRweapon, rHand.position, Quaternion.identity);
				LRWeapon.transform.parent = rHand; //attach the weapon to the right hand
				
				//initialising hand IK targets
				playerChar.gameObject.GetComponent<CommanderScript>().LRWeapon = LRWeapon;
				Transform [] weaponParts = playerChar.gameObject.GetComponent<CommanderScript>().LRWeapon.GetComponentsInChildren<Transform>();
				for (int a = 0; a < weaponParts.Length; a++)
				{
					if (weaponParts[a].gameObject.tag == "RHandHandle")
					{
						playerChar.gameObject.GetComponent<CommanderScript>().RHandPos = weaponParts[a];
						
						
					}
					else if (weaponParts[a].gameObject.tag == "LHandHandle")
					{
						playerChar.gameObject.GetComponent<CommanderScript>().LHandPos = weaponParts[a];
					}
					
					else if (weaponParts[a].gameObject.tag == "FirePoint")
					{
						playerChar.gameObject.GetComponent<CharacterBase>().shot_source = weaponParts[a];
					}
					
				}


				//playerChar.gameObject.AddComponent ("Controller" + i);
					//cam[i-1] = playerChar.gameObject.GetComponentInChildren<Camera> ();
					//GunCamera gc = cam[i-1].gameObject.AddComponent ("GunController" + i) as GunCamera;
					//gc.target = playerChar.gameObject;
				}	
				break;
			case 3:
				{	
					playerChar = Instantiate (ninja, new Vector3 (-4, 1, -4), Quaternion.identity) as Transform;
				Rigidbody [] rbodies =  playerChar.gameObject.GetComponentsInChildren<Rigidbody>();
				Transform rHand = playerChar.transform;
				
				for (int a = 0; a < rbodies.Length; a++)
				{
					if (rbodies[a].gameObject.tag == "RightHand") //locating right hand position
					{
						rHand = rbodies[a].gameObject.transform;
						break;
						
					}
				}
				
				LRWeapon = (GameObject) Instantiate (ninjaLRweapon, rHand.position, Quaternion.identity);
				LRWeapon.transform.parent = rHand; //attach the weapon to the right hand

				//initialising hand IK targets
				playerChar.gameObject.GetComponent<NinjaScript>().LRWeapon = LRWeapon;
				playerChar.gameObject.GetComponent<NinjaScript>().RHandPos = LRWeapon.transform;
				playerChar.gameObject.GetComponent<NinjaScript>().LHandPos = LRWeapon.transform;



					//playerChar.gameObject.AddComponent ("Controller" + i);
					//cam[i-1] = playerChar.gameObject.GetComponentInChildren<Camera> ();
					//GunCamera gc = cam[i-1].gameObject.AddComponent ("GunController" + i) as GunCamera;
					//gc.target = playerChar.gameObject;
				}
				break;
			case 4:
				{
					playerChar = Instantiate (sniper, new Vector3 (0, 1, 5), Quaternion.identity) as Transform;
				Rigidbody [] rbodies =  playerChar.gameObject.GetComponentsInChildren<Rigidbody>();
				Transform rHand = playerChar.transform;
				
				for (int a = 0; a < rbodies.Length; a++)
				{
					if (rbodies[a].gameObject.tag == "RightHand") //locating right hand position
					{
						rHand = rbodies[a].gameObject.transform;
						break;
						
					}
				}
				
				LRWeapon = (GameObject) Instantiate (sniperLRweapon, rHand.position, Quaternion.identity);
				LRWeapon.transform.parent = rHand; //attach the weapon to the right hand
				
				//initialising hand IK targets
				playerChar.gameObject.GetComponent<SniperScript>().LRWeapon = LRWeapon;
				Transform [] weaponParts = playerChar.gameObject.GetComponent<SniperScript>().LRWeapon.GetComponentsInChildren<Transform>();

				for (int a = 0; a < weaponParts.Length; a++)
				{
					if (weaponParts[a].gameObject.tag == "RHandHandle")
					{
						playerChar.gameObject.GetComponent<SniperScript>().RHandPos = weaponParts[a];
						
						
					}
					else if (weaponParts[a].gameObject.tag == "LHandHandle")
					{
						playerChar.gameObject.GetComponent<SniperScript>().LHandPos = weaponParts[a];
					}
					
					else if (weaponParts[a].gameObject.tag == "FirePoint")
					{
						playerChar.gameObject.GetComponent<CharacterBase>().shot_source = weaponParts[a];
					}
					
				}


				//playerChar.gameObject.AddComponent ("Controller" + i);
					//cam[i-1] = playerChar.gameObject.GetComponentInChildren<Camera> ();
					//GunCamera gc = cam[i-1].gameObject.AddComponent ("GunController" + i) as GunCamera;
					//gc.target = playerChar.gameObject;
				}
				break;

			default: playerChar = null; break;
			}

			heavyLRweapon.SetActive(false);
			ninjaLRweapon.SetActive(false);
			commanderLRweapon.SetActive(false);
			sniperLRweapon.SetActive(false);

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
