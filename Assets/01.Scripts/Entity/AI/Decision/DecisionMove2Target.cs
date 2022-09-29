using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMove2Target : AIDecision
{
	public override bool DecisionResult()
	{
		if (brain.DetectiveGauge > 2f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
