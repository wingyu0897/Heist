using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCCTV : Enemy
{
    [SerializeField]
    private Observer observer;
	[SerializeField]
	private bool controllByObserver = true;

	private void Update()
	{
		if (controllByObserver && (observer == null || observer.brain.IsDead))
		{
			brain.enabled = false;
		}
	}
}
