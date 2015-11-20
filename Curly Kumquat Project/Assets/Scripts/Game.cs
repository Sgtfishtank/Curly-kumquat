using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	public GameObject[] mPlayers = new GameObject[4];
	public GameObject[] mStartPositinos = new GameObject[4];
	public GameObject mChef;

	public bool mGameStarted;
	public bool mGameEnded;

	void Awake()
	{

		for (int i = 0; i < 4; i++) 
		{
			Vector3 pos = mStartPositinos[i].transform.position;

			//mPlayers[i].CreatePlayer(i, pos);
		}
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.R)) 
		{
			Reset();
		}

		int playersAlive = 0;
		for (int i = 0; i < 4; i++) 
		{
			//if (mPlayers[i].IsDead()) 
			{
				playersAlive++;
			}

			if (OutOfBounds(mPlayers[i])) 
			{
				//mPlayers.Kill();
			}
		}

		if (playersAlive == 1) 
		{
			EndGame();
		}
	}

	void EndGame ()
	{
		// game ends
		mGameEnded = true;
	}

	void Reset()
	{
		for (int i = 0; i < 4; i++) 
		{
			//mPlayers[i].Reset();
		}
	}

	bool OutOfBounds (GameObject gameObject)
	{
		if (gameObject.transform.position.magnitude > 25f) 
		{
			return true;
		}

		if (gameObject.transform.position.y < -5f) 
		{
			return true;
		}

		return false;
	}
}
