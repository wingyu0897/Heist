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
	private FindPlayerView fpv;
	[HideInInspector]
	public WeaponManager weaponManager;

	[Header("Properties")]
	public bool isNotice = false;
	public bool canTransition = true;

	[SerializeField] 
	private Transform basePosition;
	public Transform BasePosition => basePosition;
	[SerializeField] 
	private Transform target;
	public Transform Target { get => target; set => target = value; }
	[SerializeField] 
	private Vector2 targetPos;
	public Vector2 TargetPos { get => targetPos; set => targetPos = value; }
	[SerializeField] 
	private LayerMask obstacleLayer;
	public LayerMask ObstacleLayer => obstacleLayer;
	public Gun currentGun;
	public bool isPlayerInView = false;
	public bool IsPlayerInView { get => isPlayerInView; set => isPlayerInView = value; }
	public bool canShootPlayer;
	private bool isDead = false;
	public bool IsDead { get => isDead; set => isDead = value; }
	private float detectiveGauge = 0f;
	public float DetectiveGauge { get => detectiveGauge; set => detectiveGauge = value; }
	private Enemy enemy;
	public Enemy Enemy => enemy;

	[Header("Events")]
	public UnityEvent<Vector2> OnMovementKeyPress;
	public UnityEvent<Vector2> OnPointerPositionChanged;
	public UnityEvent<Vector2> OnFindPlayer;
	public UnityEvent OnAttackButtonPressed;
	public UnityEvent OnAttackButtonReleased;
	public UnityEvent OnReloadWeapon;

	private void Start()
	{
		enemy = GetComponent<Enemy>();
		pathFinding = GetComponent<AIPathFinding>();
		fpv = GetComponent<FindPlayerView>();
		weaponManager = transform.Find("WeaponHolder")?.GetComponent<WeaponManager>();
		target = StageManager.Instance.player.transform;
		currentAction?.StartState();
	}

	private void Update()
	{
		fpv.FindViewTargets(); //시야 내 플레이어 감지
		currentAction?.UpdateState(); //현재 액션을 계속 실행한다
		if (StageManager.Instance.isLoud || StageManager.Instance.isDetected)
		{
			Notice();
		}

		if (currentGun != weaponManager?.currentWeapon)
		{
			currentGun = weaponManager?.currentWeapon as Gun;
		}

		if (isPlayerInView || isNotice)
		{
			StageManager.Instance?.detectManager.DetectInput(detectiveGauge);
		}
	}

	public void ChangeState(AIState nextAction) //액션을 받아 전환하는 함수
	{
		currentAction?.EndState();	 //상태 퇴장 함수 실행 (1회)
		currentAction = nextAction;	 //상태 변경
		currentAction?.StartState(); //상태 입장 함수 실행 (1회)
	}

	public void MoveByDirection(Vector2 moveDirection, Vector2 aimPosition) //목표를 향하여 직선으로 이동, 장애물 판단 안 함
	{
		OnMovementKeyPress?.Invoke(moveDirection);
		if (aimPosition != Vector2.zero)
		{
			if (weaponManager)
			{
				if (!isNotice)
				{
					weaponManager.basePosition = basePosition;
				}
				else
				{
					weaponManager.basePosition = weaponManager.transform;
				}
			}
			OnPointerPositionChanged?.Invoke(aimPosition);
		}
	}

	public void MoveByNode(Vector2Int targetPos, Vector2 point) //목표를 향하여 길찾기 방법을 사용하여 이동
	{
		pathFinding.MoveToTarget(targetPos, point);
	}

	public void AimAtTarget(Vector2 targetPoint)
	{
		if (weaponManager)
		{
			weaponManager.basePosition = weaponManager.transform;
		}
		OnFindPlayer?.Invoke(targetPoint);
	}

	public void StartAttack()
	{
		OnAttackButtonPressed?.Invoke();
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
		isDead = true;

		StopAllCoroutines();
		PoolManager.Instance.Push(enemy);
	}

	public void Notice()
	{
		if (!isNotice)
		{
			detectiveGauge = StageManager.Instance.detectTime;
			isNotice = true;
			StartCoroutine(Report());
		}
	}

	IEnumerator Report()
	{
		yield return new WaitForSeconds(4f);

		StageManager.Instance.Louded();
		print("Reported!");
	}
}
