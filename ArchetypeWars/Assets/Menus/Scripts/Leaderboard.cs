using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Leaderboard : MonoBehaviour {

	public Text player1, player2, player3, player4, player5, player6, player7, player8;
	public Text player1Time, player2Time, player3Time, player4Time, player5Time, player6Time, player7Time, player8Time;

	string[] playerNames = new string[8];
	string[] highScores = new string[8];
	// Use this for initialization
	void Start () {

		string highScoreKey;
		string playerKey;

		for (int i = 0; i<highScores.Length; i++)
		{
			highScoreKey = "highScore" + i;
			playerKey = "player" + i;

			playerNames[i] = PlayerPrefs.GetString(playerKey, "AAA");
			highScores[i] = PlayerPrefs.GetString(highScoreKey,"0:00:00 with 0 rounds");
		}
	}
	
	// Update is called once per frame
	void Update () {

		player1.text = playerNames[0]; player1Time.text = highScores [0]; 
		player2.text = playerNames[1]; player2Time.text = highScores [1];
		player3.text = playerNames[2]; player3Time.text = highScores [2];
		player4.text = playerNames[3]; player4Time.text = highScores [3];
		player5.text = playerNames[4]; player5Time.text = highScores [4];
		player6.text = playerNames[5]; player6Time.text = highScores [5];
		player7.text = playerNames[6]; player7Time.text = highScores [6];
		player8.text = playerNames[7]; player8Time.text = highScores [7];
	}

	public static void MainMenu() {
		Application.LoadLevel ("menu");
	}

	public static void Credits() {
		Application.LoadLevel ("credits");
	}
}
