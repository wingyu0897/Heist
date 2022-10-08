using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionPlayerNotInView : AIDecision
{
	public override bool DecisionResult()
	{
		if (!brain.IsPlayerInView)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
