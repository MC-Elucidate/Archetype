
using UnityEngine;
using System.Collections;

public class Crossfader : MonoBehaviour {

	public AudioClip lowkey;
	public AudioClip combat;
	
	private AudioSource lowkeyAudio;
	private AudioSource combatAudio;

	public float fadeFactor = 0.5f;
	public float targetVolume = 0.7f;
	
	void  Start (){
		lowkeyAudio = gameObject.AddComponent<AudioSource>();
		lowkeyAudio.clip = lowkey;
		lowkeyAudio.volume = 0.0f;
		combatAudio = gameObject.AddComponent<AudioSource>();
		combatAudio.clip = combat;
		combatAudio.volume = 0.0f;

		lowkeyAudio.Play ();
		lowkeyAudio.loop = true;
		//combatAudio.Play ();
		combatAudio.loop = true;
	}
	
	void Update() {

		//Transitions BGM from combat music to down-time music.
		//Uses crossfade to transition between tracks.
		if (combatAudio.isPlaying == false && lowkeyAudio.time > 8.0f)	//First time playing combat music, we want a delayed start
			combatAudio.Play ();

		if (RoundManager.currentRound == RoundManager.Round.Survival && combatAudio.volume < targetVolume)
		{
			combatAudio.volume += fadeFactor * Time.deltaTime;
			lowkeyAudio.volume -= fadeFactor * Time.deltaTime;

			if (lowkeyAudio.volume <= 0.05f)
				lowkeyAudio.volume = 0;
		}
		
		else if (RoundManager.currentRound == RoundManager.Round.PreRound && lowkeyAudio.volume < targetVolume)
		{
			combatAudio.volume -= fadeFactor * Time.deltaTime;
			lowkeyAudio.volume += fadeFactor * Time.deltaTime;

			if (combatAudio.volume < 0.05f)
				combatAudio.volume = 0;
		}

		if (combatAudio.time >= 124.666f)
			combatAudio.time = 22.597f;

		if (lowkeyAudio.time >= 80.110f)
			lowkeyAudio.time = 0.110f;

	}
		
}