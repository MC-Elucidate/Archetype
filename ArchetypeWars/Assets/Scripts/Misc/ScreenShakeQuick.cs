using UnityEngine;
using System.Collections;

public class ScreenShakeQuick : MonoBehaviour {

	public Camera cam;
	public float shake;
	public float shakeAmount;
	public float decreaseFactor;
	// Use this for initialization
	void Start () {
	
		shake = 0f;						//Shake time
		shakeAmount = 0.7f;				//How crazy it goes
		decreaseFactor = 1.0f;			//How fast relative to time it decreases
	}
	
	// Update is called once per frame
	void Update () {

		if (shake > 0) {
			Quaternion rotation = transform.localRotation;
			cam.transform.localPosition = (rotation * new Vector3(0, 35, -55)) + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;

		} else if (shake <= 0) {
			shake = 0.0f;
			Quaternion rotation = transform.localRotation;
			cam.transform.localPosition = rotation * new Vector3(0, 35, -55);
		}
	}

	public void Shake(float val) {

		shake = val;
		Debug.Log ("Sending shake!");
	}
}
