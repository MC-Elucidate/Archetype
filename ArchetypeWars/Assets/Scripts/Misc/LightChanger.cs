using UnityEngine;
using System.Collections;

public class LightChanger : MonoBehaviour {

	public Light light;

	public int targetColour = 1;
	public float targetBlend = 0f;

	public Color dawn = new Color(0.5f, 0.5f, 0.5f), 
				 noon = new Color(1.0f, 1.0f, 1.0f), 
				 dusk = new Color(0.5f, 0.5f, 0.5f), 
				 midnight = new Color(0.0f, 0.0f, 0.0f);

	public float timer = 0f, changeTime = 4*60, t = 0f, duration = 15f;
	public bool makeChange = false;

	// Use this for initialization
	void Start () {
		light.color = noon;
		timer = changeTime;
		targetColour = 1;
		RenderSettings.skybox.SetFloat ("_Blend", 0f);
	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;

		if (targetColour == 1 && makeChange)
		{
			RenderSettings.skybox.SetFloat ("_Blend", (Mathf.Lerp(RenderSettings.skybox.GetFloat("_Blend"), targetBlend, t/25)));	//0f
			light.color = Color.Lerp (dawn, noon, t); 
		}
		else if (targetColour == 2 && makeChange)
		{
			RenderSettings.skybox.SetFloat ("_Blend", (Mathf.Lerp(RenderSettings.skybox.GetFloat("_Blend"), targetBlend, t/25)));	//0f
			light.color = Color.Lerp (noon, dusk, t);
		}
		else if (targetColour == 3 && makeChange)
		{
			RenderSettings.skybox.SetFloat ("_Blend", (Mathf.Lerp(RenderSettings.skybox.GetFloat("_Blend"), targetBlend, t/25)));	//0f
			light.color = Color.Lerp (dusk, midnight, t);
		}
		else if (targetColour == 4 && makeChange)
		{
			RenderSettings.skybox.SetFloat ("_Blend", (Mathf.Lerp(RenderSettings.skybox.GetFloat("_Blend"), targetBlend, t/25)));	//0f
			light.color = Color.Lerp (midnight, dawn, t);
		}
		if (timer <= 0)
		{
			makeChange = true;
			if (targetColour <4)
			{
				targetColour++;

				if (targetColour==2)
					targetBlend = 0.5f;

				else if (targetColour==3)
					targetBlend = 1f;

				else if (targetColour==4)
					targetBlend = 0.5f;
			}
			else
			{
				targetColour = 1;
				targetBlend = 0f;
			}
			timer = changeTime;

		}

		if (t < 1 && makeChange) {
			t += Time.deltaTime/duration;
		}

		else if (t >= 1){
			makeChange = false;
			t = 0;
		}
	}
}
