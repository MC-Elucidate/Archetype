using UnityEngine;
using System.Collections;

public class SoundPool : MonoBehaviour {

	public AudioClip weaponLRSound, weaponSRSound, jumpSound, hitSound, special1Sound, special2Sound, special3Sound, deathSound, contextSound;
	public AudioSource voice;
	public AudioSource weapons;
	// Use this for initialization
	void Start () {

		//Uses to audio sources so that voices and weapon sounds can play at the same time.
		voice = GetComponents<AudioSource> () [0];
		weapons = GetComponents<AudioSource> () [1];

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	 * Plays the weapon fire sound
	 */
	public void pew()
	{
		weapons.clip = weaponLRSound;
		weapons.Play();
	}

	/*
	 * Plays the melee swing sound
	 */
	public void meleeSound()
	{
		weapons.clip = weaponSRSound;
		weapons.Play();
	}

	/*
	 * Plays the jump sound
	 */
	public void playJumpSound()
	{
		if (Random.Range (1, 101) > 50)	//50% chance to Jampu!
		{
			voice.clip = jumpSound;
			voice.Play();
		}
	}

	/*
	 * Plays the sound when the character takes damage
	 */
	public void playHitSound()
	{
		if (Random.Range (1, 101) > 70)	//30% chance to AITTA!!!
		{
			voice.clip = hitSound;
			voice.Play();
		}
	}

	/*
	 * Plays the sound for the first special ability
	 */
	public void playSpecial1Sound()
	{
		voice.clip = special1Sound;
		voice.Play();
	}

	/*
	 * Plays the sound for the second special ability
	 */
	public void playSpecial2Sound()
	{
		voice.clip = special2Sound;
		voice.Play();
	}

	/*
	 * Plays the sound for the third special ability (super)
	 */
	public void playSpecial3Sound()
	{
		voice.clip = special3Sound;
		voice.Play();
	}


	/*
	 * Plays the sound for when the character's health reaches 0
	 */
	public void playDeathSound()
	{
		voice.clip = deathSound;
		voice.Play();
	}

	/*
	 * An extra sound for special context situations. varies between characters.
	 */
	public void playContextSound()
	{
		voice.clip = contextSound;
		voice.Play();
	}

}
