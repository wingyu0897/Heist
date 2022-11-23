using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTaker : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out PickUpPackage package))
		{
			if (!package.isHolding)
			{
				package.canInteract = false;
				package.GetComponent<Collider2D>().enabled = false;

				StageManager.Instance.gains.Add(package);
			}
		}
	}
}
