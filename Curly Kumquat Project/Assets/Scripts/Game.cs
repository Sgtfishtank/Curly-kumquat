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

	public enum State
	{
		Menu,
		Playing,
		End
	}
	public GameObject mPlayerPrefab;

	private GameObject[] mStartPositinos = new GameObject[4];
	private MasterChef mMasterChef;
	private playerScript[] mPlayers;
	private State mCurrentState;

	void Awake()
	{
		mMasterChef = GameStarter.Instance.MasterChef();

		for (int i = 0; i < 4; i++)
		{
			mStartPositinos[i] = transform.Find("StartPos" + (i + 1)).gameObject;
		}

		mCurrentState = State.Menu;
	}

	// Use this for initialization
	void Start () 
	{
		mMasterChef.enabled = false;
		UpdateGUI();
		GUICanvas.Instance.ShowQuit(true);
	}

	void UpdateGUI()
	{
		GUICanvas.Instance.Show(mCurrentState);
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (mCurrentState) 
		{
		case State.Menu:
			// mrnu
			UpdateMenu();
			break;
		case State.Playing:
			// game running
			UpdatePlaying();
			break;
		case State.End:
			// game ended
			UpdateEnd();
			break;
		default:
			Debug.Log("Error gem state");
			break;
		}
	}

	public void UpdateMenu()
	{

	}

	public void UpdatePlaying()
	{
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
		
		if (((playersAlive < 2) && (mPlayers.Length > 1)) || ((playersAlive < 1) && (mPlayers.Length == 1)))
		{
			EndGame(playerID);
		}
	}

	public void UpdateEnd()
	{
		if (Input.GetKeyDown(KeyCode.R)) 
		{
			Reset();
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

		mCurrentState = State.Playing;
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
		mCurrentState = State.End;
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
		switch (mCurrentState) 
		{
		case State.Menu:
			// menu
			break;
		case State.Playing:
			// game running
			for (int i = 0; i < mPlayers.Length; i++) 
			{
				Vector3 pos = mStartPositinos[i].transform.position;
				mPlayers[i].Reset();
				
				mPlayers[i].transform.position = pos;
				mPlayers[i].CreatePlayer(i, (playerScript.FruitType)(Random.Range(0, (int)playerScript.FruitType.FruitCount)));
			}
			mMasterChef.Reset();
			break;
		case State.End:
			// game ended
			mMasterChef.Reset();
			mCurrentState = State.Menu;
			break;
		default:
			Debug.Log("Error gem state");
			break;
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

	public State CurrentState()
	{
		return mCurrentState;
	}
}
