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
		cooldown = Random.Range(2,5) +Time.time;
		float emission = Mathf.PingPong (localtime*scale, 1.0f);
		Color baseColor = Color.yellow; //Replace this with whatever you want for your base color at emission level '1'
		Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
		mat.SetColor ("_EmissionColor", finalColor);
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.time < cooldown) || (Game.Instance.CurrentState() != Game.State.Playing))
			return;

		localtime += Time.deltaTime;

		float emission = Mathf.PingPong (localtime*scale, 1.0f);
		Color baseColor = Color.yellow; //Replace this with whatever you want for your base color at emission level '1'

		if (emission <= 0.01f)
		{
			emission = 0;
			cooldown = Random.Range(2,5) +Time.time;
		}
		else if (emission >= 0.99f)
		{
			emission = 1;
			cooldown = Random.Range(2,5) +Time.time;
		}
		
		transform.tag = "Untagged";

		if (emission > 0.1f) 
		{
			transform.tag = "Stove";
		}
		
		Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
		mat.SetColor ("_EmissionColor", finalColor);

		print(emission);
	}
}
