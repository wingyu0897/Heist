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
		if (brain.isNotice)
		{
			brain.MoveByNode(new Vector2Int(Mathf.RoundToInt(brain.Target.position.x), Mathf.RoundToInt(brain.Target.position.y)), brain.Target.position);
		}
		else
		{
			brain.MoveByNode(new Vector2Int(Mathf.RoundToInt(brain.TargetPos.x), Mathf.RoundToInt(brain.TargetPos.y)), brain.TargetPos);
		}
	}

	public override void ExitAction()
	{

	}

	//private void 
}
