using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPackageMission : Missions
{
	[SerializeField]
	private string text;
	public override string Text => $"{text}";
	[SerializeField]
	private PackageHolder packHolder;
	[SerializeField]
	private PickUpPackage targetPackage;

	public override bool Condition()
	{
		return packHolder.packs.Contains(targetPackage);
	}
}
