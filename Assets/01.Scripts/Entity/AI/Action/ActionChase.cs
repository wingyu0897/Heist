using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChase : AIAction
{
	public override void EnterAction()
	{

	}

	public override void TakeAction()
	{
		Vector2 direction = brain.Target.position - transform.position;

		brain.MoveTo(direction, brain.Target.position);
	}

	public override void ExitAction()
	{

	}
}
