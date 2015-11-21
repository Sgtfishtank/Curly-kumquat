using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour 
{
	public enum FruitType
	{
		Onion,
		Carrot,

		// len of enum
		FruitCount
	}

	public float moveSpeed;
	public float jumpForce;
	public float mDashSpeed;
	public float mDashDuration;
	public float mKnockBackDuration;

	private int numberOfJumps;
	private int maxJumps = 2;
<<<<<<< HEAD
	
	private int LeftButtonCount;
	private int RightButtonCount;
	private int DownButtonCount;
	private int UpButtonCount;
	private bool LeftDash = false;
	private bool RightDash = false;
	private bool DownDash = false;
	private bool UpDash = false;
=======
	private string LastKeyPressed;
>>>>>>> origin/master
	private float buttonCooldown = 0.5F;

	public GameObject mOinionBodyPrefab;
	public GameObject mCarrotBodyPrefab;

	private float gravityForce;
	private Rigidbody RB;

	private bool mIsDead;
	private int mPlayerID;

	private bool mKnockBacking = false;
	private float mKnockBackT;

	private KeyCode mLeftKey = KeyCode.A;
	private KeyCode mRightKey = KeyCode.D;
	private KeyCode mUpKey = KeyCode.W;
	private KeyCode mDownKey = KeyCode.S;
	private KeyCode mSpaceKey = KeyCode.Space;


	private bool mDashing = false;
	private float mDashT;

	private GameObject mBody;

	void Awake()
	{
		RB = GetComponent<Rigidbody>();
		gravityForce = -50;
		Physics.gravity = new Vector3 (0, gravityForce, 0);
	}

	public void CreatePlayer (int playerID, FruitType type)
	{
		switch (playerID) 
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
		mPlayerID = playerID;

		if (mBody != null) 
		{
			Destroy(mBody);
		}
		switch (type) 
		{
		case FruitType.Carrot:
			mBody = Instantiate(mOinionBodyPrefab);
			break;
		case FruitType.Onion:
			mBody = Instantiate(mCarrotBodyPrefab);
			break;
		}

		mBody.transform.parent = transform;
		mBody.transform.localPosition = Vector3.zero;
	}

	void Start () 
	{
	}

	void Update () 
	{
		if (mKnockBacking)
		{
			if (mKnockBackT < Time.time)
			{
				mKnockBacking = false;
			}
		}

		float moveSpeed2 = moveSpeed;
		if (IsDashing ())
		{
			moveSpeed2 = mDashSpeed;
			if (mDashT < Time.time) 
			{
				mDashing = false;
			}
		}

		if (Input.anyKeyDown)
		{
			if ( buttonCooldown > 0 && LeftButtonCount == 2 || RightButtonCount == 2 || DownButtonCount == 2 || UpButtonCount == 2)
			{
				Dash();

				if (LeftButtonCount == 2)
				{
					LeftDash = true;
				}

				else if (RightButtonCount == 2)
				{
					RightDash = true;
				}

				else if (DownButtonCount == 2)
				{
					DownDash = true;
				}

				else if (UpButtonCount == 2)
				{
					UpDash = true;
				}
				LeftButtonCount = 0;
				RightButtonCount = 0;
				DownButtonCount = 0;
				UpButtonCount = 0;
			}

			else
			{
				buttonCooldown = 1F ;
			}
		}

		if ( buttonCooldown > 0 )
		{
			buttonCooldown -= 1 * Time.deltaTime ;
		}
		else
		{
			LeftButtonCount = 0;
			RightButtonCount = 0;
			DownButtonCount = 0;
			UpButtonCount = 0;
		}

		if (!mKnockBacking)
		{
			if (Input.GetKey(mLeftKey))
			{
<<<<<<< HEAD
				RightButtonCount = 0;
				DownButtonCount = 0;
				UpButtonCount = 0;

				if (Input.GetKeyDown (mLeftKey))
				{
					LeftButtonCount++;
				}
=======
				LastKeyPressed = mLeftKey.ToString();
				mBody.transform.localScale = new Vector3(1, 1, 1);
>>>>>>> origin/master
				transform.Translate(Vector3.left * moveSpeed2 * Time.deltaTime, Space.World);
			}
			
			if (Input.GetKey(mDownKey))
			{
				LeftButtonCount = 0;
				RightButtonCount = 0;
				UpButtonCount = 0;

				if (Input.GetKeyDown (mDownKey))
				{
					DownButtonCount++;
				}
				transform.Translate(Vector3.back * moveSpeed2 * Time.deltaTime, Space.World);
			}
			
			if (Input.GetKey(mRightKey))
			{
<<<<<<< HEAD
				LeftButtonCount = 0;
				DownButtonCount = 0;
				UpButtonCount = 0;

				if (Input.GetKeyDown (mRightKey))
				{
					RightButtonCount++;
				}
=======
				LastKeyPressed = mRightKey.ToString();
				mBody.transform.localScale = new Vector3(1, 1, -1);
>>>>>>> origin/master
				transform.Translate(Vector3.right * moveSpeed2 * Time.deltaTime, Space.World);
			}
			
			if (Input.GetKey(mUpKey))
			{
				LeftButtonCount = 0;
				RightButtonCount = 0;
				DownButtonCount = 0;

				if (Input.GetKeyDown (mUpKey))
				{
					UpButtonCount++;
				}
				transform.Translate(Vector3.forward * moveSpeed2 * Time.deltaTime, Space.World);
			}

			if (Input.GetKeyDown(mSpaceKey) && numberOfJumps < maxJumps)
			{
				numberOfJumps++;
				RB.velocity = new Vector3(RB.velocity.x, jumpForce, RB.velocity.z);
			}
		}

		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp(pos.z, -4, 3);
		transform.position = pos;
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
	}

	public bool IsDashing()
	{
		return mDashing;
	}

	public void Reset ()
	{
		gameObject.SetActive (true);
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
		gameObject.SetActive (false);
		mIsDead = true;
	}

	void KnockBack ()
	{
		mKnockBacking = true;
		mKnockBackT = Time.time + mKnockBackDuration;
	}

	void OnCollisionEnter(Collision coll)
	{
		
		if (coll.gameObject.tag == "Ground" && numberOfJumps > 0)
		{
			numberOfJumps = 0;
		}
		if (coll.collider.tag == "Player") 
		{
			playerScript otherPlayer = coll.collider.GetComponent<playerScript>();
			int otherID = otherPlayer.mPlayerID;
			print("ID: " + otherID);

			//otherPlayer.GetComponent<Rigidbody>().velocity;
			KnockBack();
			RB.velocity = (transform.position - coll.collider.transform.position).normalized * 7;
			RB.velocity += new Vector3(0, 7, 0);
		}
		else if (coll.collider.tag == "Knife") 
		{
			Kill();
		}
	}
	
	void OnCollisionExit(Collision coll)
	{
	
	}
}
