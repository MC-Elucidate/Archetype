// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

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

		if (combatAudio.time >= 119.827f)
			combatAudio.time = 62.802f;

		if (lowkeyAudio.time >= 118.982f)
			lowkeyAudio.time = 2.944f;

	}
		
}