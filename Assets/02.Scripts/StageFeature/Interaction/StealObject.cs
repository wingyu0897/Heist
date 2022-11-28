using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealObject : PickUpPackage
{
	[SerializeField]
	private PackageTaker packTaker;


	public override void OnInteraction()
	{
		gameObject.SetActive(false);
		packTaker.Take(mCollider);
	}
}
