using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	public GameObject[] mPlayers = new GameObject[4];
	public GameObject mChef;



	void Awake()
	{
		for (int i = 0; i < 4; i++) 
		{
			//mPlayers[i] = CreatePlayer(i);
		}
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
