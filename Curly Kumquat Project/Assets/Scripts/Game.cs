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
				instance = thisObject.GetComponent<Game>();
			}
			return instance;
		}
	}

	public GameObject mPlayerPrefab;
	private GameObject[] mStartPositinos = new GameObject[4];
	private MasterChef mMasterChef;

	private playerScript[] mPlayers = new playerScript[4];

	private bool mGameStarted;
	private bool mGameEnded;

	void Awake()
	{
		mMasterChef = GameObject.Find("MasterChef").GetComponent<MasterChef>();
		for (int i = 0; i < 4; i++)
		{
			mStartPositinos[i] = transform.Find("StartPos" + (i + 1)).gameObject;
			mPlayers[i] = Instantiate<GameObject>(mPlayerPrefab).GetComponent<playerScript>();
		}

		mGameStarted = true;
		mGameEnded = false;
	}

	// Use this for initialization
	void Start () 
	{
		GUICanvas.Instance.HideWin();

		for (int i = 0; i < 4; i++) 
		{
			Vector3 pos = mStartPositinos[i].transform.position;

			mPlayers[i].transform.position = pos;
			mPlayers[i].CreatePlayer(i);
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
			int playerID = -1;
			for (int i = 0; i < 4; i++) 
			{
				if (!mPlayers[i].IsDead()) 
				{
					playersAlive++;
					playerID = i;
				}
				
				if (OutOfBounds(mPlayers[i])) 
				{
					mPlayers[i].Kill();
				}
			}
			
			if (playersAlive == 1) 
			{
				EndGame(playerID);
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

	void EndGame (int playerID)
	{
		// game ends
		GUICanvas.Instance.ShowWin(playerID);
		mGameEnded = true;
	}

	public playerScript GetPlayer(int playerID)
	{
		return mPlayers[playerID];
	}

	void Reset()
	{
		for (int i = 0; i < 4; i++) 
		{
			mPlayers[i].transform.position = mStartPositinos[i].transform.position;
			mPlayers[i].gameObject.SetActive(true);
			mPlayers[i].Reset();
		}
		
		GUICanvas.Instance.HideWin();
		mGameEnded = false;
		mGameStarted = true;
		mMasterChef.Reset();
	}

	bool OutOfBounds (playerScript player)
	{
		if (player.transform.position.magnitude > 25f) 
		{
			return true;
		}

		if (player.transform.position.y < -5f) 
		{
			return true;
		}

		return false;
	}
}
