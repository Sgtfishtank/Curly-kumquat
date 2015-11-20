using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour 
{
	public float moveSpeed;
	public float jumpForce;

	private Rigidbody RB;

	void Start () 
	{
		RB = GetComponent<Rigidbody> ();
	}

	void Update () 
	{
		if (Input.GetKey(KeyCode.A))
		{
			transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.S))
		{
			transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.D))
		{
			transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.W))
		{
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			RB.velocity = new Vector3(RB.velocity.x, jumpForce, RB.velocity.z);
		}

	}
}
