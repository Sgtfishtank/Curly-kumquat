using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public bool mIsDead;
	public KeyCode mLeftKey;
	public KeyCode mRightKey;
	public KeyCode mUpKey;
	public KeyCode mDownKey;

	// Use this for initialization
	void Start () 
	{
	
	}

	public void CreatePlayer (int i)
	{
		switch (i) 
		{
		case 0:
			InitKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
			break;
		case 1:
			InitKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
			break;
		case 2:
			InitKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
			break;
		case 3:
			InitKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
			break;
		}
	}

	void InitKeys (KeyCode w, KeyCode s, KeyCode a, KeyCode d)
	{
		mLeftKey = a;
		mRightKey = d;
		mUpKey = w;
		mDownKey = s;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Reset ()
	{
		mIsDead = false;
	}

	public bool IsDead ()
	{
		return mIsDead;
	}
	
	public void Kill()
	{
		mIsDead = true;
	}
}
