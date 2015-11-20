using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour 
{
	public GameObject mAM;
	public GameObject mMasterChef;
	public GameObject mEventSystem;
	public GameObject mCanvas;
	public GameObject mGame;

	void Awake()
	{
		Instantiate<GameObject>(mAM).name = mAM.name;
		Instantiate<GameObject>(mMasterChef).name = mMasterChef.name;
		Instantiate<GameObject>(mEventSystem).name = mEventSystem.name;
		Instantiate<GameObject>(mCanvas).name = mCanvas.name;
		Instantiate<GameObject>(mGame).name = mGame.name;
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
