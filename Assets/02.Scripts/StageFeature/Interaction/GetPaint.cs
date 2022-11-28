using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPaint : PickUpPackage
{
	public bool isReal = false;
	[SerializeField]
	private Collider2D paintCol;
	[SerializeField]
	private Collider2D packCol;

	protected override void Start()
	{
		base.Start();
		mCollider = paintCol;
		packCol.enabled = false;
	}

	public override void OnInteraction()
	{
		if (isReal)
		{
			base.OnInteraction();
			transform.localScale = new Vector3(0.5f, 0.5f);
			mCollider = packCol;
			paintCol.enabled = false;
		}
		else
		{
			canInteract = false;
		}
	}
}
