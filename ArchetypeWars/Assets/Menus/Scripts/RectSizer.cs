using UnityEngine;
using System.Collections;

public class RectSizer : MonoBehaviour {

	RectTransform rect;
	// Use this for initialization
	void Start () {
	
		rect = GetComponent<RectTransform> ();
		rect.sizeDelta = new Vector2 ((Screen.width * .7f) / 4, Screen.height / 2);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
