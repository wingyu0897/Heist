using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMoved2TargetPos : AIDecision
{
	public override bool DecisionResult()
	{
		if (Vector2.Distance(brain.BasePosition.position, brain.TargetPos) < 0.5f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
