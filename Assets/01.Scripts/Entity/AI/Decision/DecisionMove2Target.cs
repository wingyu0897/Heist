using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어를 일정한 시간 이상 탐지했을 경우
/// </summary>
public class DecisionMove2Target : AIDecision
{
	[SerializeField] private float targetTime;

	public override bool DecisionResult()
	{
		if (brain.DetectiveGauge >= targetTime)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
