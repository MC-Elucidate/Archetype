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
	/// 		NB (they can't be children of the character because they get downscaled)
	/// 
	/// 3) Each respective hud just needs to find the characterBase stats and update them on here.
	/// 
	/// </summary>

	public PlayerCharacter character;	//Find the corresponding character's health and ammo getters
	public Camera followcam;			//Set this object's canvas to use the correct camera
										//NB! If we want the sniper to have her HUD in scope, we need to change the followcam here too

	public Text healthVal;			//Update the health counter on updates
	public Text ammoVal;				//Update the ammo counter on updates

	public Text cooldownOneName;			
	public Text cooldownTwoName;
	public Text cooldownThreeName;			
	public Text cooldownOneVal;			
	public Text cooldownTwoVal;
	public Text cooldownThreeVal;			

	public Sprite heavy_portrait;
	public Sprite commander_portrait;
	public Sprite ninja_portrait;
	public Sprite sniper_portrait;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		healthVal.text = character.getHealth ().ToString ();
		ammoVal.text = character.getAmmo().ToString();
		if (character.getCooldownOne () <= 0)
			cooldownOneVal.text = "Ready!";
		else
			cooldownOneVal.text = Mathf.Round (character.getCooldownOne()).ToString ();

		if (character.getCooldownTwo () <= 0)
			cooldownTwoVal.text = "Ready!";
		else
			cooldownTwoVal.text = Mathf.Round (character.getCooldownTwo()).ToString();

		if (character.getCooldownThree () <= 0)
			cooldownThreeVal.text = "Ready!";
		else
			cooldownThreeVal.text = Mathf.Round (character.getCooldownThree()).ToString();
	}

	public void setCam()
	{
		gameObject.GetComponent<Canvas>().worldCamera = followcam;
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

	public void setCooldownName(int cooldown, string name) {
		switch (cooldown) {
		case 1:
			cooldownOneName.text = name;
			break;
		case 2:
			cooldownTwoName.text = name;
			break;
		case 3:
			cooldownThreeName.text = name;
			break;
		}
	}
}
