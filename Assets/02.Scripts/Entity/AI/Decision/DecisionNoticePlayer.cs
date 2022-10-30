using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 침입을 감지했을 경우 (침입을 알아챔)
/// </summary>
public class DecisionNoticePlayer : AIDecision
{
	public override bool Result()
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
