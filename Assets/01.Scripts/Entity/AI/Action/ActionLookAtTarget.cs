using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLookAtTarget : AIAction
{
	public override void EnterAction()
	{

	}

	public override void TakeAction()
	{
		brain.MoveTo(Vector2.zero, brain.TargetPos);
	}

	public override void ExitAction()
	{

	}
}
