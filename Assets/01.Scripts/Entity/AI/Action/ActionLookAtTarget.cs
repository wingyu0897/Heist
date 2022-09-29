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
		brain.MoveTo(Vector2.zero, brain.Target.position);
	}

	public override void ExitAction()
	{

	}
}
