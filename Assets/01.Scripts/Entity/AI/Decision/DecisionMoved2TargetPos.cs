using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 목표 지점까지 이동했을 경우
/// </summary>
public class DecisionMoved2TargetPos : AIDecision
{
	public override bool Result()
	{
		if (Vector2.Distance(brain.BasePosition.position, brain.TargetPos) < 1f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
