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
		brain.MoveToTarget(new Vector2Int(Mathf.RoundToInt(brain.TargetPos.x), Mathf.RoundToInt(brain.TargetPos.y)), brain.TargetPos);
	}

	public override void ExitAction()
	{

	}
}
