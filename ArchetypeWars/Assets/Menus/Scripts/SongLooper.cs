using UnityEngine;
using System.Collections;

public class SongLooper : MonoBehaviour {

	public AudioSource audio;
	public int caseButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (caseButton == 1 && audio.time > 60.869f)		//Case 1 is for the leaderboard
			audio.time = 3.429f;

		else if (caseButton == 2 && !audio.isPlaying)	//Case 2 is for the credits screen
			Leaderboard.MainMenu();
	}
}
