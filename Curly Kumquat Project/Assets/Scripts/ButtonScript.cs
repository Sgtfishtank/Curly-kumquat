using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour 
{
	public GameObject buttonsource;
	public MeshRenderer mr;
	public Button but;


	// Use this for initialization
	void Start () 
	{
		//mr = buttonsource.GetComponent<MeshRenderer> ();
		//but = GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//mr.material.color = but.colors.pressedColor;
	}
}
