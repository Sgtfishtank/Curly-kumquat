using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour 
{
	public float moveSpeed;
	public float jumpForce;
	public float gravityForce;

	private Rigidbody RB;
	
	private bool mIsDead;
	private KeyCode mLeftKey;
	private KeyCode mRightKey;
	private KeyCode mUpKey;
	private KeyCode mDownKey;
	private KeyCode mSpaceKey;

	public void CreatePlayer (int i)
	{
		switch (i) 
		{
		case 0:
			InitKeys(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
			break;
		case 1:
			InitKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
			break;
		case 2:
			InitKeys(KeyCode.Keypad8, KeyCode.Keypad2, KeyCode.Keypad4, KeyCode.Keypad6);
			break;
		case 3:
			InitKeys(KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L);
			break;
		}
	}

	void Start () 
	{
		RB = GetComponent<Rigidbody> ();
		gravityForce *= -1;
		Physics.gravity = new Vector3 (0, gravityForce, 0);
	}

	void Update () 
	{
		if (Input.GetKey(mUpKey))
		{
			transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKey(mDownKey))
		{
			transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKey(mLeftKey))
		{
			transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKey(mRightKey))
		{
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKeyDown(mSpaceKey))
		{
			RB.velocity = new Vector3(RB.velocity.x, jumpForce, RB.velocity.z);
		}

	}
	
	void InitKeys (KeyCode w, KeyCode s, KeyCode a, KeyCode d)
	{
		mLeftKey = a;
		mRightKey = d;
		mUpKey = w;
		mDownKey = s;
		mSpaceKey = KeyCode.Space;
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
