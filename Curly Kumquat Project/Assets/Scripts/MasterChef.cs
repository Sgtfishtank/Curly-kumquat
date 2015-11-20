
using UnityEngine;
using System.Collections;

public class MasterChef : MonoBehaviour
{
	enum attacks {chop = 0, chopShove = 1, chopSwipe = 2, trippleChop = 3};
	float mCooldown;


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch (Random.Range(0,4))
		{
		case 0:
			Chop();
			break;
		case 1:
			ChopShove();
			break;
		case 2:
			ChopSwipe();
			break;
		case 3:
			TrippleChop();
			break;
		default:
			break;
		}		
	}
	void Chop()
	{

	}
	void ChopShove()
	{

	}
	void ChopSwipe()
	{

	}
	void TrippleChop()
	{

	}

}
