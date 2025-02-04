﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/*
 * Sets values on GUI to round information. Used for entire screen. Not just a single character.
 * */

public class GameOverlay : MonoBehaviour {
	
	public Transform gameManager;
	public RoundManager rounds;

	public Text roundType;			
	public Text roundTimer;
	public Text roundCounter;
	public Text totalTimer;
	public Text alternateRound;
	public Text affix1;			
	public Text affix2;
	public Text affix3;	

	public Text gameOverText;
	public Text gameOverSubtext;

	public Text pauseText;
	public Text pauseSubtext;

	public bool overlayPause = false;

	public float endTimer = 10f;

	// Use this for initialization
	void Start () {
		rounds = gameManager.GetComponentInChildren<RoundManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateOverlay ();

	}

	public void UpdateOverlay() {

		//Set round type and timer.
		roundType.text = rounds.getRound().ToString();
		roundTimer.text = System.String.Format("{0}:{1}", (int)rounds.roundTimer/60, ((int)rounds.roundTimer%60).ToString("D2"));
		roundCounter.text = "Round " + RoundManager.roundCounter;
		totalTimer.text = System.String.Format ("{0}:{1}", (int)rounds.totalTime/60, ((int)rounds.totalTime%60).ToString("D2"));

		if (rounds.alternateRound) {
			alternateRound.text = "ALTERNATE ROUND ACTIVE";
			affix1.text = rounds.affixOne.ToString();
			affix2.text = rounds.affixTwo.ToString();
			affix3.text = rounds.affixThree.ToString();
		}
		else {
			alternateRound.text = "";
			affix1.text = "";
			affix2.text = "";
			affix3.text = "";
		}

		if(rounds.paused && Input.GetKeyDown(KeyCode.B))
		{
			rounds.ExitGame();
		}

		if (rounds.paused && rounds.gameInProgress) {
				
			overlayPause = true;
			pauseText.gameObject.SetActive(true);
			pauseSubtext.gameObject.SetActive(true);
		}

		if (!rounds.paused && overlayPause) {
			overlayPause = false;
			pauseText.gameObject.SetActive(false);
			pauseSubtext.gameObject.SetActive(false);
		}

		if (!rounds.gameInProgress) {
			gameOverText.gameObject.SetActive(true);
			gameOverSubtext.gameObject.SetActive(true);

			GameObject[] uiPieces = GameObject.FindGameObjectsWithTag("HUD");
			foreach (GameObject ui in uiPieces)
			{
				ui.SetActive(false);
			}

			if (endTimer>0) {
				endTimer-=Time.deltaTime;
				gameOverSubtext.text = System.String.Format ("This is the endgame!\nYou held out for as long as you could!\nHold on\n{0}...", (int)endTimer);
			}
		}

		if (endTimer <= 0) {
			gameOverSubtext.text = "Press any key to return to the main menu";
			if (Input.anyKeyDown)
				rounds.ExitGame ();

		}
	}
}
