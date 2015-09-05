using UnityEngine;
using System.Collections;

public class SoundPool : MonoBehaviour {

	public AudioClip weaponLRSound, weaponSRSound, jumpSound, hitSound, special1Sound, special2Sound, special3Sound, deathSound, contextSound;
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

	public void playJumpSound()
	{
		audio.clip = jumpSound;
		audio.Play();
	}

	public void playHitSound()
	{
		audio.clip = hitSound;
		audio.Play();
	}

	public void playSpecial1Sound()
	{
		audio.clip = special1Sound;
		audio.Play();
	}

	public void playSpecial2Sound()
	{
		audio.clip = special2Sound;
		audio.Play();
	}

	public void playSpecial3Sound()
	{
		audio.clip = special3Sound;
		audio.Play();
	}

	public void playDeathSound()
	{
		audio.clip = deathSound;
		audio.Play();
	}

	public void playContextSound()	//This sound is for things like the commander's melee heal, or if another player has a specific interaction
	{
		audio.clip = contextSound;
		audio.Play();
	}

}
