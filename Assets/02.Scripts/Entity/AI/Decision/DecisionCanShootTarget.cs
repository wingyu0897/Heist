using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionCanShootTarget : AIDecision
{
	public override bool Result()
	{
		return brain.canShootPlayer;
	}
}
