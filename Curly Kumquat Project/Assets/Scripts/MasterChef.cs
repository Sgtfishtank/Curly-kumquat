using UnityEngine;
using System.Collections;

public class MasterChef : MonoBehaviour
{
	enum attacks {chop = 0, chopShove = 1, Swipe = 2, trippleChop = 3};
	enum state {pick = 0, execute = 1, reset = 2, idel = 3};
	float mCooldown;
	int currentAttack;
	public int currentState;
	public float Limits;
	float targetPos;
	float pos;
	public float shoveSpeed;
	public float swipeSpeed;
	public GameObject chopWarning;
	public GameObject chopShoveWarning;
	public GameObject swipeWarning;
	public GameObject trippleChopWarning;
	public GameObject knifePrefab;
	public GameObject knife;

	void Awake()
	{
		knife = Instantiate<GameObject>(knifePrefab);
	}

	// Use this for initialization
	void Start ()
	{
		currentState = (int)state.pick;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(currentState == (int)state.pick && Time.time > mCooldown)
		{
			pos = Random.Range(-Limits,Limits+0.1f);
			knife.transform.position = new Vector3(pos, transform.position.y, transform.position.z);
			currentState = (int)state.idel;
			switch (1)//UnityEngine.Random.Range(0,2))
			{
			case (int)attacks.chop:
				currentAttack = (int)attacks.chop;
				knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, 120* Time.deltaTime);
				//Destroy( Instantiate(chopWarning,new Vector3(pos,0,0),Quaternion.identity),1.1f);
				Invoke("Chop" ,0.1f);
				break;
			case (int)attacks.chopShove:
				currentAttack = (int)attacks.chopShove;
				targetPos = (Random.Range(0,2)*2-1) * Limits;
				knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, 120* Time.deltaTime);
				Invoke("ChopShove" ,0.1f);
				break;
			case (int)attacks.Swipe:
				currentAttack = (int)attacks.Swipe;
				targetPos = (Random.Range(0,2)*2-1) * Limits;

				Invoke("Swipe" ,0.1f);

				break;
			case (int)attacks.trippleChop:
				currentAttack = (int)attacks.trippleChop;
				targetPos = (Random.Range(0,2)*2-1);

				Invoke("TrippleChop" ,0.1f);
				break;
			default:
				break;
			}
		}
		if(currentState == (int)state.execute)
		{
			switch (currentAttack)
			{
			case 0:
				Chop();
				break;
			case 1:
				ChopShove();
				break;
			case 2:
				Swipe();
				break;
			case 3:
				TrippleChop();
				break;
			default:
				break;
			}
		}
		if(currentState == (int)state.reset)
		{
			if(knife.transform.rotation.eulerAngles.x-180 < 180 && knife.transform.rotation.eulerAngles.x-180 > 0)
				knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, -120 * Time.deltaTime);
			else
			{
				knife.transform.rotation = Quaternion.Euler(0,0,90);
				mCooldown = Random.Range(3,10) + Time.time;
				currentState =(int)state.pick;
				currentAttack = -1;
			}
		}
	}

	public void Reset ()
	{
		// reset state
		knife.transform.position = Vector3.zero;
		knife.transform.rotation = Quaternion.identity;
		currentState = (int)state.pick;
	}

	void Chop()
	{
		currentState = (int)state.execute;
		if(knife.transform.rotation.eulerAngles.x > 270)
			knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, 120 * Time.deltaTime);
		else
		{
			knife.transform.rotation = Quaternion.Euler(270,0,90);
			currentState = (int)state.idel;
			Invoke("idel", 0.5f);
		}

	}
	void ChopShove()
	{
		currentState = (int)state.execute;
		if(knife.transform.rotation.eulerAngles.x > 270)
			knife.transform.RotateAround(new Vector3(0,0,0), Vector3.left, 120 * Time.deltaTime);
		else if (knife.transform.position.x != targetPos)
		{
			print ("shove");
			knife.transform.position = Vector3.MoveTowards(knife.transform.position,new Vector3(targetPos,transform.position.y,transform.position.z),shoveSpeed*Time.deltaTime);
		}
		else
		{
			currentState = (int)state.idel;
			Invoke("idel", 1f);
		}
	}
	void Swipe()
	{
		currentState = (int)state.execute;



	}
	void TrippleChop()
	{
		currentState = (int)state.execute;


	}
	void idel()
	{
		currentState = (int)state.reset;
	}

}
