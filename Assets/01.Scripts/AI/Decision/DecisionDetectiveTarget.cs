using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ �߰����� ���
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
