using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Leaderboard : MonoBehaviour {

	public Text player1, player2, player3, player4, player5, player6, player7, player8;
	public Text player1Time, player2Time, player3Time, player4Time, player5Time, player6Time, player7Time, player8Time;
	public Text enterYourName;
	public Text field;
	public Button continueButton;

	string[] playerNames = new string[8];
	string[] highScores = new string[8];

	public ScoreContainer score;

	string tempScore;
	string tempName;

	bool updated = false;
	// Use this for initialization
	void Start () {

		string highScoreKey;
		string playerKey;


		for (int i = 0; i<highScores.Length; i++)
		{
			highScoreKey = "highScore" + i;
			playerKey = "player" + i;

			//PlayerPrefs.DeleteKey(highScoreKey);	//Remove this later
			//PlayerPrefs.DeleteKey (playerKey);

			playerNames[i] = PlayerPrefs.GetString(playerKey, "AAA");
			highScores[i] = PlayerPrefs.GetString(highScoreKey,"0:00:00 with 0 rounds");
		}

		player1.text = playerNames[0]; player1Time.text = highScores [0]; 
		player2.text = playerNames[1]; player2Time.text = highScores [1];
		player3.text = playerNames[2]; player3Time.text = highScores [2];
		player4.text = playerNames[3]; player4Time.text = highScores [3];
		player5.text = playerNames[4]; player5Time.text = highScores [4];
		player6.text = playerNames[5]; player6Time.text = highScores [5];
		player7.text = playerNames[6]; player7Time.text = highScores [6];
		player8.text = playerNames[7]; player8Time.text = highScores [7];

		if (GameObject.FindWithTag("Score")!=null)
			score = GameObject.FindWithTag("Score").GetComponent<ScoreContainer>();	//We don't need to make a tag and be efficient if nothing's really happening on this page
	}
	
	// Update is called once per frame
	void Update () {

		if (field!=null) 
		{
			if (field.text.Length < 3) {
				if (Input.GetKeyDown (KeyCode.A))
					field.text += "A";
				else if (Input.GetKeyDown (KeyCode.B))
					field.text += "B";
				else if (Input.GetKeyDown (KeyCode.C))
					field.text += "C";
				else if (Input.GetKeyDown (KeyCode.D))
					field.text += "D";
				else if (Input.GetKeyDown (KeyCode.E))
					field.text += "E";
				else if (Input.GetKeyDown (KeyCode.F))
					field.text += "F";
				else if (Input.GetKeyDown (KeyCode.G))
					field.text += "G";
				else if (Input.GetKeyDown (KeyCode.H))
					field.text += "H";
				else if (Input.GetKeyDown (KeyCode.I))
					field.text += "I";
				else if (Input.GetKeyDown (KeyCode.J))
					field.text += "J";
				else if (Input.GetKeyDown (KeyCode.K))
					field.text += "K";
				else if (Input.GetKeyDown (KeyCode.L))
					field.text += "L";
				else if (Input.GetKeyDown (KeyCode.M))
					field.text += "M";
				else if (Input.GetKeyDown (KeyCode.N))
					field.text += "N";
				else if (Input.GetKeyDown (KeyCode.O))
					field.text += "O";
				else if (Input.GetKeyDown (KeyCode.P))
					field.text += "P";
				else if (Input.GetKeyDown (KeyCode.Q))
					field.text += "Q";
				else if (Input.GetKeyDown (KeyCode.R))
					field.text += "R";
				else if (Input.GetKeyDown (KeyCode.S))
					field.text += "S";
				else if (Input.GetKeyDown (KeyCode.T))
					field.text += "T";
				else if (Input.GetKeyDown (KeyCode.U))
					field.text += "U";
				else if (Input.GetKeyDown (KeyCode.V))
					field.text += "V";
				else if (Input.GetKeyDown (KeyCode.W))
					field.text += "W";
				else if (Input.GetKeyDown (KeyCode.X))
					field.text += "X";
				else if (Input.GetKeyDown (KeyCode.Y))
					field.text += "Y";
				else if (Input.GetKeyDown (KeyCode.Z))
					field.text += "Z";
			}

			if (field.text.Length >= 3 && !updated)
			{
				updated = true;
				UpdateScores ();
				continueButton.Select();
			}
		}

	}

	public void UpdateScores() {

		if (score!=null)
		{
			string thisScore = string.Format("{0}:{1}:{2} with {3} rounds", (int)score.totalTime/60, ((int)(score.totalTime%60)).ToString("D2"), ((int)(score.totalTime*100%100)).ToString("D2"), score.rounds);
			//Debug.Log(thisScore);
			
			for (int i = 0; i<highScores.Length; i++)
			{
				string[] currScore = highScores[i].Split(' ');
				
				//Debug.Log ("Showing: " + playerNames[i]);
				//foreach (string s in currScore)
				//	Debug.Log(s);
				
				if (int.Parse (currScore[2]) < score.rounds)	//If the last game has more than our current score
				{					
					//Store the old player name
					tempScore = highScores[i];
					tempName = playerNames[i];
					//Set the new values
					highScores[i] = thisScore;
					playerNames[i] = field.text;

					switch(i) {
					case 0:
						player1.text = playerNames[i];
						player1Time.text = highScores[i];
						break;
					case 1:
						player2.text = playerNames[i];
						player2Time.text = highScores[i];
						break;
					case 2:
						player3.text = playerNames[i];
						player3Time.text = highScores[i];
						break;
					case 3:
						player4.text = playerNames[i];
						player4Time.text = highScores[i];
						break;
					case 4:
						player5.text = playerNames[i];
						player5Time.text = highScores[i];
						break;
					case 5:
						player6.text = playerNames[i];
						player6Time.text = highScores[i];
						break;
					case 6:
						player7.text = playerNames[i];
						player7Time.text = highScores[i];
						break;
					case 7:
						player8.text = playerNames[i];
						player8Time.text = highScores[i];
						break;
					}
					//Makes the old score the current score
					thisScore = tempScore;
					field.text = tempName;

				}
				
				else if (int.Parse (currScore[2]) == score.rounds)	//If the last game has the same number of rounds but a better time
				{
					string[] oldScorePieces = currScore[0].Split(':');
					float oldScoreTime = int.Parse (oldScorePieces[0])*60 + int.Parse (oldScorePieces[1]) + int.Parse (oldScorePieces[2])/100;	//Rebuild some stupid float of a score from the old one
					
					if (score.totalTime > oldScoreTime)
					{
						//Store the old player name
						tempScore = highScores[i];
						tempName = playerNames[i];
						//Set the new values
						highScores[i] = thisScore;
						playerNames[i] = field.text;

						switch(i) {
						case 0:
							player1.text = playerNames[i];
							player1Time.text = highScores[i];
							break;
						case 1:
							player2.text = playerNames[i];
							player2Time.text = highScores[i];
							break;
						case 2:
							player3.text = playerNames[i];
							player3Time.text = highScores[i];
							break;
						case 3:
							player4.text = playerNames[i];
							player4Time.text = highScores[i];
							break;
						case 4:
							player5.text = playerNames[i];
							player5Time.text = highScores[i];
							break;
						case 5:
							player6.text = playerNames[i];
							player6Time.text = highScores[i];
							break;
						case 6:
							player7.text = playerNames[i];
							player7Time.text = highScores[i];
							break;
						case 7:
							player8.text = playerNames[i];
							player8Time.text = highScores[i];
							break;
						}
						//Makes the old score the current score
						thisScore = tempScore;
						field.text = tempName;
					}
					
				}
			}
			
		}//End if statement

		enterYourName.gameObject.SetActive (false);
		field.gameObject.SetActive (false);
		continueButton.gameObject.SetActive (true);
		SaveScores ();
	}

	public void SaveScores() {
	
		for (int i = 0; i<highScores.Length; i++)	//Run through our high scores after the scores are updated, then save them
		{
			string highScoreKey = "highScore" + i;
			string playerKey = "player" + i;
			
			PlayerPrefs.SetString(playerKey,playerNames[i]);
			PlayerPrefs.SetString(highScoreKey,highScores[i]);
		}
	
	}

	public static void MainMenu() {
		Application.LoadLevel ("menu");
	}

	public void Credits() {
		if (score.gameObject!=null)
			Destroy (score.gameObject);
		Application.LoadLevel ("credits");
	}
}
