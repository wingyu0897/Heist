using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ ������ �ð� �̻� Ž������ ���
/// </summary>
public class DecisionDetectiveTargetMoreTime : AIDecision
{
	[SerializeField] private float targetTime;

	public override bool Result()
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
