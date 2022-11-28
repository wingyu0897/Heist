using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttack : AIAction
{
	[Header("Attack Action Properties")]
	[Range(0, 10f)]
	[SerializeField] 
	private float attackCycleDelay = 0f;
	[SerializeField][Range(0, 1f)]
	private float delayRandomness = 0f;
	[SerializeField]
	private float noRecoilMaxDistance = 5f;

	private bool canAttack = true;
	private float randomAim;

	public override void EnterAction()
	{
		canAttack = true;
		StopAllCoroutines();
	}

	public override void TakeAction()
	{
		if (Vector2.Distance(brain.Target.position, transform.position) > noRecoilMaxDistance)
		{
			Vector3 dir = brain.Target.position + new Vector3(randomAim, randomAim, 0);
			brain.AimAtTarget(dir);
			brain.MoveByDirection(Vector2.zero, dir);
		}
		else
		{
			brain.AimAtTarget(brain.Target.position);
			brain.MoveByDirection(Vector2.zero, brain.Target.position);
		}
		brain.MoveByDirection(Vector2.zero, Vector2.zero);

		if (canAttack && brain.Target.gameObject.activeInHierarchy)
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
		float attackDelay = attackCycleDelay + attackCycleDelay * Random.Range(-delayRandomness, delayRandomness);
		yield return new WaitForSeconds(attackDelay);

		canAttack = true;
		if (randomAim != 0)
		{
			randomAim = 0;
		}
		else
		{
			randomAim = Random.Range(-1f, 1f);
		}
		brain.StopAttack();
		brain.StartAttack();
	}
}
