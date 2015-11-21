using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour
{
	//private Camera mCamera;

	void Awake()
	{
		//mCamera = GetComponent<Camera>();
	}

    void Start()
    {

    }

    void Update()
    {
		if (Game.Instance.CurrentState() == Game.State.Menu) 
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Mathf.Sin(Time.time) * 15, transform.rotation.eulerAngles.z);
		}
		else 
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
		}
	}
}
