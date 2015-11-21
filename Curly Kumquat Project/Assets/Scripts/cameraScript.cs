using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour
{
	private Camera mCamera;

	void Awake()
	{
		mCamera = GetComponent<Camera>();
	}

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {

        }
    }
}
