using UnityEngine;
using System.Collections;

public class StoveTurnOn : MonoBehaviour {


	Renderer renderer;
	Material mat;
	// Use this for initialization
	void Start ()
	{
		renderer = GetComponent<Renderer> ();
		mat = renderer.material;
	}
	
	// Update is called once per frame
	void Update () {
		float emission = Mathf.PingPong (Time.time*0.15f, 1.0f);
		Color baseColor = Color.yellow; //Replace this with whatever you want for your base color at emission level '1'
		
		Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
		
		mat.SetColor ("_EmissionColor", finalColor);
	
	}
}
