using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionNotNoticePlayer : AIDecision
{
	public override bool Result()
	{
		if (!brain.isNotice == true)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
