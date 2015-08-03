using UnityEngine;
using System.Collections;

public class GameStartup : MonoBehaviour {

	public int numberOfPlayers;

	private bool char1_selected;
	private bool char2_selected;
	private bool char3_selected;
	private bool char4_selected;

	// Use this for initialization
	void Start () {

		char1_selected = false;
		char2_selected = false;
		char3_selected = false;
		char4_selected = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startGame() {

		DontDestroyOnLoad (this);
		Application.LoadLevel ("ss");

	}

	public void selectCharacter(int charNo) {
		if (charNo == 1) {
			char1_selected = !char1_selected;
		}

		else  if (charNo == 2) {
			char2_selected = !char2_selected;
		}

		else  if (charNo == 3) {
			char3_selected = !char3_selected;
		}

		else  if (charNo == 4) {
			char4_selected = !char4_selected;
		}
	}
}
