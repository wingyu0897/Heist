using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackMission : Missions
{
	[SerializeField]
	private string text;
	public override string Text => text;

	public override bool Condition()
	{
		return PlayerData.Instance.backPacks > 0;
	}
}
