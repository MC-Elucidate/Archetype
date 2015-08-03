using UnityEngine;
using System.Collections;

public class CamSetup : MonoBehaviour {

	public Camera cam1;
	public Camera cam2;
	public Camera cam3;
	public Camera cam4;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		if (cam1 && cam2 && cam3 && cam4) {
			//Debug.Log ("Doing 4 cameras");
			cam1.rect = new Rect (0f, 0.5f, 0.5f, 0.5f);
			cam2.rect = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
			cam3.rect = new Rect (0f, 0f, 0.5f, 0.5f);
			cam4.rect = new Rect (0.5f,0f,0.5f,0.5f);
		}

		else if (cam1 && cam2 && cam3) {
			//Debug.Log ("Doing 3 cameras");
			cam1.rect = new Rect (0f, 0.5f, 1f, 0.5f);
			cam2.rect = new Rect (0f, 0f, 0.5f, 0.5f);
			cam3.rect = new Rect (0.5f, 0f, 0.5f, 0.5f);
		} 

		else if (cam1 && cam2) {
			//Debug.Log ("Doing 2 cameras");
			cam2.rect = new Rect (0f, 0f, 1f, 0.5f);
			cam1.rect = new Rect (0f, 0.5f, 1f, 0.5f);
		} 

		else{}
	
	}
}
