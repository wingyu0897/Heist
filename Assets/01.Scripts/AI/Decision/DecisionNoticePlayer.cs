using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾��� ħ���� �������� ��� (ħ���� �˾�è)
/// </summary>
public class DecisionNoticePlayer : AIDecision
{
	public override bool DecisionResult()
	{
		if (brain.isNotice == true)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
