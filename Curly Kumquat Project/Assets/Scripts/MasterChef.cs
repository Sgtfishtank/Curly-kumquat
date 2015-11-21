using UnityEngine;
using System.Collections;

public class MasterChef : MonoBehaviour
{
	enum attacks {chop = 0, chopShove = 1, Swipe = 2, trippleChop = 3};
	float mCooldown;
	int currentAttack;
	public float Limits;
	float pos;
	bool done;
	public GameObject chopWarning;
	public GameObject chopShoveWarning;
	public GameObject swipeWarning;
	public GameObject trippleChopWarning;
	public GameObject knife;


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(currentAttack == -1 && Time.time > mCooldown)
		{
			switch (UnityEngine.Random.Range(0,4))
			{
			case 0:
				currentAttack = 0;
				pos = Random.Range(-Limits,Limits+0.1f);
				Destroy( Instantiate(chopWarning,new Vector3(pos,0,0),Quaternion.identity),1.1f);
				Invoke("Chop" ,1);
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
		if(currentAttack != -1)
		{
			switch (UnityEngine.Random.Range(0,4))
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
		if(done)
		{
			if(knife.transform.rotation.eulerAngles.z != -90)
				knife.transform.RotateAround(new Vector3(0,0,-5), new Vector3(0,0,1), -30 * Time.deltaTime);
			else
			{
				mCooldown = Random.Range(3,10) + Time.time;
				done = false;
				currentAttack = -1;
			}
		}
	}

	public void Reset ()
	{
		// reset state
	}

	void Chop()
	{
		knife.transform.position = new Vector3(pos, transform.position.y, transform.position.z);
		if(knife.transform.rotation.eulerAngles.z != 0)
			knife.transform.RotateAround(new Vector3(0,0,-5), new Vector3(0,0,1), 30 * Time.deltaTime);
		else
			done = true;

	}
	void ChopShove()
	{

	}
	void Swipe()
	{

	}
	void TrippleChop()
	{

	}

}
