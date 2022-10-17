using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScoutCCTV : AIAction
{
	[SerializeField] private float scoutDelayMin;
	[SerializeField] private float scoutDelayMax;
	[SerializeField] private List<Vector2Int> scoutPoints = new List<Vector2Int>();

	private Vector2Int currentTargetPoint;
	private bool move = false;

	private void Start()
	{
		currentTargetPoint = scoutPoints[0];
	}

	public override void EnterAction()
	{
		move = false;
		currentTargetPoint = scoutPoints[0];
	}

	public override void TakeAction()
	{
		if (!move)
		{
			move = true;
			StartCoroutine(SelectNextPoint());
		}
		else if (move)
		{
			brain.MoveByDirection(Vector2.zero, currentTargetPoint);
		}
	}

	public override void ExitAction()
	{
		StopAllCoroutines();
	}

	IEnumerator SelectNextPoint() //다음 위치를 랜덤으로 지정
	{
		currentTargetPoint = scoutPoints[Random.Range(0, scoutPoints.Count)];

		yield return new WaitForSeconds(Random.Range(scoutDelayMin, scoutDelayMax));

		move = false;
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < scoutPoints.Count; i++)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine((Vector2)scoutPoints[i], (Vector2)scoutPoints[i == 0 ? scoutPoints.Count - 1 : i - 1]);
		}
	}
}