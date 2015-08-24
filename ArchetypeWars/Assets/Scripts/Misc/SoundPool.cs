using UnityEngine;
using System.Collections;

public class SoundPool : MonoBehaviour {

	public AudioClip weaponLRSound, weaponSRSound, jumpSound, hitSound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void pew()
	{
		audio.clip = weaponLRSound;
		audio.Play();
	}

	public void meleeSound()
	{
		audio.clip = weaponSRSound;
		audio.Play();
	}

}
