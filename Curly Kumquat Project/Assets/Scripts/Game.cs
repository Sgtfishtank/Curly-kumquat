using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	private static Game instance = null;
	public static Game Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("Game");
				instance = thisObject.GetComponent<AudioManager>();
			}
			return instance;
		}
	}

	public GameObject mPlayerPrefab;
	public GameObject[] mStartPositinos = new GameObject[4];
	public MasterChef mChef;

	private GameObject[] mPlayers = new GameObject[4];

	private bool mGameStarted;
	private bool mGameEnded;

	void Awake()
	{
		for (int i = 0; i < 4; i++) 
		{
			mPlayers[i] = Instantiate<GameObject>(mPlayerPrefab);
		}

		mGameStarted = true;
		mGameEnded = false;
	}

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < 4; i++) 
		{

			Vector3 pos = mStartPositinos[i].transform.position;
			
			//mPlayers[i].CreatePlayer(i, pos);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!mGameStarted) 
		{
			// start game
		}
		else if (!mGameEnded)
		{
			// game running
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
		else 
		{
			// game ended
			if (Input.GetKeyDown(KeyCode.R)) 
			{
				Reset();
			}

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

		mChef.Reset();
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
