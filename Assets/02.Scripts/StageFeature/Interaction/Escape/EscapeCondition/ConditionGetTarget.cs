using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionGetTarget : EscapeCondition
{
	[SerializeField]
	private PickUpPackage package;

	public override bool Result()
	{
		return StageManager.Instance.gains.Contains(package);
	}
}
