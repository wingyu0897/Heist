using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIdle : AIAction
{
	[SerializeField] private float transitionDelay;

	private float time = 0;

	public override void EnterAction()
	{
		StopAllCoroutines();
		brain.canTransition = false;
		time = 0;
		StartCoroutine(TransitionDelay());
	}

	public override void TakeAction()
	{
		time += Time.deltaTime;

		if (time >= transitionDelay)
		{
			brain.canTransition = true;
		}
		
		brain.MoveByDirection(Vector2.zero, Vector2.zero);
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
