using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ǥ �������� �̵����� ���
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
