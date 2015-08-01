using UnityEngine;
using System.Collections;

public class CharacterSelectVoices : MonoBehaviour {

	public AudioClip char1_sound;
	public AudioClip char2_sound;
	public AudioClip char3_sound;
	public AudioClip char4_sound;

	private AudioSource player;

	// Use this for initialization
	void Start () {

		player = GetComponent<AudioSource> ();
		player.Stop ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playCharacterSound(int character) {

		if (character == 1) {
			player.Stop ();
			player.clip = char1_sound;
			player.Play();
		}
		if (character == 2) {
			player.Stop ();
			player.clip = char2_sound;
			player.Play();
		}
		if (character == 3) {
			player.Stop ();
			player.clip = char3_sound;
			player.Play();
		}
		if (character == 4) {
			player.Stop ();
			player.clip = char4_sound;
			player.Play();
		}
	}
}
