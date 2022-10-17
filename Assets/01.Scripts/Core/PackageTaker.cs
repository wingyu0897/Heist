using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTaker : MonoBehaviour
{
	//private void GainPackage(Package package)
	//{
	//	MissionData.Instance.gainPackages.Add(package);
	//}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Package package))
		{
			print(package);
			if (!package.isHolding)
			{
				package.canInteract = false;
				package.GetComponent<Collider2D>().enabled = false;

				MissionData.Instance.gainPackages.Add(package);
			}
		}
	}
}
