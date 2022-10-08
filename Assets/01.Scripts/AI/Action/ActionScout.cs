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
		move = true;
		currentTargetPoint = scoutPoints[0];
	}

	public override void TakeAction()
	{
		//��ǥ �������� �Ÿ��� 0.5���� �ָ鼭 move�� Ȱ��ȭ �Ǿ����� �� ��ǥ �������� �̵�
		if (Vector2.Distance(brain.BasePosition.position, currentTargetPoint) > 0.5f && move == true)
		{
			brain.MoveToTarget(currentTargetPoint, Vector2.zero);
		}
		else if (move == true) //��ǥ ������ ���������� move�� Ȱ��ȭ �Ǿ����� �� ���� ��ǥ ���� ����
		{
			move = false;
			StartCoroutine(SelectNextPoint());
		}
		else //move�� ��Ȱ��ȭ �Ǿ����� �� ����
		{
			brain.MoveTo(Vector2.zero, Vector2.zero);
		}
	}

	public override void ExitAction()
	{
		StopAllCoroutines();
	}

	IEnumerator SelectNextPoint() //���� ��ġ�� �������� ����
	{
		currentTargetPoint = scoutPoints[Random.Range(0, scoutPoints.Count)];

		yield return new WaitForSeconds(Random.Range(scoutDelayMin, scoutDelayMax));

		move = true;
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
