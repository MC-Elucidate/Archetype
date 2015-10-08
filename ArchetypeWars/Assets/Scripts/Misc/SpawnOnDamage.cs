using UnityEngine;
using System.Collections;


//=================================================
//NOT USED AT THE MOMENT
//=================================================


public class SpawnOnDamage : MonoBehaviour {



	public GameObject hole;
	public float dThresh; //how much damage taken before the object breaks or explodes
	public GameObject explosion;
	public AudioClip explosionClip;

	void Damage(RaycastHit hit)
	{
		if (dThresh < 3) 
		{
			audio.clip = explosionClip;
			audio.Play();
			GameObject effect = (GameObject) Instantiate (explosion,transform.position + new Vector3(0, 10, 0), transform.rotation);
			effect.SetActive (true);
			if (gameObject != null)
			{
				Destroy (gameObject);
			}

		} 
		else 
		{
			GameObject bulletHole = (GameObject) Instantiate (hole);
			bulletHole.SetActive (true);
			bulletHole.transform.position = hit.point + 0.02f * hit.normal; // Vector3.up - 3.8f * Vector3.Cross(Vector3.up,hit.normal).normalized; //offsetting to avoid z-fighting
			bulletHole.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
			bulletHole.transform.parent = hit.collider.gameObject.transform; //useful for dynamic objects
			print ("particle spawned");
		}

	}



}
