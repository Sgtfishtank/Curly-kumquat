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

	void Awake()
	{
		for (int i = 0; i < 4; i++) 
		{
			mPlayerTexts[i] = transform.Find("PlayerText" + (1 + i)).GetComponent<Text>();
		}

		mWinText = transform.Find("WinText").GetComponent<Text>();
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < 4; i++) 
		{
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

	public void HideWin ()
	{
		mWinText.gameObject.SetActive (false);
	}

	public void ShowWin(int i)
	{
		mWinText.gameObject.SetActive (true);
		mWinText.text = "Player " + i + " wons!";
	}
}
