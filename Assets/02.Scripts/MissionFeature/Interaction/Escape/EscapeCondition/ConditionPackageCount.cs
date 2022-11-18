using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionPackageCount : EscapeCondition
{
	[SerializeField] private int count;

	public override bool Result()
	{
		return MissionData.Instance?.gains.Count >= count;
	}
}
