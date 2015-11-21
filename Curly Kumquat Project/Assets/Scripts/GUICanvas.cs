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
				GameObject thisObject = GameObject.Find("Canvas");
				instance = thisObject.GetComponent<GUICanvas>();
			}
			return instance;
		}
	}

	public Text mWinText;
	public Text[] mPlayerTexts = new Text[4];
	
	public GameObject mStart;
	public GameObject mPlaying;
	public GameObject mEnd;

	void Awake()
	{
		mStart = transform.Find("Start").gameObject;
		mPlaying = transform.Find("Playing").gameObject;
		mEnd = transform.Find("End").gameObject;

		for (int i = 0; i < 4; i++) 
		{
			mPlayerTexts[i] = mPlaying.transform.Find("PlayerText" + (1 + i)).GetComponent<Text>();
		}

		mWinText = mEnd.transform.Find("WinText").GetComponent<Text>();
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
			mPlayerTexts[i].gameObject.SetActive(true);
			if (Game.Instance.GetPlayer(i).IsDead()) 
			{
				mPlayerTexts[i].text = "Player " + (i + 1) + ": Dead";
			}
			else 
			{
				mPlayerTexts[i].text = "Player " + (i + 1) + ": Aviobe";
			}
		}
	}

	public void StartGame(int i)
	{
		Game.Instance.StartGame(i);
	}

	public void ShowEnd(bool show)
	{
		mEnd.gameObject.SetActive (show);
	}
	
	public void ShowStart(bool show)
	{
		mStart.gameObject.SetActive (show);
	}
	
	public void ShowPlaying(bool show)
	{
		mPlaying.gameObject.SetActive (show);
	}

	public void SetWin(int i)
	{
		mWinText.text = "Player " + (i + 1) + " wons!";
	}
}
