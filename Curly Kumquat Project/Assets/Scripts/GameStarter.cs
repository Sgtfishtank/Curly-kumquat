using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour 
{
	private static GameStarter instance = null;
	public static GameStarter Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("GameStarter");
				instance = thisObject.GetComponent<GameStarter>();
			}
			return instance;
		}
	}

	public GameObject mAM;
	public GameObject mMasterChef;
	public GameObject mEventSystem;
	public GameObject mCanvas;
	public GameObject mGame;
	public GameObject mMainCamera;

	void Awake()
	{
		GetOrCreate (mAM);
		GetOrCreate (mMasterChef);
		GetOrCreate (mEventSystem);
		GetOrCreate (mCanvas);
		GetOrCreate (mGame);
		GetOrCreate (mMainCamera);
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	GameObject GetOrCreate (GameObject prefab)
	{
		GameObject x = GameObject.Find(prefab.name);
		if (x == null) 
		{
			x = Instantiate<GameObject>(prefab);
			x.name = prefab.name;
		}

		return x;
	}

	public MasterChef MasterChef()
	{
		return GetOrCreate(mMasterChef).GetComponent<MasterChef>();
	}

	public GUICanvas Canvas ()
	{
		return GetOrCreate(mCanvas).GetComponent<GUICanvas>();
	}

	public Game Game ()
	{
		return GetOrCreate(mGame).GetComponent<Game>();
	}

	public AudioManager AudioManager ()
	{
		return GetOrCreate(mAM).GetComponent<AudioManager>();
	}
	
	public cameraScript Camera()
	{
		return GetOrCreate(mMainCamera).GetComponent<cameraScript>();
	}
}
