using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttack : AIAction
{
	[Header("Attack Action Properties")]
	[Range(0, 10f)]
	[SerializeField] private float attackCycleDelay = 0f;
	[Range(0, 10f)]
	[SerializeField] private float delayRandomness = 0f;

	private bool canAttack = true;
	private float randomAim;

	public override void EnterAction()
	{
		canAttack = true;
		StopAllCoroutines();
	}

	public override void TakeAction()
	{
		brain.AimAtTarget(brain.Target.position + new Vector3(randomAim, randomAim, 0));

		if (canAttack)
		{
			canAttack = false;
			StartCoroutine(Attack());
		}
	}

	public override void ExitAction()
	{
	
	}

	IEnumerator Attack()
	{
		yield return new WaitForSeconds(Random.Range(attackCycleDelay - delayRandomness, attackCycleDelay + delayRandomness));

		if (randomAim != 0)
		{
			randomAim = 0;
		}
		else
		{
			randomAim = Random.Range(-2f, 2f);
		}
		canAttack = true;
		brain.StartAttack();
	}
}
