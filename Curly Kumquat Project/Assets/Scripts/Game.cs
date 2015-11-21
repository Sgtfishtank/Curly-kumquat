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
				instance = GameStarter.Instance.Game();
			}
			return instance;
		}
	}

	public GameObject mPlayerPrefab;

	private GameObject[] mStartPositinos = new GameObject[4];
	private MasterChef mMasterChef;
	private playerScript[] mPlayers;
	private bool mGameStarted;
	private bool mGameEnded;

	void Awake()
	{
		mMasterChef = GameStarter.Instance.MasterChef();

		for (int i = 0; i < 4; i++)
		{
			mStartPositinos[i] = transform.Find("StartPos" + (i + 1)).gameObject;
		}

		mGameStarted = false;
		mGameEnded = false;
	}

	// Use this for initialization
	void Start () 
	{
		mMasterChef.enabled = false;
		UpdateGUI();
		GUICanvas.Instance.ShowQuit(true);
	}

	void UpdateGUI ()
	{
		GUICanvas.Instance.ShowEnd(mGameEnded);
		GUICanvas.Instance.ShowStart(!mGameStarted);
		GUICanvas.Instance.ShowPlaying(mGameStarted && (!mGameEnded));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!mGameStarted) 
		{
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
			for (int i = 0; i < mPlayers.Length; i++) 
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
			
			if (playersAlive < 2) 
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

	public void StartGame (int playerCount)
	{
		GUICanvas.Instance.ShowQuit(false);
		mMasterChef.enabled = true;
		mPlayers = new playerScript[playerCount];
		for (int i = 0; i < mPlayers.Length; i++)
		{
			mPlayers[i] = Instantiate<GameObject>(mPlayerPrefab).GetComponent<playerScript>();
		}
		
		for (int i = 0; i < mPlayers.Length; i++) 
		{
			Vector3 pos = mStartPositinos[i].transform.position;
			
			mPlayers[i].transform.position = pos;
			mPlayers[i].CreatePlayer(i, (playerScript.FruitType)(Random.Range(0, (int)playerScript.FruitType.FruitCount)));
		}

		mGameStarted = true;
		mGameEnded = false;
		UpdateGUI();
	}

	public int PlayerCount ()
	{
		if (mPlayers == null) 
		{
			return 0;
		}

		return mPlayers.Length;
	}

	void EndGame (int playerID)
	{
		mMasterChef.enabled = false;
		// game ends
		GUICanvas.Instance.SetWin(playerID);
		mGameEnded = true;
		for (int i = 0; i < mPlayers.Length; i++) 
		{
			Destroy(mPlayers[i].gameObject);
		}
		GUICanvas.Instance.ShowQuit(true);
		mPlayers = null;
		UpdateGUI();
	}

	public playerScript GetPlayer(int playerID)
	{
		return mPlayers[playerID];
	}

	void Reset()
	{
		if (mGameEnded) 
		{
			mGameEnded = false;
			mGameStarted = false;
			mMasterChef.Reset();
		}
		else if (mGameStarted) 
		{
			for (int i = 0; i < mPlayers.Length; i++) 
			{
				Vector3 pos = mStartPositinos[i].transform.position;
				mPlayers[i].Reset();

				mPlayers[i].transform.position = pos;
				mPlayers[i].CreatePlayer(i, (playerScript.FruitType)(Random.Range(0, (int)playerScript.FruitType.FruitCount)));
			}

			mGameEnded = false;
			mGameStarted = true;
			mMasterChef.Reset();
		}
		else
		{
			// 	
		}

		UpdateGUI();
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
