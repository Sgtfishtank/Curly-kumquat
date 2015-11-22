using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUICanvas : MonoBehaviour 
{
	private static GUICanvas instance = null;
	public static GUICanvas Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameStarter.Instance.Canvas();
			}
			return instance;
		}
	}

	public Image[] mWinText = new Image[4];
	private Text[] mPlayerTexts = new Text[4];
	
	private GameObject mStart;
	private GameObject mPlaying;
	private GameObject mEnd;
	private Button mQuitButton;

	void Awake()
	{
		mStart = transform.Find("Start").gameObject;
		mPlaying = transform.Find("Playing").gameObject;
		mEnd = transform.Find("End").gameObject;
		//mQuitButton = transform.Find("Quit").GetComponent<Button>();

		for (int i = 0; i < 4; i++) 
		{
			mPlayerTexts[i] = mPlaying.transform.Find("PlayerText" + (1 + i)).GetComponent<Text>();
			mWinText[i] = mEnd.transform.Find("p" + (i + 1)).GetComponent<Image>();
		}

	}

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < 4; i++) 
		{
			mPlayerTexts[i].gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < Game.Instance.PlayerCount(); i++) 
		{
			mPlayerTexts[i].gameObject.SetActive(false);
			/*if (Game.Instance.GetPlayer(i).IsDead()) 
			{
				mPlayerTexts[i].text = "Player " + (i + 1) + ": Dead";
			}
			else 
			{
				mPlayerTexts[i].text = "Player " + (i + 1) + ": Aviobe";
			}*/
		}
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void StartGame(int i)
	{
		Game.Instance.StartGame(i);
	}
	
	public void Restart()
	{
		Game.Instance.RestartGame();
	}
	
	public void MenuBack()
	{
		Game.Instance.Reset ();
	}

	public void Show (Game.State mCurrentState)
	{
		mEnd.gameObject.SetActive (mCurrentState == Game.State.End);
		mStart.gameObject.SetActive (mCurrentState == Game.State.Menu);
		mPlaying.gameObject.SetActive (mCurrentState == Game.State.Playing);
	}

	public void SetWin(int i2)
	{
		for (int i = 0; i < mWinText.Length; i++) 
		{
			mWinText[i].gameObject.SetActive(i == i2);
		}
	}

	public void ShowQuit (bool show)
	{
		//mQuitButton.gameObject.SetActive (show);
	}
}
