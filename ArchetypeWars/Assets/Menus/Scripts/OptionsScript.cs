using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsScript : MonoBehaviour {
	
	public GameObject[] controlScreens;

	// Use this for initialization
	void Start () {
		foreach (GameObject g in controlScreens)
		{
			g.SetActive(false);
		}
		controlScreens[0].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeScreen(int i) {

		foreach (GameObject g in controlScreens)
		{
			g.SetActive(false);
		}
		controlScreens[i].SetActive(true);
	}
}
