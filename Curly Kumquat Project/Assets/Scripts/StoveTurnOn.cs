using UnityEngine;
using System.Collections;

public class StoveTurnOn : MonoBehaviour {


	Renderer renderer;
	Material mat;
	public float scale;
	float cooldown;
	float localtime;
	// Use this for initialization
	void Start ()
	{
		renderer = GetComponent<Renderer> ();
		mat = renderer.material;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time < cooldown)
			return;

		localtime += Time.deltaTime;

		float emission = Mathf.PingPong (localtime*scale, 1.0f);
		Color baseColor = Color.yellow; //Replace this with whatever you want for your base color at emission level '1'
		
		Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
		mat.SetColor ("_EmissionColor", finalColor);
		if(emission <= 0.002f)
		{
			transform.tag = "Untagged";
			cooldown = Random.Range(2,5) +Time.time;
		}
		if(emission >= 0.998f)
		{
			transform.tag = "Stove";
			cooldown = Random.Range(2,5) +Time.time;
		}
	
	}
}
