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

	// dash smope on player
	// splatter on death
	// 


	public float moveSpeed;
	public float jumpForce;
	public float mDashSpeed;
	public float mDashDuration;
	public float mKnockBackDuration;
	public float zMin;
	public float zMax;
	public float xMin;
	public float xMax;
	public bool Is2D;

	public float mStunDuration;
	public float mDashStunDuration;
	
	public GameObject mPartPrefab;
	public GameObject[] mBodyParts;
	public GameObject[] mBodyParts2;
	public GameObject[] mBodyPartsBast;
	public GameObject[] mBodyParts2Bast;
	
	private int numberOfJumps;
	private int maxJumps = 2;

	public int LeftButtonCount;
	public int RightButtonCount;
	public int DownButtonCount;
	public int UpButtonCount;

	private float buttonCooldown = 0.5F;
	
	public GameObject mSplaterParticlesPrefab;
	public GameObject mDahsParticlesPrefab;
	public GameObject mOinionBodyPrefab;
	public GameObject mCarrotBodyPrefab;
	public GameObject mOinionBodyPartsPrefab;
	public GameObject mCarrotBodyPartsPrefab;
	public GameObject mRingParticlesPrefab;

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
	private FruitType mType;
	private bool mGotDashHit;
	
	public Material carrotMaterial;
	public Material carrotOtherMaterial;
	public Material onionMaterial;
	public Material onionOtherMaterial;
	private GameObject dathshit;

	void Awake()
	{
		mCrossT = -1;
		RB = GetComponent<Rigidbody>();
		gravityForce = -45;
		Physics.gravity = new Vector3 (0, gravityForce, 0);
		
		dathshit = Instantiate (mDahsParticlesPrefab, transform.position, transform.rotation) as GameObject;
		dathshit.transform.parent = transform;
		dathshit.transform.localPosition = mDahsParticlesPrefab.transform.localPosition;
		dathshit.transform.localRotation = mDahsParticlesPrefab.transform.localRotation;
		dathshit.transform.localScale = mDahsParticlesPrefab.transform.localScale;
		dathshit.SetActive(false);
	}

	public void CreatePlayer (int playerID, FruitType type)
	{
		switch (playerID) 
		{
		case 0:
			InitKeys(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D,KeyCode.LeftControl);
			break;
		case 1:
			InitKeys(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.RightControl);
			break;
		case 2:
			InitKeys(KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L, KeyCode.Space);
			break;
		case 3:
			InitKeys(KeyCode.Keypad8, KeyCode.Keypad5, KeyCode.Keypad4, KeyCode.Keypad6, KeyCode.KeypadEnter);
			break;
		}
		mPlayerID = playerID;

		if (mBody != null) 
		{
			Destroy(mBody);
		}

		mBodyParts = new GameObject[4];
		mBodyParts2 = new GameObject[4];
		mBodyPartsBast = new GameObject[3];
		mBodyParts2Bast = new GameObject[3];
		Transform trans;
		mType = (FruitType)(playerID % (int)FruitType.FruitCount);
		switch (mType) 
		{
		case FruitType.Carrot:
			trans = mCarrotBodyPartsPrefab.transform;
			mBody = Instantiate(mCarrotBodyPrefab);
			mBodyParts[0] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/group5/carrot_upper_left1").gameObject;
			mBodyParts[1] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/group5/carrot_upper_right1").gameObject;
			mBodyParts[2] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/group5/carrot_lower_right1_1").gameObject;
			mBodyParts[3] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/group5/carrot_lower_left1").gameObject;
			mBodyParts2[0] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/group5/carrot_upper_left1").gameObject;
			mBodyParts2[1] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/group5/carrot_upper_right1").gameObject;
			mBodyParts2[2] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/group5/carrot_lower_right1_1").gameObject;
			mBodyParts2[3] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/group5/carrot_lower_left1").gameObject;
			mBodyPartsBast[0] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/polySurface37/carrot_blast_left1").gameObject;
			mBodyPartsBast[1] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/polySurface37/carrot_blast_middle1").gameObject;
			mBodyPartsBast[2] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/polySurface37/carrot_blast_right1").gameObject;
			mBodyParts2Bast[0] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/polySurface37/carrot_blast_left1").gameObject;
			mBodyParts2Bast[1] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/polySurface37/carrot_blast_middle1").gameObject;
			mBodyParts2Bast[2] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/polySurface37/carrot_blast_right1").gameObject;
			break;
		case FruitType.Onion:
			trans = mOinionBodyPartsPrefab.transform;
			mBody = Instantiate(mOinionBodyPrefab);
			mBodyParts[0] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/onionbody/onion_body_part1").gameObject;
			mBodyParts[1] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/onionbody/onion_body_part2").gameObject;
			mBodyParts[2] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/onionbody/onion_body_part4").gameObject;
			mBodyParts[3] = mBody.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/onionbody/onion_body_part3").gameObject;
			mBodyParts2[0] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/onionbody/onion_body_part1").gameObject;
			mBodyParts2[1] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/onionbody/onion_body_part2").gameObject;
			mBodyParts2[2] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/onionbody/onion_body_part4").gameObject;
			mBodyParts2[3] = trans.transform.Find("full_body_ctrl/root_joint/hip_joint/Body_joint/onionbody/onion_body_part3").gameObject;
			break;
		}

		MeshRenderer[] renders = mBody.GetComponentsInChildren<MeshRenderer> ();
		Material refMat;
		Material otherMat;

		if (mType == FruitType.Carrot) 
		{
			refMat = carrotMaterial;
			otherMat = carrotOtherMaterial;
		}
		else 
		{
			refMat = onionMaterial;
			otherMat = onionOtherMaterial;
		}

		if ((playerID == 2) || (playerID == 3))
		{
			for (int i = 0; i < renders.Length; i++) 
			{
				if (renders [i].sharedMaterial == refMat) 
				{
					renders [i].material = otherMat;
				}
			}
		}


		mAni = mBody.GetComponent<Animator> ();
		mAni.CrossFade("Running", 0.5f, 0, Random.value);
		mBody.transform.parent = transform;
		mBody.transform.localPosition = Vector3.zero;
		mBody.transform.localRotation = Quaternion.Euler (0,-90, 0);
		mBody.transform.localRotation = Quaternion.Euler(0, 180, 0);
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
		RB.WakeUp();

		if (Game.Instance.CurrentState() != Game.State.Playing) 
		{
			return;
		}
		if (mGotDashHit) 
		{
			dathshit.SetActive(false);
			mDashing = false;
			mGotDashHit = false;
		}
		mAni.SetBool("moving",false);
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
				dathshit.SetActive(false);
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
				}

				mBody.transform.localScale = new Vector3(1, 1, 1);
				mBody.transform.localRotation = Quaternion.Euler(0, -90, 0);
				mAni.SetBool("moving",true);
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
				mAni.SetBool("moving",true);
				mBody.transform.localRotation = Quaternion.Euler(0, 180, 0);
				mBody.transform.localScale = new Vector3(1, 1, 1);
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
				}
				mAni.SetBool("moving",true);
				mBody.transform.localRotation = Quaternion.Euler(0, -90, 0);
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
				}
				mAni.SetBool("moving",true);
				mBody.transform.localRotation = Quaternion.Euler(0, 0, 0);
				mBody.transform.localScale = new Vector3(1, 1, 1);
				transform.Translate(Vector3.forward * moveSpeed2 * Time.deltaTime, Space.World);
			}
			//if(!Input.GetKey(mUpKey) && !Input.GetKey(mRightKey) && !Input.GetKey(mLeftKey) && !Input.GetKey(mDownKey))

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
		if ((pos.x < xMin) || (pos.x > xMax))
		{
			Kill();
			return;
		}

		if (!Is2D)
		{
			//pos.z = Mathf.Clamp(pos.z, zMin, zMax);
		}
		else
		{
			pos.z = Mathf.Clamp(pos.z, 0, 0);
		}
		pos.x = Mathf.Clamp(pos.x, xMin, xMax);
		transform.position = pos;
	}

	void Dash ()
	{
		AudioManager.Instance.PlaySoundOnce(mDash);
		mCrossT = Time.time + ((9 / 24.0f) / 1f) - 0.5f;
		mAni.SetTrigger ("Dash");
		mDashT = Time.time + mDashDuration;
		mDashing = true;
		dathshit.SetActive(true);
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
		dathshit.SetActive(false);
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
		mGotDashHit = false;
	}
	
	public bool IsDead ()
	{
		return mIsDead;
	}
	
	public void Kill()
	{
		Vector3 possdf = new Vector3(0, -0.5f, 0);
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), Vector3.down, out hitInfo, 10, LayerMask.GetMask("Ground")))
		{
			possdf = hitInfo.point + new Vector3(0, 0.01f, 0);
		}

		GameObject splatooon = Instantiate(mSplaterParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		Destroy (splatooon, 10);
		splatooon.transform.position = possdf;
		splatooon.transform.rotation = mSplaterParticlesPrefab.transform.rotation;
		splatooon.transform.localScale = mSplaterParticlesPrefab.transform.localScale;

		GameObject splatooon2 = Instantiate(mRingParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		Destroy (splatooon2, 10);
		splatooon2.transform.position = transform.position + mRingParticlesPrefab.transform.position;
		splatooon2.transform.rotation = mRingParticlesPrefab.transform.rotation;
		splatooon2.transform.localScale = mRingParticlesPrefab.transform.localScale;
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
			if (coll.relativeVelocity.magnitude > 0.5f) 
			{
				return;
			}

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

			mGotDashHit = true;
		}
		else if (coll.collider.tag == "Knife") 
		{
			AudioManager.Instance.PlaySoundOnce(mKnifeBodyHit);
			
			if (GameStarter.Instance.MasterChef().IsKnifeSide()) 
			{
				SpawnParts(true, false);
			}
			else 
			{
				SpawnParts(false, false);
			}
			Kill();
		}
	}
	
	void OnCollisionStay(Collision coll)
	{
		if (coll.collider.tag == "Stove") 
		{
			SpawnParts((Random.value > 0.5f), false);
			Kill();
		}
	}

	void SpawnParts (bool side, bool all)
	{
		Vector3 prevScale = mBody.transform.localScale;
		mBody.transform.localScale = Vector3.one;
		GameObject rb1 = Instantiate(mPartPrefab, transform.position, transform.rotation) as GameObject;
		GameObject rb2 = Instantiate(mPartPrefab, transform.position, transform.rotation) as GameObject;
		GameObject part10;
		GameObject part11;
		GameObject part20;
		GameObject part21;
		GameObject blast1 = null;
		GameObject blast2 = null;
		GameObject blast3 = null;

		if (all) 
		{
			part10 = Instantiate(mBodyParts2[0], mBodyParts[0].transform.position, mBodyParts[0].transform.rotation) as GameObject;
			part11 = Instantiate(mBodyParts2[1], mBodyParts[1].transform.position, mBodyParts[1].transform.rotation) as GameObject;
			part20 = Instantiate(mBodyParts2[2], mBodyParts[2].transform.position, mBodyParts[2].transform.rotation) as GameObject;
			part21 = Instantiate(mBodyParts2[3], mBodyParts[3].transform.position, mBodyParts[3].transform.rotation) as GameObject;
			
			if (mType == FruitType.Carrot) 
			{
				blast1 = Instantiate(mBodyParts2Bast[0], mBodyPartsBast[0].transform.position, mBodyPartsBast[0].transform.rotation) as GameObject;
				blast2 = Instantiate(mBodyParts2Bast[1], mBodyPartsBast[1].transform.position, mBodyPartsBast[1].transform.rotation) as GameObject;
				blast3 = Instantiate(mBodyParts2Bast[2], mBodyPartsBast[2].transform.position, mBodyPartsBast[2].transform.rotation) as GameObject;
			}
		}
		else if (side) 
		{
			part10 = Instantiate(mBodyParts2[0], mBodyParts[0].transform.position, mBodyParts[0].transform.rotation) as GameObject;
			part11 = Instantiate(mBodyParts2[1], mBodyParts[1].transform.position, mBodyParts[1].transform.rotation) as GameObject;
			part20 = Instantiate(mBodyParts2[2], mBodyParts[2].transform.position, mBodyParts[2].transform.rotation) as GameObject;
			part21 = Instantiate(mBodyParts2[3], mBodyParts[3].transform.position, mBodyParts[3].transform.rotation) as GameObject;

			if (mType == FruitType.Carrot) 
			{
				blast1 = Instantiate(mBodyParts2Bast[0], mBodyPartsBast[0].transform.position, mBodyPartsBast[0].transform.rotation) as GameObject;
				blast2 = Instantiate(mBodyParts2Bast[1], mBodyPartsBast[1].transform.position, mBodyPartsBast[1].transform.rotation) as GameObject;
				blast3 = Instantiate(mBodyParts2Bast[2], mBodyPartsBast[2].transform.position, mBodyPartsBast[2].transform.rotation) as GameObject;
			}
		}
		else 
		{
			part10 = Instantiate(mBodyParts2[1], mBodyParts[1].transform.position, mBodyParts[1].transform.rotation) as GameObject;
			part11 = Instantiate(mBodyParts2[2], mBodyParts[2].transform.position, mBodyParts[2].transform.rotation) as GameObject;
			part20 = Instantiate(mBodyParts2[3], mBodyParts[3].transform.position, mBodyParts[3].transform.rotation) as GameObject;
			part21 = Instantiate(mBodyParts2[0], mBodyParts[0].transform.position, mBodyParts[0].transform.rotation) as GameObject;
			
			if (mType == FruitType.Carrot) 
			{
				blast1 = Instantiate(mBodyParts2Bast[0], mBodyPartsBast[0].transform.position, mBodyPartsBast[0].transform.rotation) as GameObject;
				blast2 = Instantiate(mBodyParts2Bast[1], mBodyPartsBast[1].transform.position, mBodyPartsBast[1].transform.rotation) as GameObject;
				blast3 = Instantiate(mBodyParts2Bast[2], mBodyPartsBast[2].transform.position, mBodyPartsBast[2].transform.rotation) as GameObject;
			}
		}

		rb1.transform.position = (part10.transform.position + part11.transform.position) / 2;
		rb2.transform.position = (part20.transform.position + part21.transform.position) / 2;

		part10.AddComponent<MeshCollider> ().convex = true;
		part11.AddComponent<MeshCollider> ().convex = true;
		part20.AddComponent<MeshCollider> ().convex = true;
		part21.AddComponent<MeshCollider> ().convex = true;
		part10.transform.parent = rb1.transform;
		part11.transform.parent = rb1.transform;
		part20.transform.parent = rb2.transform;
		part21.transform.parent = rb2.transform;

		Material refMat;
		Material otherMat;
		if (mType == FruitType.Carrot) 
		{
			refMat = carrotMaterial;
			otherMat = carrotOtherMaterial;
		}
		else 
		{
			refMat = onionMaterial;
			otherMat = onionOtherMaterial;
		}

		if ((mPlayerID == 2) || (mPlayerID == 3)) 
		{
			part10.GetComponent<MeshRenderer>().material = otherMat;
			part11.GetComponent<MeshRenderer>().material = otherMat;
			part20.GetComponent<MeshRenderer>().material = otherMat;
			part21.GetComponent<MeshRenderer>().material = otherMat;
		}
		else 
		{
			part10.GetComponent<MeshRenderer>().material = refMat;
			part11.GetComponent<MeshRenderer>().material = refMat;
			part20.GetComponent<MeshRenderer>().material = refMat;
			part21.GetComponent<MeshRenderer>().material = refMat;
		}

		if (blast1 != null) 
		{
			blast1.AddComponent<MeshCollider> ().convex = true;
			blast2.AddComponent<MeshCollider> ().convex = true;
			blast3.AddComponent<MeshCollider> ().convex = true;
			blast1.transform.parent = rb1.transform;
			blast2.transform.parent = rb1.transform;
			blast3.transform.parent = rb1.transform;
		}

		rb1.transform.localScale = prevScale;
		rb2.transform.localScale = prevScale;

		if (all) 
		{
			part20.transform.parent = rb1.transform;
			part21.transform.parent = rb1.transform;
			Destroy (rb2);
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
