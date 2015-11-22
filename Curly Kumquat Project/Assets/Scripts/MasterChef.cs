using UnityEngine;
using System.Collections;

public class MasterChef : MonoBehaviour
{
	public enum attacks {chop = 0, chopShove = 1, Swipe = 2, trippleChop = 3, AttackSize};
	public enum state {pick = 0, execute = 1, reset = 2, idel = 3};
	float mCooldown;
	public attacks currentAttack;
	public state currentState;
	public float Limits;
	float targetPos;
	int tripchopdir;
	int toches;
	float pos;
	public float shoveSpeed;
	public float swipeSpeed;
	public GameObject chopWarning;
	public GameObject chopShoveWarning;
	public GameObject swipeWarning;
	public GameObject trippleChopWarning;
	public GameObject knifePrefab;
	public GameObject knife;
	public bool mFirstHit;

	private FMOD.Studio.EventInstance mKnifeHit;
	private FMOD.Studio.EventInstance mChefComment;
	private FMOD.Studio.EventInstance mKnifeSwoosh;
	private float mSayTime;
	private bool mDidTop;

	void Awake()
	{
		knife = Instantiate<GameObject>(knifePrefab);
	}

	// Use this for initialization
	void Start ()
	{
		mKnifeSwoosh = FMOD_StudioSystem.instance.GetEvent("event:/KnifeSwing/Knifeswing");
		mChefComment = FMOD_StudioSystem.instance.GetEvent("event:/Chef comment/ChefComment");
		mKnifeHit = FMOD_StudioSystem.instance.GetEvent("event:/Knifehit/KnifeHit");
		currentState = state.pick;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (mSayTime < Time.time) 
		{
			AudioManager.Instance.PlaySoundOnce(mChefComment);
			mSayTime = Time.time + Random.Range(2.5f, 10.0f);
		}

		if(currentState == state.pick && Time.time > mCooldown)
		{
			mFirstHit = false;
			pos = Random.Range(-Limits,Limits+0.1f);
			knife.transform.position = new Vector3(pos, transform.position.y, transform.position.z);
			currentState = state.idel;
			switch ((attacks)UnityEngine.Random.Range(0, (int)attacks.AttackSize))
			{
			case attacks.chop:
				currentAttack = attacks.chop;
				knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, 120* Time.deltaTime);
				//Destroy( Instantiate(chopWarning,new Vector3(pos,0,0),Quaternion.identity),1.1f);
				Invoke("Chop" ,0.1f);
				break;
			case attacks.chopShove:
				currentAttack = attacks.chopShove;
				targetPos = (Random.Range(0,2)*2-1) * Limits;
				knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, 120* Time.deltaTime);
				Invoke("ChopShove" ,0.1f);
				break;
			case attacks.Swipe:
				knife.transform.position = new Vector3(0, transform.position.y, transform.position.z);
				currentAttack = attacks.Swipe;
				targetPos = (Random.Range(0,2)*2-1) * Limits;
				knife.transform.rotation = Quaternion.Euler(0,359,0);
				Invoke("Swipe" ,0.1f);
				break;
			case attacks.trippleChop:
				knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, 120* Time.deltaTime);
				tripchopdir = Random.Range(0,2)*2-1;
				currentAttack = attacks.trippleChop;
				targetPos = (Random.Range(0,2)*2-1);
				Invoke("TrippleChop" ,0.1f);
				break;
			default:
				Debug.LogError("Error currentAttack " + currentAttack);
				break;
			}
		}

		if(currentState == state.execute)
		{
			switch (currentAttack)
			{
			case attacks.chop:
				Chop();
				break;
			case attacks.chopShove:
				ChopShove();
				break;
			case attacks.Swipe:
				Swipe();
				break;
			case attacks.trippleChop:
				TrippleChop();
				break;
			default:
				Debug.LogError("Error currentAttack " + currentAttack);
				break;
			}
		}

		if(currentState == state.reset)
		{
			if(knife.transform.rotation.eulerAngles.x-180 < 180 && knife.transform.rotation.eulerAngles.x-180 > 0)
				knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, -120 * Time.deltaTime);
			else
			{
				knife.transform.rotation = Quaternion.Euler(0,0,90);
				mCooldown = Random.Range(3,10) + Time.time;
				currentState = state.pick;
				//currentAttack = -1;
			}
		}
	}

	public void Reset ()
	{
		// reset state
		mFirstHit = false;
		knife.transform.position = Vector3.zero;
		knife.transform.rotation = Quaternion.Euler(0,0, 270);
		currentState = state.pick;
	}

	void Chop()
	{
		currentState = state.execute;
		if(knife.transform.rotation.eulerAngles.x > 270)
			knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, 120 * Time.deltaTime);
		else
		{
			if (!mFirstHit) 
			{
				AudioManager.Instance.PlaySoundOnce(mKnifeHit);
				mFirstHit = true;
			}
			knife.transform.rotation = Quaternion.Euler(270,0,90);
			currentState = state.idel;
			Invoke("idel", 0.5f);
		}
	}

	void ChopShove()
	{
		currentState = state.execute;
		if(knife.transform.rotation.eulerAngles.x > 270)
			knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, 180 * Time.deltaTime);
		else if (knife.transform.position.x != targetPos)
		{
			if (!mFirstHit) 
			{
				AudioManager.Instance.PlaySoundOnce(mKnifeHit);
				AudioManager.Instance.PlaySoundOnce(mKnifeSwoosh);
				mFirstHit = true;
			}
			knife.transform.position = Vector3.MoveTowards(knife.transform.position,new Vector3(targetPos,transform.position.y,transform.position.z),shoveSpeed*Time.deltaTime);
		}
		else
		{
			if (!mFirstHit) 
			{
				AudioManager.Instance.PlaySoundOnce(mKnifeHit);
				AudioManager.Instance.PlaySoundOnce(mKnifeSwoosh);
				mFirstHit = true;
			}
			currentState = state.idel;
			Invoke("idel", 1f);
		}
	}

	void Swipe()
	{
		if (!mFirstHit) 
		{
			//AudioManager.Instance.PlaySoundOnce(mKnifeHit);
			AudioManager.Instance.PlaySoundOnce(mKnifeSwoosh);
			mFirstHit = true;
		}
		currentState = state.execute;
		if(knife.transform.rotation.eulerAngles.y > 180)
			knife.transform.RotateAround(new Vector3(0,0,0), Vector3.down, 180 * Time.deltaTime);
		else
		{
			knife.transform.rotation = Quaternion.Euler(0,0,270);
			currentState = state.pick;
			mCooldown = Random.Range(3,10) + Time.time;
			//Invoke("idel",1f);
		}
	}

	void TrippleChop()
	{
		currentState = state.execute;
		if(knife.transform.rotation.eulerAngles.x > 270)
			knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, 120 * Time.deltaTime);
		else if(choping())
		{
			currentState = state.idel;
			Invoke("idel",1f);
			toches = 0;
		}
	}

	void idel()
	{
		currentState = state.reset;
	}

	bool choping()
	{
		if (!mFirstHit) 
		{
			AudioManager.Instance.PlaySoundOnce(mKnifeHit);
			mFirstHit = true;
			mDidTop = false;
		}

		if(tripchopdir == -1)
		{
			knife.transform.position = new Vector3(knife.transform.position.x + -5 * Time.deltaTime,Mathf.Clamp(5+5*Mathf.Sin(10*Time.time),0,50),knife.transform.position.z);
		}
		else if(tripchopdir == 1)
		{
			knife.transform.position = new Vector3(knife.transform.position.x + 5 * Time.deltaTime,Mathf.Clamp(5+5*Mathf.Sin(10*Time.time),0,50),knife.transform.position.z);
		}

		if ((Mathf.Clamp(5+5*Mathf.Sin(10*Time.time),0,50) <= 0.05f) && (mDidTop))
		{
			AudioManager.Instance.PlaySoundOnce(mKnifeHit);
			toches++;
			mDidTop = false;
		}
		else if ((Mathf.Clamp(5+5*Mathf.Sin(10*Time.time),0,50) > 5f)) 
		{
			mDidTop = true;
		}

		if(toches == 2)
			return true;

		return false;
	}

}
