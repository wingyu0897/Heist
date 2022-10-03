using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIBrain : MonoBehaviour
{
	public AIState currentAction;
	public AIState CurrentAction { get => currentAction; set => currentAction = value; }

	[SerializeField] private Transform target;
	public Transform Target { get => target; set => target = value; }
	[SerializeField] private Vector2 targetPos;
	public Vector2 TargetPos { get => targetPos; set => targetPos = value; }
	[SerializeField] private Transform basePosition;
	public Transform BasePosition { get => basePosition; }

	private Enemy enemy;
	public Enemy Enemy { get => enemy; }
	private float detectiveGauge = 0f;
	public float DetectiveGauge { get => detectiveGauge; set => detectiveGauge = value; }
	private bool findPlayer = false;
	public bool FindPlayer { get => findPlayer; set => findPlayer = value; }

	public UnityEvent<Vector2> OnMovementKeyPress;
	public UnityEvent<Vector2> OnPointerPositionChanged;
	public UnityEvent OnFireButtonPress;
	public UnityEvent OnFireButtonRelease;

	private AIPathFinding pathFinding;

	private void Awake()
	{
		enemy = GetComponent<Enemy>();
		pathFinding = GetComponent<AIPathFinding>();
		basePosition = transform.Find("MovementCollider").transform;
	}

	private void Update()
	{
		currentAction?.UpdateState(); //���� �׼��� ��� �����Ѵ�
	}

	public void ChangeState(AIState nextAction) //�׼��� �޾� ��ȯ�ϴ� �Լ�
	{
		currentAction?.EndState();	 //���� ���� �Լ� ���� (1ȸ)
		currentAction = nextAction;	 //���� ����
		currentAction?.StartState(); //���� ���� �Լ� ���� (1ȸ)
	}

	public void MoveTo(Vector2 direction, Vector2 targetPos) //������ ���� �Լ� (�����̴� ����, �ٶ󺸴� ����)
	{
		OnMovementKeyPress?.Invoke(direction);
		if (targetPos != Vector2.zero)
		{
			OnPointerPositionChanged?.Invoke(targetPos);
		}
	}

	public void MoveToTarget(Vector2Int targetPos, Vector2 point)
	{
		pathFinding.MoveToTarget(targetPos, point);
	}
}
