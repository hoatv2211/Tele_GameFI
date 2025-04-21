using System;
using System.Collections;
using SWS;
using UnityEngine;
using UnityEngine.AI;

public class EventReceiver : MonoBehaviour
{
	public void MyMethod()
	{
	}

	public void PrintText(string text)
	{
		UnityEngine.Debug.Log(text);
	}

	public void RotateSprite(float newRot)
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		eulerAngles.y = newRot;
		base.transform.eulerAngles = eulerAngles;
	}

	public void SetDestination(UnityEngine.Object target)
	{
		base.StartCoroutine(this.SetDestinationRoutine(target));
	}

	private IEnumerator SetDestinationRoutine(UnityEngine.Object target)
	{
		NavMeshAgent agent = base.GetComponent<NavMeshAgent>();
		navMove myMove = base.GetComponent<navMove>();
		GameObject tar = (GameObject)target;
		myMove.ChangeSpeed(4f);
		agent.SetDestination(tar.transform.position);
		while (agent.pathPending)
		{
			yield return null;
		}
		float remain = agent.remainingDistance;
		while (remain == float.PositiveInfinity || remain - agent.stoppingDistance > 1.401298E-45f || agent.pathStatus != NavMeshPathStatus.PathComplete)
		{
			remain = agent.remainingDistance;
			yield return null;
		}
		yield return new WaitForSeconds(4f);
		myMove.ChangeSpeed(1.5f);
		myMove.moveToPath = true;
		myMove.StartMove();
		yield break;
	}

	public void ActivateForTime(UnityEngine.Object target)
	{
		base.StartCoroutine(this.ActivateForTimeRoutine(target));
	}

	private IEnumerator ActivateForTimeRoutine(UnityEngine.Object target)
	{
		GameObject tar = (GameObject)target;
		tar.SetActive(true);
		yield return new WaitForSeconds(6f);
		tar.SetActive(false);
		yield break;
	}
}
