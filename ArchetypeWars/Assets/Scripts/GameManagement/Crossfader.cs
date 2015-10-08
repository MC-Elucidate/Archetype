
using UnityEngine;
using System.Collections;

public class Crossfader : MonoBehaviour {

	public AudioClip lowkey;
	public AudioClip combat;
	
	private AudioSource lowkeyAudio;
	private AudioSource combatAudio;

	public float fadeFactor = 0.3f;
	public float targetVolume = 0.5f;
	
	void  Start (){
		lowkeyAudio = gameObject.AddComponent<AudioSource>();
		lowkeyAudio.clip = lowkey;
		lowkeyAudio.volume = 0.0f;
		combatAudio = gameObject.AddComponent<AudioSource>();
		combatAudio.clip = combat;
		combatAudio.volume = 0.0f;

		lowkeyAudio.Play ();
		lowkeyAudio.loop = true;
		combatAudio.Play ();
		combatAudio.loop = true;
	}
	
	void Update() {

		//Transitions BGM from combat music to down-time music.
		//Uses crossfade to transition between tracks.

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