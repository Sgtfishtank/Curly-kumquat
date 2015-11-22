using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour
{
	public GameObject MenuCamera;

	private Vector3 menipos;
	private Quaternion menirot;

	private Vector3 gamepos;
	private Quaternion gamerot;
	//private Camera mCamera;

	void Awake()
	{
		menipos = MenuCamera.transform.position;
		menirot = MenuCamera.transform.rotation; 

		// REOMVE KEBAB
		menipos = transform.position;
		menirot = transform.rotation;

		gamepos = transform.position;
		gamerot = transform.rotation;
		//mCamera = GetComponent<Camera>();
	}

    void Start()
    {

    }

    void Update()
    {
		transform.position = gamepos;
		if (Game.Instance.CurrentState() == Game.State.Menu) 
		{
			transform.position = menipos;
			transform.rotation = menirot; 
			//transform.rotation = Quaternion.Euler(gamerot.eulerAngles.x, gamerot.eulerAngles.y + (Mathf.Sin(Time.time) * 15), gamerot.eulerAngles.z);
		}
		else 
		{
			transform.position = Vector3.MoveTowards(transform.position, gamepos, Time.deltaTime * 10);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, gamerot, Time.deltaTime * 10);
		}
	}
}
