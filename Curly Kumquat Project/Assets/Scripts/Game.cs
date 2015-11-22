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
	private FMOD.Studio.EventInstance mMenuJIZZINMYPANTS;
	private FMOD.Studio.EventInstance mIntenseMusic;
	private FMOD.Studio.EventInstance mGameMusic;
	private FMOD.Studio.EventInstance mEndMusic;
	

	private GameObject[] mStartPositinos = new GameObject[4];
	private MasterChef mMasterChef;
	public playerScript[] mPlayers;
	private State mCurrentState;
	private bool mInstnse;
	public GameObject mGB;

	void Awake()
	{
		Debug.LogError ("missing Credits");
		Debug.LogError ("missing icon");
		Debug.LogError ("missing acioent stundios");
		mMasterChef = GameStarter.Instance.MasterChef();

		for (int i = 0; i < 4; i++)
		{
			mStartPositinos[i] = transform.Find("StartPos" + (i + 1)).gameObject;
		}
		mGB = GameStarter.Instance.BG();
		mCurrentState = State.Menu;
	}

	// Use this for initialization
	void Start () 
	{
		mMenuJIZZINMYPANTS = FMOD_StudioSystem.instance.GetEvent("event:/Music/Startmenu");
		mIntenseMusic = FMOD_StudioSystem.instance.GetEvent("event:/Music/Intensemusic");
		mGameMusic = FMOD_StudioSystem.instance.GetEvent("event:/Music/Song 1");
		mEndMusic = FMOD_StudioSystem.instance.GetEvent("event:/Applause/Applause");

		initplayers(4);
		SpawnPlayers ();
		mMasterChef.enabled = false;
		UpdateGUI();
		GUICanvas.Instance.ShowQuit(true);
		AudioManager.Instance.PlayMusic(mMenuJIZZINMYPANTS);
	}

	void initplayers (int size)
	{
		mPlayers = new playerScript[size];
		for (int i = 0; i < mPlayers.Length; i++)
		{
			mPlayers[i] = Instantiate<GameObject>(mPlayerPrefab).GetComponent<playerScript>();
		}
	}

	void UpdateGUI()
	{
		GUICanvas.Instance.Show(mCurrentState);
		mGB.SetActive(mCurrentState == State.Menu);
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

		if ((playersAlive == 2) && (mPlayers.Length > 2) && (!mInstnse))
		{
			mInstnse = true;
			AudioManager.Instance.StopMusic(mGameMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			AudioManager.Instance.PlayMusic(mIntenseMusic);
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
			Game.Instance.RestartGame();
		}
		else if (Input.GetKeyDown(KeyCode.T)) 
		{
			Game.Instance.Reset();
		}
	}
	
	public void StartGame ()
	{
		StartGame (mPlayers.Length);
	}

	public void StartGame (int playerCount)
	{
		GameObject[] fd = GameObject.FindGameObjectsWithTag ("Splatoon");
		for (int i = 0; i < fd.Length; i++) 
		{
			Destroy(fd[i]);
		}
		mCurrentState = State.Playing;

		GUICanvas.Instance.ShowQuit(false);
		mMasterChef.enabled = true;
		
		Desyoplayers();
		
		initplayers(playerCount);
		SpawnPlayers();
		
		AudioManager.Instance.StopMusic(mMenuJIZZINMYPANTS, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

		if (mPlayers.Length <= 2) 
		{
			AudioManager.Instance.PlayMusic(mIntenseMusic);
		}
		else {
			AudioManager.Instance.PlayMusic(mGameMusic);
		}
		UpdateGUI();
	}

	void SpawnPlayers ()
	{
		for (int i = 0; i < mPlayers.Length; i++) 
		{
			Vector3 pos = mStartPositinos[i].transform.position;

			mPlayers[i].GetComponent<Rigidbody>().isKinematic = (mCurrentState == State.Menu);
			mPlayers[i].transform.position = pos;
			mPlayers[i].transform.position += mPlayerPrefab.transform.position;
			mPlayers[i].CreatePlayer(i, (playerScript.FruitType)(Random.Range(0, (int)playerScript.FruitType.FruitCount)));
		}
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
		mCurrentState = State.End;
		AudioManager.Instance.StopMusic(mGameMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Instance.StopMusic(mIntenseMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (mPlayers.Length > 1) 
		{
			AudioManager.Instance.PlayMusic (mEndMusic);
		}
		mMasterChef.enabled = false;
		// game ends
		GUICanvas.Instance.SetWin(playerID);

		mCurrentState = State.End;
		for (int i = 0; i < mPlayers.Length; i++) 
		{
			if (mPlayers[i] != null) 
			{
				mPlayers[i].enabled = false;
			}
		}
		GUICanvas.Instance.ShowQuit(true);
		//mPlayers = null;
		UpdateGUI();
	}

	void Desyoplayers ()
	{
		for (int i = 0; i < mPlayers.Length; i++) 
		{
			if (mPlayers[i] != null) 
			{
				Destroy(mPlayers[i].gameObject);
			}
			mPlayers[i] = null;
		}
	}

	public playerScript GetPlayer(int playerID)
	{
		return mPlayers[playerID];
	}

	public void Reset()
	{
		mInstnse = false;
		switch (mCurrentState) 
		{
		case State.Menu:
			// menu
			AudioManager.Instance.StopMusic(mMenuJIZZINMYPANTS, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			break;
		case State.Playing:
			GameReset ();
			SpawnPlayers();
			break;
		case State.End:
			mCurrentState = State.Menu;
			// game ended
			Desyoplayers();
			initplayers(4);
			SpawnPlayers();
			AudioManager.Instance.PlayMusic(mMenuJIZZINMYPANTS);
			AudioManager.Instance.StopMusic(mEndMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			mMasterChef.Reset();
			break;
		default:
			Debug.Log("Error gem state");
			break;
		}

		UpdateGUI();
	}

	public void RestartGame ()
	{
		int size = mPlayers.Length;
		Game.Instance.Reset();
		Game.Instance.StartGame(size);
	}

	void GameReset()
	{
		// game running
		SpawnPlayers ();
		AudioManager.Instance.StopMusic(mGameMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Instance.StopMusic(mIntenseMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Instance.PlayMusic(mGameMusic);
		mMasterChef.Reset();
	}
	
	bool OutOfBounds (playerScript player)
	{
		if (player.transform.position.magnitude > 250f) 
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
