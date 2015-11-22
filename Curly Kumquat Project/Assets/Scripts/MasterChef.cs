using UnityEngine;
using System.Collections;

public class MasterChef : MonoBehaviour
{
	public enum attacks {chop = 0, chopShove = 1, Swipeleft = 2, SwipeRight = 3,chop1,chop2,chop3,chop4,chop5/*, trippleChop = 4*/, AttackSize};
	public enum state {pick = 0, execute = 1, reset = 2, idel = 3, prep = 4};
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
	public GameObject knifeHitParticePrefab;
	public bool mFirstHit;
	public bool mfirstattack;
	public Animator mAni;

	float attacktime;
	float orgpos;
	private FMOD.Studio.EventInstance mKnifeHit;
	private FMOD.Studio.EventInstance mChefComment;
	private FMOD.Studio.EventInstance mKnifeSwoosh;
	private float mSayTime;
	private bool mDidTop;

	void Awake()
	{
	}

	// Use this for initialization
	void Start ()
	{
		orgpos = transform.position.y;
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
		if (currentState == state.prep && transform.position.x != pos) {
			transform.position = Vector3.MoveTowards(transform.position,new Vector3(pos, transform.position.y, transform.position.z),20*Time.deltaTime);

		}
		else if (currentState == state.prep)
		{
			currentState = state.pick;
		}

		if(currentState == state.pick && Time.time > mCooldown)
		{
			mFirstHit = false;
			mfirstattack = true;
			currentState = state.idel;
			switch ((attacks)UnityEngine.Random.Range(0, (int)attacks.AttackSize))
			{
			case attacks.chop:
				currentAttack = attacks.chop;
				attacktime = Time.time + 1.458f;
				//Destroy( Instantiate(chopWarning,new Vector3(pos,0,0),Quaternion.identity),1.1f);
				Invoke("Chop" ,1f);
				break;
			case attacks.chop1:
				currentAttack = attacks.chop;
				attacktime = Time.time + 1.458f;
				//Destroy( Instantiate(chopWarning,new Vector3(pos,0,0),Quaternion.identity),1.1f);
				Invoke("Chop" ,1f);
				break;
			case attacks.chop2:
				currentAttack = attacks.chop;
				attacktime = Time.time + 1.458f;
				//Destroy( Instantiate(chopWarning,new Vector3(pos,0,0),Quaternion.identity),1.1f);
				Invoke("Chop" ,1f);
				break;
			case attacks.chop3:
				currentAttack = attacks.chop;
				attacktime = Time.time + 1.458f;
				//Destroy( Instantiate(chopWarning,new Vector3(pos,0,0),Quaternion.identity),1.1f);
				Invoke("Chop" ,1f);
				break;
			case attacks.chop4:
				currentAttack = attacks.chop;
				attacktime = Time.time + 1.458f;
				//Destroy( Instantiate(chopWarning,new Vector3(pos,0,0),Quaternion.identity),1.1f);
				Invoke("Chop" ,1f);
				break;
			case attacks.chop5:
				currentAttack = attacks.chop;
				attacktime = Time.time + 1.458f;
				//Destroy( Instantiate(chopWarning,new Vector3(pos,0,0),Quaternion.identity),1.1f);
				Invoke("Chop" ,1f);
				break;
			case attacks.chopShove:
				currentAttack = attacks.chopShove;
				targetPos = (Random.Range(0,2)*2-1);
				attacktime = Time.time + 1.458f;
				Invoke("ChopShove" ,1f);
				break;
			case attacks.Swipeleft:
				currentAttack = attacks.Swipeleft;
				//transform.position = new Vector3(transform.position.x, transform.position.y +1.25f,transform.position.z);
				attacktime = Time.time + 2f;
				Invoke("Swipe" ,1f);
				break;
			case attacks.SwipeRight:
				currentAttack = attacks.SwipeRight;
				attacktime = Time.time + 2f;
				Invoke("Swipe" ,1f);
				break;
			/*case attacks.trippleChop:
				tripchopdir = Random.Range(0,2)*2-1;
				currentAttack = attacks.trippleChop;
				targetPos = (Random.Range(0,2)*2-1);
				Invoke("TrippleChop" ,1f);
				break;*/
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
			case attacks.Swipeleft:
				Swipe();
				break;
			case attacks.SwipeRight:
				Swipe();
				break;
			/*case attacks.trippleChop:
				TrippleChop();
				break;*/
			default:
				Debug.LogError("Error currentAttack " + currentAttack);
				break;
			}
		}

		if(currentState == state.reset)
		{
			if (transform.rotation.eulerAngles.y >= 182 || transform.rotation.eulerAngles.y <= 178) {
				transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y + -targetPos * 90 * Time.deltaTime, 0);
			}
			else
			{
				transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
				mCooldown = Random.Range(0,1)+ Time.time;
				currentState = state.prep;
				pos = Random.Range(-Limits,Limits+0.1f);
				transform.position = new Vector3(transform.position.x, orgpos,transform.position.z);
			}
		}
	}

	void OnEnable()
	{
		mAni.SetInteger("Attack",(int)attacks.AttackSize);
	}
	
	void OnDisable()
	{
		mAni.SetInteger("Attack",(int)attacks.AttackSize);
	}

	public void Reset ()
	{

		// reset state
		mFirstHit = false;
		transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));
		currentState = state.pick;
		mCooldown = 0;
	}

	void Chop()
	{
		currentState = state.execute;
		if (mfirstattack) {
			mAni.SetInteger ("Attack", (int)attacks.chop);
			mfirstattack = false;
		}
		if (Time.time > attacktime) {
			if (!mFirstHit) {
				HitBoard();
				mFirstHit = true;
				currentState = state.idel;
				mAni.SetInteger("Attack",(int)attacks.AttackSize);
				Invoke("idel", 0.5f);
			}
		}
	}

	void ChopShove()
	{
		currentState = state.execute;
		if (mfirstattack) {
			mAni.SetInteger ("Attack", (int)attacks.chop);
			currentState = state.execute;
			mfirstattack = false;
			print (attacktime +" " +Time.time);
		}
		if (attacktime < Time.time && !mFirstHit) {
			print (true);
			if (!mFirstHit) {
				HitBoard ();
				AudioManager.Instance.PlaySoundOnce (mKnifeSwoosh);
				mFirstHit = true;
			}
		}
		else if (transform.rotation.eulerAngles.y >= 125 && transform.rotation.eulerAngles.y <= 235 &&attacktime < Time.time) {
			transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y + targetPos * 22.5f * Time.deltaTime, 0);
		} else if(attacktime < Time.time){
			mAni.SetInteger ("Attack", (int)attacks.AttackSize);
			currentState = state.idel;
			Invoke ("idel", 1f);
		}
}

	void HitBoard()
	{
		AudioManager.Instance.PlaySoundOnce(mKnifeHit);
		GameObject splatooon2 = Instantiate(knifeHitParticePrefab, transform.position, Quaternion.identity) as GameObject;
		//splatooon2.transform.position = knife.transform.position + knifeHitParticePrefab.transform.position;
		splatooon2.transform.rotation = knifeHitParticePrefab.transform.rotation;
		splatooon2.transform.localScale = knifeHitParticePrefab.transform.localScale;
		Destroy (splatooon2, 10);
	}

	void Swipe()
	{

		if (mfirstattack)
		{
			mAni.SetInteger ("Attack", (int)currentAttack);
			mfirstattack = false;
			currentState = state.execute;
		}
		if (!mFirstHit) 
		{
			AudioManager.Instance.PlaySoundOnce(mKnifeSwoosh);
			mFirstHit = true;
		}
		if (Time.time > attacktime)
		{
			currentState = state.pick;
			mAni.SetInteger ("Attack", (int)attacks.AttackSize);
			mCooldown = Random.Range (3, 10) + Time.time;
			transform.position = new Vector3(transform.position.x, orgpos,transform.position.z);
		}
			//Invoke("idel",1f);
	}
	
	void TrippleChop()
	{
		currentState = state.execute;
		mAni.SetInteger ("Attack", (int)attacks.chop);
		if(choping())
		{
			currentState = state.idel;
			Invoke("idel",1f);
			toches = 0;
		}
	}

	public bool IsKnifeSide ()
	{
		if (currentAttack == attacks.Swipeleft || currentAttack == attacks.SwipeRight) 
		{
			return true;
		}

		return false;
	}

	void idel()
	{
		currentState = state.reset;
	}

	bool choping()
	{
		if (!mFirstHit) 
		{
			HitBoard();
			mFirstHit = true;
			mDidTop = false;
		}


		if ((Mathf.Clamp(5+5*Mathf.Sin(10*Time.time),0,50) <= 0.05f) && (mDidTop))
		{
			HitBoard();
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
