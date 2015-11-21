using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour 
{
	public float moveSpeed;
	public float jumpForce;
	public float mDashSpeed;
	public float mDashDuration;
	public float mKnockBackDuration;

	private string LastKeyPressed;

	private float buttonCooldown = 0.5F;
	private int buttonCount = 0;
	
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
	private KeyCode mDashKey = KeyCode.LeftShift;


	private bool mDashing = false;
	private float mDashT;

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
		if (mKnockBacking)
		{
			if (mKnockBackT < Time.time)
			{
				mKnockBacking = false;
			}
			else 
			{
				return;
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
			if ( buttonCooldown > 0 && buttonCount == 2 && Input.inputString == LastKeyPressed)
			{
				Dash();
			}

			else
			{
				buttonCooldown = 0.5F ; 
				buttonCount += 1 ;
			}
		}

		if ( buttonCooldown > 0 )
		{
			
			buttonCooldown -= 1 * Time.deltaTime ;
			
		}

		else
		{
			buttonCount = 0 ;
		}
		
		if (Input.GetKey(mLeftKey))
		{
			LastKeyPressed = mLeftKey.ToString();
			transform.Translate(Vector3.left * moveSpeed2 * Time.deltaTime, Space.World);
		}

		if (Input.GetKey(mDownKey))
		{
			LastKeyPressed = mLeftKey.ToString();
			transform.Translate(Vector3.back * moveSpeed2 * Time.deltaTime, Space.World);
		}

		if (Input.GetKey(mRightKey))
		{
			LastKeyPressed = mLeftKey.ToString();
			transform.Translate(Vector3.right * moveSpeed2 * Time.deltaTime, Space.World);
		}

		if (Input.GetKey(mUpKey))
		{
			LastKeyPressed = mLeftKey.ToString();
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
		InitKeys (w, s, a, d, KeyCode.Space, KeyCode.LeftShift);
	}

	void InitKeys (KeyCode w, KeyCode s, KeyCode a, KeyCode d, KeyCode jump, KeyCode dash)
	{
		mLeftKey = a;
		mRightKey = d;
		mUpKey = w;
		mDownKey = s;
		mSpaceKey = jump;
		mDashKey = dash;
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

	void KnockBack ()
	{
		mKnockBacking = true;
		mKnockBackT = Time.time + mKnockBackDuration;
	}

	void OnCollisionEnter(Collision coll)
	{
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
	}
	
	void OnCollisionExit(Collision coll)
	{
	
	}
}
