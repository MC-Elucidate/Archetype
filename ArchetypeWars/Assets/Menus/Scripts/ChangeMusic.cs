using UnityEngine;
using System.Collections;

public class ChangeMusic : MonoBehaviour {

	public AudioClip menuBGM;
	public AudioClip charSelectBGM;

	private AudioSource player;

	// Use this for initialization
	void Start () {

		player = GetComponent<AudioSource> ();
		player.clip = menuBGM;
		player.Play ();
		player.loop = true;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void switchSong() {

		if (player.clip == menuBGM) {
			player.Stop();
			player.clip = charSelectBGM;
			player.volume = 0.8f;
			player.Play ();
			player.loop = true;
		}

		else if (player.clip == charSelectBGM) {
			player.Stop();
			player.clip = menuBGM;
			player.volume = 1f;
			player.Play ();
			player.loop = true;
		}
	}

}
