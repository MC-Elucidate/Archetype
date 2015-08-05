using UnityEngine;
using System.Collections;

public class CamSetup : MonoBehaviour {

	public Camera cam1;
	public Camera cam2;
	public Camera cam3;
	public Camera cam4;

	public GameStartup charSelection;
	public Transform heavy;
	public Transform commander;
	public Transform ninja;
	public Transform sniper;

	// Use this for initialization
	void Start () {
		charSelection = GameObject.Find ("CharacterSelectManager").GetComponent<GameStartup>();
		switch (charSelection.playerChoices [1]) {
		case 1: {
			Transform playerChar = Instantiate(heavy, new Vector3(0, 1, 0), Quaternion.identity) as Transform;
			playerChar.gameObject.AddComponent("Controller1");
			cam1 = playerChar.gameObject.GetComponentInChildren<Camera>();
			GunController1 gc = cam1.gameObject.AddComponent("GunController1") as GunController1;
			gc.target = playerChar.gameObject;
		}
			break;
		case 2:{
			Transform playerChar = Instantiate(commander, new Vector3(0, 1, 0), Quaternion.identity) as Transform;
			playerChar.gameObject.AddComponent("Controller1");
			cam1 = playerChar.gameObject.GetComponentInChildren<Camera>();
			GunController1 gc = cam1.gameObject.AddComponent("GunController1") as GunController1;
			gc.target = playerChar.gameObject;
		}
			break;
		case 3:{
			Transform playerChar = Instantiate(ninja, new Vector3(0, 1, 0), Quaternion.identity) as Transform;
			playerChar.gameObject.AddComponent("Controller1");
			cam1 = playerChar.gameObject.GetComponentInChildren<Camera>();
			GunController1 gc = cam1.gameObject.AddComponent("GunController1") as GunController1;
			gc.target = playerChar.gameObject;
		}
			break;
		case 4:{
			Transform playerChar = Instantiate(sniper, new Vector3(0, 1, 0), Quaternion.identity) as Transform;
			playerChar.gameObject.AddComponent("Controller1");
			cam1 = playerChar.gameObject.GetComponentInChildren<Camera>();
			GunController1 gc = cam1.gameObject.AddComponent("GunController1") as GunController1;
			gc.target = playerChar.gameObject;
		}
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {


		if (cam1 && cam2 && cam3 && cam4) {
			//Debug.Log ("Doing 4 cameras");
			cam1.rect = new Rect (0f, 0.5f, 0.5f, 0.5f);
			cam2.rect = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
			cam3.rect = new Rect (0f, 0f, 0.5f, 0.5f);
			cam4.rect = new Rect (0.5f,0f,0.5f,0.5f);
		}

		else if (cam1 && cam2 && cam3) {
			//Debug.Log ("Doing 3 cameras");
			cam1.rect = new Rect (0f, 0.5f, 1f, 0.5f);
			cam2.rect = new Rect (0f, 0f, 0.5f, 0.5f);
			cam3.rect = new Rect (0.5f, 0f, 0.5f, 0.5f);
		} 

		else if (cam1 && cam2) {
			//Debug.Log ("Doing 2 cameras");
			cam2.rect = new Rect (0f, 0f, 1f, 0.5f);
			cam1.rect = new Rect (0f, 0.5f, 1f, 0.5f);
		} 

		else{}
	
	}
}
