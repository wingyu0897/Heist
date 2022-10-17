using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIdle : AIAction
{
	[SerializeField] private float transitionDelay;

	public override void EnterAction()
	{
		StopAllCoroutines();
		brain.canTransition = false;
		StartCoroutine(TransitionDelay());
	}

	public override void TakeAction()
	{
		if (brain.isNotice)
		{
			brain.MoveByDirection(Vector2.zero, brain.Target.position);
		}
		else
		{
			brain.MoveByDirection(Vector2.zero, Vector2.zero);
		}
	}

	public override void ExitAction()
	{
		StopAllCoroutines();
		brain.canTransition = true;
	}

	IEnumerator TransitionDelay()
	{
		yield return new WaitForSeconds(transitionDelay);

		brain.canTransition = true;
	}
}