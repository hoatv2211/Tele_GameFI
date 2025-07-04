using System;
using HedgehogTeam.EasyTouch;
using UnityEngine;

public class TooglePickMethodUI : MonoBehaviour
{
	public void SetPickMethod2Finger(bool value)
	{
		if (value)
		{
			EasyTouch.SetTwoFingerPickMethod(EasyTouch.TwoFingerPickMethod.Finger);
		}
	}

	public void SetPickMethod2Averager(bool value)
	{
		if (value)
		{
			EasyTouch.SetTwoFingerPickMethod(EasyTouch.TwoFingerPickMethod.Average);
		}
	}
}
