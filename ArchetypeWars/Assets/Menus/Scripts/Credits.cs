using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Credits : MonoBehaviour {

	public GameObject thanks1, thanks2, thanks3;
	public Button mainMenu;
	public float timer = 0.0f, maxTime = 5f;
	// Use this for initialization
	void Start () {
		thanks1.SetActive (true);
		thanks2.SetActive (false);
		thanks3.SetActive (false);
		timer = maxTime;

		mainMenu.Select ();

	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0 || Input.GetMouseButtonDown(0)) {
			if (thanks1.activeInHierarchy)
			{
				thanks1.SetActive (false);
				thanks2.SetActive (true);
				thanks3.SetActive (false);
			}

			else if (thanks2.activeInHierarchy)
			{
				thanks1.SetActive (false);
				thanks2.SetActive (false);
				thanks3.SetActive (true);
			}

			else if (thanks3.activeInHierarchy)
			{
				thanks1.SetActive (true);
				thanks2.SetActive (false);
				thanks3.SetActive (false);
			}

			timer = maxTime;	
		}
	
	}
}
