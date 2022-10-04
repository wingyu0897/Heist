using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScout : AIAction
{
	[SerializeField] private float scoutDelayMin;
	[SerializeField] private float scoutDelayMax;
	[SerializeField] private List<Vector2Int> scoutPoints = new List<Vector2Int>();

	private Vector2Int currentTargetPoint;
	private bool move = true;

	private void Start()
	{
		currentTargetPoint = scoutPoints[0];
	}

	public override void EnterAction()
	{
		currentTargetPoint = scoutPoints[0];
	}

	public override void TakeAction()
	{
		//목표 지점과의 거리가 0.5보다 멀면서 move가 활성화 되어있을 때 목표 지점으로 이동
		if (Vector2.Distance(brain.BasePosition.position, currentTargetPoint) > 0.5f && move == true)
		{
			brain.MoveToTarget(currentTargetPoint, Vector2.zero);
		}
		else if (move == true) //목표 지점에 도달했으며 move가 활성화 되어있을 때 다음 목표 지점 선택
		{
			move = false;
			StartCoroutine(SelectNextPoint());
		}
		else //move가 비활성화 되어있을 때 멈춤
		{
			brain.MoveTo(Vector2.zero, Vector2.zero);
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

		move = true;
	}
}
