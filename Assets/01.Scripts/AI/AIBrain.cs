using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIBrain : MonoBehaviour
{
	[Header("AI")]
	public AIState currentAction;
	private AIPathFinding pathFinding;
	public AIState CurrentAction { get => currentAction; set => currentAction = value; }

	[Header("Properties")]
	public bool isNotice = false;
	public bool canTransition = true;

	[SerializeField] private LayerMask obstacleLayer;
	[SerializeField] private Transform basePosition;
	[SerializeField] private Transform target;
	[SerializeField] private Vector2 targetPos;
	private Enemy enemy;
	private float detectiveGauge = 0f;
	private bool isPlayerInView = false;
	public LayerMask ObstacleLayer => obstacleLayer;
	public Transform BasePosition => basePosition;
	public Transform Target { get => target; set => target = value; }
	public Vector2 TargetPos { get => targetPos; set => targetPos = value; }
	public Enemy Enemy => enemy;
	public float DetectiveGauge { get => detectiveGauge; set => detectiveGauge = value; }
	public bool IsPlayerInView { get => isPlayerInView; set => isPlayerInView = value; }

	[Header("Events")]
	public UnityEvent<Vector2> OnMovementKeyPress;
	public UnityEvent<Vector2> OnPointerPositionChanged;
	public UnityEvent<Vector2> OnFindPlayer;
	public UnityEvent OnAttackButtonPressed;
	public UnityEvent OnAttackButtonReleased;
	public UnityEvent OnReloadWeapon;

	private void Awake()
	{
		enemy = GetComponent<Enemy>();
		pathFinding = GetComponent<AIPathFinding>();
		currentAction?.StartState();
	}

	private void Update()
	{
		currentAction?.UpdateState(); //���� �׼��� ��� �����Ѵ�
		isNotice = GameManager.instance.isDetected ? true : isNotice;
		if (isNotice || GameManager.instance.isDetected) print("Notice!");
	}

	public void ChangeState(AIState nextAction) //�׼��� �޾� ��ȯ�ϴ� �Լ�
	{
		currentAction?.EndState();	 //���� ���� �Լ� ���� (1ȸ)
		currentAction = nextAction;	 //���� ����
		currentAction?.StartState(); //���� ���� �Լ� ���� (1ȸ)
	}

	public void MoveByDirection(Vector2 direction, Vector2 targetPos) //��ǥ�� ���Ͽ� �������� �̵�, ��ֹ� �Ǵ� �� ��
	{
		OnMovementKeyPress?.Invoke(direction);
		if (targetPos != Vector2.zero)
		{
			OnPointerPositionChanged?.Invoke(targetPos);
		}
	}

	public void MoveByNode(Vector2Int targetPos, Vector2 point) //��ǥ�� ���Ͽ� ��ã�� ����� ����Ͽ� �̵�
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

	public void Dead()
	{
		Destroy(gameObject);
	}

	public void Notice()
	{

		if (!GameManager.instance.isDetected && !isNotice)
		{
			isNotice = true;
			StartCoroutine(Report());
		}
	}

	IEnumerator Report()
	{
		yield return new WaitForSeconds(4f);

		GameManager.instance.isDetected = true;
		print("Reported!");
	}
}
