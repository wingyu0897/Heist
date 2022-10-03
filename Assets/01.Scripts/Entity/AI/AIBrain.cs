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
		currentAction?.UpdateState(); //현재 액션을 계속 실행한다
	}

	public void ChangeState(AIState nextAction) //액션을 받아 전환하는 함수
	{
		currentAction?.EndState();	 //상태 퇴장 함수 실행 (1회)
		currentAction = nextAction;	 //상태 변경
		currentAction?.StartState(); //상태 입장 함수 실행 (1회)
	}

	public void MoveTo(Vector2 direction, Vector2 targetPos) //움직임 실행 함수 (움직이는 방향, 바라보는 방향)
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
