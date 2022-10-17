using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionLouded : EscapeCondition
{
	public override bool Result()
	{
		return MissionData.Instance.isLoud;
	}
}
