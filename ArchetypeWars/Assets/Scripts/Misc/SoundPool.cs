using UnityEngine;
using System.Collections;

public class SoundPool : MonoBehaviour {

	public AudioClip weaponLRSound, weaponSRSound, jumpSound, hitSound, special1Sound, special2Sound, special3Sound, deathSound, contextSound;
	public AudioSource voice;
	public AudioSource weapons;
	// Use this for initialization
	void Start () {

		try {
		voice = GetComponents<AudioSource> () [0];
		weapons = GetComponents<AudioSource> () [1];
		}
		catch(System.IndexOutOfRangeException) {
			Debug.Log ("Someone still needs an audio source\nCheck the enemy prefabs");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void pew()
	{
		weapons.clip = weaponLRSound;
		weapons.Play();
	}

	public void meleeSound()
	{
		weapons.clip = weaponSRSound;
		weapons.Play();
	}

	public void playJumpSound()
	{
		voice.clip = jumpSound;
		voice.Play();
	}

	public void playHitSound()
	{
		voice.clip = hitSound;
		voice.Play();
	}

	public void playSpecial1Sound()
	{
		voice.clip = special1Sound;
		voice.Play();
	}

	public void playSpecial2Sound()
	{
		voice.clip = special2Sound;
		voice.Play();
	}

	public void playSpecial3Sound()
	{
		voice.clip = special3Sound;
		voice.Play();
	}

	public void playDeathSound()
	{
		voice.clip = deathSound;
		voice.Play();
	}

	public void playContextSound()	//This sound is for things like the commander's melee heal, or if another player has a specific interaction
	{
		voice.clip = contextSound;
		voice.Play();
	}

}
