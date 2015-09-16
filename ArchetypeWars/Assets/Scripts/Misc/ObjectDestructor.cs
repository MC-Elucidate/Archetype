using UnityEngine;
using System.Collections;

public class ObjectDestructor : MonoBehaviour {

	//This class is just to throw onto effects so that you can destroy them after a time
	//e.g explosion particle effects, or bullet holes

	public float timer;
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, timer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
