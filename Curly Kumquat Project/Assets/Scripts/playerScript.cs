using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour 
{
	public float moveSpeed;
	public float jumpForce;
	public float mDashSpeed;
	public float mDashDuration;
	
	private float gravityForce;
	private Rigidbody RB;
	
	private bool mIsDead;
	private KeyCode mLeftKey = KeyCode.A;
	private KeyCode mRightKey = KeyCode.D;
	private KeyCode mUpKey = KeyCode.W;
	private KeyCode mDownKey = KeyCode.S;
	private KeyCode mSpaceKey = KeyCode.Space;
	private KeyCode mDashKey = KeyCode.Space;

	private bool mDashing = false;
	private float mDashT;
	private int mPlayerID;

	void Awake()
	{
	
	}

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
		mPlayerID = i;
	}

	void Start () 
	{
		RB = GetComponent<Rigidbody> ();
		gravityForce = -50;
		Physics.gravity = new Vector3 (0, gravityForce, 0);
	}

	void Update () 
	{
		float moveSpeed2 = moveSpeed;
		if (IsDashing ())
		{
			moveSpeed2 = mDashSpeed;
			if (mDashT < Time.time) 
			{
				mDashing = false;
			}
		}

		if (Input.GetKey(mLeftKey))
		{
			transform.Translate(Vector3.left * moveSpeed2 * Time.deltaTime, Space.World);
		}

		if (Input.GetKey(mDownKey))
		{
			transform.Translate(Vector3.back * moveSpeed2 * Time.deltaTime, Space.World);
		}

		if (Input.GetKey(mRightKey))
		{
			transform.Translate(Vector3.right * moveSpeed2 * Time.deltaTime, Space.World);
		}

		if (Input.GetKey(mUpKey))
		{
			transform.Translate(Vector3.forward * moveSpeed2 * Time.deltaTime, Space.World);
		}

		if (Input.GetKeyDown(mSpaceKey))
		{
			RB.velocity = new Vector3(RB.velocity.x, jumpForce, RB.velocity.z);
		}

		if (Input.GetKeyDown(mDashKey))
		{
			Dash();
		}
	}

	void Dash ()
	{
		mDashT = Time.time + mDashDuration;
		mDashing = true;
	}

	void InitKeys (KeyCode w, KeyCode s, KeyCode a, KeyCode d)
	{
		InitKeys (w, s, a, d, KeyCode.Space);
	}

	void InitKeys (KeyCode w, KeyCode s, KeyCode a, KeyCode d, KeyCode jump)
	{
		mLeftKey = a;
		mRightKey = d;
		mUpKey = w;
		mDownKey = s;
		mSpaceKey = jump;
		mDashKey = jump;
	}

	public bool IsDashing()
	{
		return mDashing;
	}

	public void Reset ()
	{
		mIsDead = false;
		RB.velocity = Vector3.zero;
		RB.angularVelocity = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.position = Vector3.zero;
	}
	
	public bool IsDead ()
	{
		return mIsDead;
	}
	
	public void Kill()
	{
		mIsDead = true;
	}

	void OnCollisionEnter(Collision coll)
	{
		if (coll.collider.tag == "Player") 
		{
			playerScript otherPlayer = coll.collider.GetComponent<playerScript>();
			int otherID = otherPlayer.mPlayerID;
			print("ID: " + otherID);
		}
	}
	
	void OnCollisionExit(Collision coll)
	{
	
	}
}
