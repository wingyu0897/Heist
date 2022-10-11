using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionPlayerInView : AIDecision
{
	public override bool DecisionResult()
	{
		if (brain.IsPlayerInView)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
