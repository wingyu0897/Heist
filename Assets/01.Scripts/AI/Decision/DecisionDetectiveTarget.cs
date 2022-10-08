using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어를 발견했을 경우
/// </summary>
public class DecisionDetectiveTarget : AIDecision
{
	public override bool DecisionResult()
	{
		if (brain.DetectiveGauge > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
