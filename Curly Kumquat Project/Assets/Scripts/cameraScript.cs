using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour
{
	private Vector3 pos;
	private Quaternion rot;
	//private Camera mCamera;

	void Awake()
	{
		pos = transform.position;
		rot = transform.rotation;
		//mCamera = GetComponent<Camera>();
	}

    void Start()
    {

    }

    void Update()
    {
		transform.position = pos;
		if (Game.Instance.CurrentState() == Game.State.Menu) 
		{
			transform.rotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y + (Mathf.Sin(Time.time) * 15), rot.eulerAngles.z);
		}
		else 
		{
			transform.rotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
		}
	}
}
