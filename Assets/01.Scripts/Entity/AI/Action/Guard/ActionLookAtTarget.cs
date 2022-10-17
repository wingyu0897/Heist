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
		brain.MoveByDirection(Vector2.zero, brain.TargetPos);
		brain.AimAtTarget(brain.TargetPos);
	}

	public override void ExitAction()
	{

	}
}
