using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ �߰����� ���
/// </summary>
public class DecisionDetectiveTarget : AIDecision
{
	public override bool Result()
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
