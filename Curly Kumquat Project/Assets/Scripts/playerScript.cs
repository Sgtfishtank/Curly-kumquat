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
	public float mStunDuration;
	public float mDashStunDuration;
	
	private int numberOfJumps;
	private int maxJumps = 2;

	public int LeftButtonCount;
	public int RightButtonCount;
	public int DownButtonCount;
	public int UpButtonCount;

	private float buttonCooldown = 0.5F;

	public GameObject mOinionBodyPrefab;
	public GameObject mCarrotBodyPrefab;

	private float gravityForce;
	private Rigidbody RB;

	private bool mIsDead;
	private int mPlayerID;

	private bool mStunning;
	private float mStunT;
	private bool mKnockBacking;

	private KeyCode mLeftKey = KeyCode.A;
	private KeyCode mRightKey = KeyCode.D;
	private KeyCode mUpKey = KeyCode.W;
	private KeyCode mDownKey = KeyCode.S;
	private KeyCode mSpaceKey = KeyCode.Space;

	private Animator mAni;

	private FMOD.Studio.EventInstance mScream;
	private FMOD.Studio.EventInstance mDash;
	private FMOD.Studio.EventInstance mDashHit;
	private FMOD.Studio.EventInstance mHit;
	private FMOD.Studio.EventInstance mKnifeBodyHit;

	private float mSayTime;
	private bool mDashing = false;
	private float mDashT;
	private GameObject mBody;
	private float mCrossT;

	void Awake()
	{
		mCrossT = -1;
		RB = GetComponent<Rigidbody>();
		gravityForce = -45;
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

		mAni = mBody.GetComponent<Animator> ();
		mAni.CrossFade("Running", 0.5f, 0, Random.value);
		mBody.transform.parent = transform;
		mBody.transform.localPosition = Vector3.zero;
		mBody.transform.localRotation = Quaternion.Euler (0,-90, 0);
	}

	void Start () 
	{
		mHit = FMOD_StudioSystem.instance.GetEvent("event:/PlayerCollide/PlayerCollide");
		mDash = FMOD_StudioSystem.instance.GetEvent("event:/Dash/Dash");
		mDashHit = FMOD_StudioSystem.instance.GetEvent("event:/Dash Hit/DashHit");
		mScream = FMOD_StudioSystem.instance.GetEvent("event:/Scream/Scream");
		mKnifeBodyHit = FMOD_StudioSystem.instance.GetEvent("event:/KnifeBodyHit/KnifebodyHit");
	}

	void Update () 
	{

		if ((mCrossT < Time.time) && (mCrossT > 0))
		{
			print("crossfade");
			mAni.CrossFade("Running", 0.5f, 0, Random.value);
			mCrossT = -1;
		}
		
		if (mSayTime < Time.time) 
		{
			AudioManager.Instance.PlaySoundOnce(mScream);
			mSayTime = Time.time + Random.Range(2.5f, 10.0f);
		}

		if (mStunning)
		{
			if (mStunT < Time.time)
			{
				mStunning = false;
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

		if ( buttonCooldown > 0 )
		{
			buttonCooldown -= 1 * Time.deltaTime ;
			if (buttonCooldown < 0) 
			{
				LeftButtonCount = 0;
				RightButtonCount = 0;
				DownButtonCount = 0;
				UpButtonCount = 0;
			}
		}
		else
		{
		}

		if ((!mStunning) && (!mKnockBacking))
		{
			if (Input.GetKey(mLeftKey))
			{
				RightButtonCount = 0;
				DownButtonCount = 0;
				UpButtonCount = 0;

				if (Input.GetKeyDown (mLeftKey))
				{
					LeftButtonCount++;
					print("Left");
				}

				mBody.transform.localScale = new Vector3(1, 1, 1);
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
					print("Down");
				}
				transform.Translate(Vector3.back * moveSpeed2 * Time.deltaTime, Space.World);
			}
			
			if (Input.GetKey(mRightKey))
			{
				LeftButtonCount = 0;
				DownButtonCount = 0;
				UpButtonCount = 0;

				if (Input.GetKeyDown (mRightKey))
				{
					RightButtonCount++;
					print("Right");
				}

				mBody.transform.localScale = new Vector3(1, 1, -1);
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
					print("Up");
				}
				transform.Translate(Vector3.forward * moveSpeed2 * Time.deltaTime, Space.World);
			}

			if (Input.GetKeyDown(mSpaceKey) && numberOfJumps < maxJumps)
			{
				mCrossT = Time.time + ((30 / 24.0f) / 1.4f) - 0.5f;
				mAni.SetTrigger("Jump");
				numberOfJumps++;
				RB.velocity = new Vector3(RB.velocity.x, jumpForce, RB.velocity.z);
			}
		}

		if (LeftButtonCount >= 1 || RightButtonCount >= 1 || DownButtonCount >= 1 || UpButtonCount >= 1)
		{
			if ( buttonCooldown > 0)
			{
				if (LeftButtonCount == 2 || RightButtonCount == 2 || DownButtonCount == 2 || UpButtonCount == 2)
				{
					Dash();
					buttonCooldown = 0;
					LeftButtonCount = 0;
					RightButtonCount = 0;
					DownButtonCount = 0;
					UpButtonCount = 0;
				}
				else
				{
					/*LeftButtonCount = 0;
					RightButtonCount = 0;
					DownButtonCount = 0;
					UpButtonCount = 0;*/
				}
			}
			else
			{
				buttonCooldown = 0.5F ;
			}
		}

		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp(pos.z, -4, 3);
		transform.position = pos;
	}

	void Dash ()
	{
		AudioManager.Instance.PlaySoundOnce(mDash);
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
		mDashing = false;
		DownButtonCount = 0;
		LeftButtonCount = 0;
		RightButtonCount = 0;
		UpButtonCount = 0;
		gameObject.SetActive (true);
		mIsDead = false;
		RB.velocity = Vector3.zero;
		RB.angularVelocity = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.position = Vector3.zero;
		mKnockBacking = false;
		numberOfJumps = 0;
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

	void Stun (float duration)
	{
		mStunning = true;
		mStunT = Time.time + duration;
	}

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag == "Ground")
		{
			if (numberOfJumps > 0) 
			{
				numberOfJumps = 0;
			}

			mKnockBacking = false;
		}
		else if (coll.collider.tag == "Player") 
		{
			RB.velocity = Vector3.zero;
			playerScript otherPlayer = coll.collider.GetComponent<playerScript>();
			bool otherDashing = otherPlayer.mDashing;
			Vector3 dir = (transform.position - coll.collider.transform.position).normalized;

			if (mDashing && otherDashing) // both dashing
			{
				//Stun(mDashStunDuration);
				KnockBack(dir, 14);
				AudioManager.Instance.PlaySoundOnce(mHit);
			}
			else if (mDashing && (!otherDashing)) // me dashing him
			{
				AudioManager.Instance.PlaySoundOnce(mDashHit);
			}
			else if ((!mDashing) && otherDashing) // him dashing me
			{
				KnockBack(dir, 10);
				Stun(mStunDuration);
			}
			else // neither dashing
			{
				//Stun(mStunDuration);
				KnockBack(dir, 7);
				AudioManager.Instance.PlaySoundOnce(mHit);
			}
		}
		else if (coll.collider.tag == "Knife") 
		{
			AudioManager.Instance.PlaySoundOnce(mKnifeBodyHit);
			Kill();
		}
	}

	void KnockBack(Vector3 dir, float f)
	{
		mKnockBacking = true;
		RB.velocity = dir * f;
		RB.velocity += new Vector3(0, f, 0);
	}
	
	void OnCollisionExit(Collision coll)
	{
	
	}
}
