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
		if (Vector2.Distance(brain.Target.position, transform.position) > 3)
		{
			brain.AimAtTarget(brain.Target.position + new Vector3(randomAim, randomAim, 0));
		}
		else
		{
			brain.AimAtTarget(brain.Target.position);
		}
		brain.MoveTo(Vector2.zero, Vector2.zero);

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
			randomAim = Random.Range(-1f, 1f);
		}
		canAttack = true;
		brain.StartAttack();
	}
}
