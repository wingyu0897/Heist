using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCCTV : Enemy
{
    [SerializeField]
    private Observer observer;

	private void Update()
	{
		if (observer == null || observer.brain.IsDead)
		{
			brain.enabled = false;
		}
	}
}
