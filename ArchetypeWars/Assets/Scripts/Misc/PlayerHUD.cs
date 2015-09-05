using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUD : MonoBehaviour {

	/// <summary>
	/// Use this class to go and find all these sub-variables.
	/// 
	/// 1) We'll make the text and picture on the canvas public and attach them here.
	/// 
	/// 2) Put one of these in the game for each character, the same way characters are spawned.
	/// 		NB (they can't be children of the chracter because they get downscaled)
	/// 
	/// 3) Each respective hud just needs to find the characterBase stats and update them on here.
	/// 
	/// </summary>

	public PlayerCharacter character;	//Find the corresponding character's health and ammo getters
	public Camera followcam;			//Set this object's canvas to use the correct camera
										//NB! If we want the sniper to have her HUD in scope, we need to change the followcam here too

	public Text healthVal;			//Update the health counter on updates
	public Text ammoVal;				//Update the ammo counter on updates
	public Sprite heavy_portrait;
	public Sprite commander_portrait;
	public Sprite ninja_portrait;
	public Sprite sniper_portrait;

	//public Texture2D portrait			//This one's for the character face

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		healthVal.text = character.getHealth ().ToString ();
		ammoVal.text = character.getAmmo().ToString();
	}

	public void setCam()
	{
		gameObject.GetComponentInChildren<Canvas>().worldCamera = followcam;
	}

	public void setPortraitHeavy() {
		this.gameObject.GetComponentInChildren<Image> ().overrideSprite = heavy_portrait;
	}

	public void setPortraitCommander() {
		this.gameObject.GetComponentInChildren<Image> ().overrideSprite = commander_portrait;
	}

	public void setPortraitNinja() {
		this.gameObject.GetComponentInChildren<Image> ().overrideSprite = ninja_portrait;
	}

	public void setPortraitSniper() {
		this.gameObject.GetComponentInChildren<Image> ().overrideSprite = sniper_portrait;
	}
}
