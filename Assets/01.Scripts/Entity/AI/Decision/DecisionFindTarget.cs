using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionFindTarget : AIDecision
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
