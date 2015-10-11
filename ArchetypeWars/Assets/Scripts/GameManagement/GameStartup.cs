using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * Keeps information of character from player select that can be loaded in the main gameplay scene.
 * */

public class GameStartup : MonoBehaviour {

	private int char1;
	private int char2;
	private int char3;
	private int char4;

	public Text heavyChoice;
	public Text commanderChoice;
	public Text ninjaChoice;
	public Text sniperChoice;

	public Text prompt;
	public float promptMaxTime = 3f;
	public float promptTimer = 0f;

	public int[] playerChoices;// = {0,0,0,0,0};

	private int playerChoosing;

	// Use this for initialization
	void Start () {

		char1 = -1;
		char2 = -1;
		char3 = -1;
		char4 = -1;

		playerChoices = new int[5];

		playerChoosing = 1;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Cancel")) {
			cancelLastCharacterSelect();
		}

		if (promptTimer > 0)
			promptTimer -= Time.deltaTime;

		if (promptTimer <= 0)
			prompt.text = "";
	}

	/*
 	* Starts game if at least one character is chosen.
 	* */
	public void startGame() {
		if (playerChoosing > 1) {
			DontDestroyOnLoad (this);
			Screen.lockCursor=true;
			Screen.showCursor=false;
			Application.LoadLevel ("ss 1");
		} else
		{
			Debug.Log ("No character selected");
			prompt.text = "No character is selected";
			promptTimer = promptMaxTime;
		}

	}

	/*
 	* Sets status of character to "selected". They cannot be selected again until deselected.
 	* */
	public void selectCharacter(int charNo) {
		//Select open character
		switch (charNo) {
		case 1:
			if(char1 == -1)
			{
				
				char1 = playerChoosing; //Set currently selecting player as that character
				playerChoices[playerChoosing] = charNo; //Set currently choosing player to the character selected
				heavyChoice.text = "Player " + playerChoosing;
				
				//Next player chooses
				playerChoosing++;

			}
			//Can't select character
			else{
				Debug.Log ("This player has already been chosen");
				prompt.text = "This player has already been chosen";
				promptTimer = promptMaxTime;
			}
			break;
			
		case 2:
			if(char2 == -1)
			{
				
				char2 = playerChoosing; //Set currently selecting player as that character
				playerChoices[playerChoosing] = charNo; //Set currently choosing player to the character selected
				commanderChoice.text = "Player " + playerChoosing;
				
				//Debug.Log ("Player " + playerChoosing + " = " + playerChoices[playerChoosing]);
				
				//Next player chooses
				playerChoosing++;
				
				//Debug.Log("Player 1 selected");
				//Debug.Log(playerChoosing);
			}
			else{
				Debug.Log ("This player has already been chosen");
				prompt.text = "This player has already been chosen";
				promptTimer = promptMaxTime;
			}
			break;
			
		case 3:
			if(char3 == -1)
			{
				
				char3 = playerChoosing; //Set currently selecting player as that character
				playerChoices[playerChoosing] = charNo; //Set currently choosing player to the character selected
				ninjaChoice.text = "Player " + playerChoosing;
				
				//Debug.Log ("Player " + playerChoosing + " = " + playerChoices[playerChoosing]);
				
				//Next player chooses
				playerChoosing++;
				
			}
			else{
				Debug.Log ("This player has already been chosen");
				prompt.text = "This player has already been chosen";
				promptTimer = promptMaxTime;
			}
			break;
			
		case 4:
			if(char4 == -1)
			{
				
				char4 = playerChoosing; //Set currently selecting player as that character
				playerChoices[playerChoosing] = charNo; //Set currently choosing player to the character selected
				sniperChoice.text = "Player " + playerChoosing;
				
				//Debug.Log ("Player " + playerChoosing + " = " + playerChoices[playerChoosing]);
				
				//Next player chooses
				playerChoosing++;
				
				//Debug.Log("Player 1 selected");
				//Debug.Log(playerChoosing);
			}
			else{
				Debug.Log ("This player has already been chosen");
				prompt.text = "This player has already been chosen";
				promptTimer = promptMaxTime;
			}
			break;
		
		}



	}

	/*
 	* Deselects last character selected.
 	* */
	public void cancelLastCharacterSelect() {

		//Use this function when the player presses Circle or some shit

		if (playerChoosing>1) {						//If at least player1 has chosen...

			switch (playerChoices[playerChoosing-1]) {	//Set the character they chose before as -1
				case 1:

				char1 = -1;
				if (heavyChoice!=null)
					heavyChoice.text = "";
				break;

				case 2:
				char2 = -1;
				if (commanderChoice!=null)
					commanderChoice.text = "";
				break;

				case 3:
				char3 = -1;
				if (ninjaChoice!=null)
					ninjaChoice.text = "";
				break;

				case 4:
				char4 = -1;
				if (sniperChoice!=null)
					sniperChoice.text = "";
				break;

			}
			playerChoices[playerChoosing-1] = -1;	//Remove the player's choice
			playerChoosing--;						//Decrement the player choosing and go back to their selection
		}
	}

	public void clearChoices() {

		//Cheap way to quickly empty the character select choices

		cancelLastCharacterSelect ();
		cancelLastCharacterSelect ();
		cancelLastCharacterSelect ();
		cancelLastCharacterSelect ();
	}
}
