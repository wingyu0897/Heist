using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIBrain : MonoBehaviour
{
	public AIState currentAction;
	public AIState CurrentAction { get => currentAction; set => currentAction = value; }

	[SerializeField] private Transform basePosition;
	[SerializeField] private Transform target;
	[SerializeField] private Vector2 targetPos;
	public Transform BasePosition { get => basePosition; }
	public Transform Target { get => target; set => target = value; }
	public Vector2 TargetPos { get => targetPos; set => targetPos = value; }

	private Enemy enemy;
	public Enemy Enemy { get => enemy; }
	private float detectiveGauge = 0f;
	public float DetectiveGauge { get => detectiveGauge; set => detectiveGauge = value; }
	private bool isPlayerInView = false;
	public bool IsPlayerInView { get => isPlayerInView; set => isPlayerInView = value; }

	public bool isNotice = false;
	public bool canTransition = true;

	public UnityEvent<Vector2> OnMovementKeyPress;
	public UnityEvent<Vector2> OnPointerPositionChanged;
	public UnityEvent<Vector2> OnFindPlayer;
	public UnityEvent OnAttackButtonPressed;
	public UnityEvent OnAttackButtonReleased;
	public UnityEvent OnReloadWeapon;

	private AIPathFinding pathFinding;

	private void Awake()
	{
		enemy = GetComponent<Enemy>();
		pathFinding = GetComponent<AIPathFinding>();
		basePosition = transform.Find("MovementCollider").transform;
		currentAction?.StartState();
	}

	private void Update()
	{
		currentAction?.UpdateState(); //현재 액션을 계속 실행한다
		isNotice = GameManager.instance.isDetected ? true : isNotice;
		if (isNotice || GameManager.instance.isDetected) print("Notice!");
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

	public void AimAtTarget(Vector2 targetPoint)
	{
		OnFindPlayer?.Invoke(targetPoint);
	}

	public void StartAttack()
	{
		OnAttackButtonPressed?.Invoke();
		GameManager.instance.isDetected = true;
	}

	public void StopAttack()
	{
		OnAttackButtonReleased?.Invoke();
	}

	public void ReloadWeapon()
	{
		OnReloadWeapon?.Invoke();
	}
}
