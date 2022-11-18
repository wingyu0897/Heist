using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScout : AIAction
{
	[SerializeField] private float scoutDelayMin;
	[SerializeField] private float scoutDelayMax;
	[SerializeField] private List<Vector2Int> scoutPoints = new List<Vector2Int>();
	private Vector3 basePosition;

	private Vector2Int currentTargetPoint;
	private bool move = true;

	private void Start()
	{
		basePosition = brain.BasePosition.position;
	}

	public override void EnterAction()
	{
		move = false;
		StartCoroutine(SelectNextPoint());
	}

	public override void TakeAction()
	{
		//목표 지점과의 거리가 0.5보다 멀면서 move가 활성화 되어있을 때 목표 지점으로 이동
		if (Vector2.Distance(brain.BasePosition.position, currentTargetPoint) > 0.5f && move == true)
		{
			brain.MoveByNode(currentTargetPoint, Vector2.zero);
		}
		else if (move == true) //목표 지점에 도달했으며 move가 활성화 되어있을 때 다음 목표 지점 선택
		{
			move = false;
			StartCoroutine(SelectNextPoint());
		}
		else //move가 비활성화 되어있을 때 멈춤
		{
			brain.MoveByDirection(Vector2.zero, Vector2.zero);
		}
	}

	public override void ExitAction()
	{
		StopAllCoroutines();
	}

	IEnumerator SelectNextPoint() //다음 위치를 랜덤으로 지정
	{
		yield return new WaitForSeconds(Random.Range(scoutDelayMin, scoutDelayMax));

		if (scoutPoints.Count != 0)
		{
			currentTargetPoint = new Vector2Int(Mathf.RoundToInt(basePosition.x), Mathf.RoundToInt(basePosition.y)) + scoutPoints[Random.Range(0, scoutPoints.Count)];
			move = true;
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (brain == null)
		{
			brain = transform.parent.parent.GetComponent<AIBrain>();
		}
		Vector2 basePosition = !Application.isPlaying ? brain.BasePosition.position : this.basePosition;

		for (int i = 0; i < scoutPoints.Count; i++)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(basePosition + scoutPoints[i], 0.5f);
			Gizmos.DrawLine(basePosition + scoutPoints[i], basePosition + scoutPoints[i == 0 ? scoutPoints.Count - 1 : i - 1]);
		}
	}
#endif
}
