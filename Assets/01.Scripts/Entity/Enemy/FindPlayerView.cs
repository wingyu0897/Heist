using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FindPlayerView : MonoBehaviour
{
    [SerializeField] private bool debugMode = false;
    private AIBrain brain;

    [Header("View Config")]
    [Range(0f, 360f)]
    [SerializeField] private float horizontalViewAngle = 0f;
    [SerializeField] private float viewRadius = 1f;
    [Range(-180f, 180f)]
    [SerializeField] private float viewRotateZ = 0f;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask obstacleLayerMask;

    private float horizontalViewHalfAngle = 0f;

    [Header("Detect Player")]
    [SerializeField] private float detectTime;
    private bool findPlayer = false;

    public UnityEvent OnDetectionPlayer;

	private void Awake()
	{
		horizontalViewHalfAngle = horizontalViewAngle * 0.5f;
        brain = GetComponent<AIBrain>();
    }

	private void Update()
	{
		FindViewTargets();
	}

	private void FindViewTargets() //플레이어 감지 함수
	{
        Vector2 originPos = brain.BasePosition.position;
        Collider2D hitedTargets = Physics2D.OverlapCircle(originPos, viewRadius, playerLayerMask); //OverlapCircle을 이용해 범위 내 적을 감지
        findPlayer = false;

        if (hitedTargets != null) //범위 내에 적(플레이어)이 있을 경우
		{
            Vector2 targetPos = hitedTargets.transform.position;
            Vector2 dir = (targetPos - originPos).normalized;
            float dirAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + viewRotateZ - 90;
            float angle = Vector3.Angle(AngleToDirZ(viewRotateZ), dir);

            Debug.DrawRay(originPos, AngleToDirZ(-dirAngle + viewRotateZ + 5f) * Vector2.Distance(originPos, targetPos), Color.green);
            Debug.DrawRay(originPos, AngleToDirZ(-dirAngle + viewRotateZ - 5f) * Vector2.Distance(originPos, targetPos), Color.green);

            if (Mathf.Abs(angle) <= horizontalViewHalfAngle)
			{
                RaycastHit2D rayHitedTarget = Physics2D.Raycast(originPos, AngleToDirZ(-dirAngle + viewRotateZ), viewRadius + 1, playerLayerMask);
                RaycastHit2D rayHitedObstacle = Physics2D.Raycast(originPos, AngleToDirZ(-dirAngle + viewRotateZ), rayHitedTarget.distance, obstacleLayerMask);

                if (rayHitedObstacle)
				{
                    Debug.DrawLine(originPos, rayHitedObstacle.point, Color.red);
                    findPlayer = false;
                }
				else
				{
					Debug.DrawLine(originPos, rayHitedTarget.point, Color.green);
                    findPlayer = true;
                }
            }
		}

        FindPlayer();
	}

    private void FindPlayer()
	{
        if (findPlayer == true)
		{
            brain.DetectiveGauge += Time.deltaTime;
		}
		else
		{
            brain.DetectiveGauge -= Time.deltaTime;
		}

        brain.DetectiveGauge = Mathf.Clamp(brain.DetectiveGauge, 0, detectTime);

        if (brain.DetectiveGauge == detectTime)
		{
            brain.FindPlayer = true;
		}
	}

    public void LookAt(Vector2 targetPos)
	{
        Vector2 dir = (targetPos - (Vector2)brain.BasePosition.position).normalized;
        viewRotateZ = -(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
    }

    private Vector3 AngleToDirZ(float degreeAngle)
	{
        float radian = (degreeAngle - transform.eulerAngles.z) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), Mathf.Cos(radian), 0f);
	}

	private void OnDrawGizmos()
	{
        if (debugMode)
        {
            horizontalViewHalfAngle = horizontalViewAngle * 0.5f;

            Vector3 originPos = transform.GetComponent<AIBrain>().BasePosition.position;

            Gizmos.DrawWireSphere(originPos, viewRadius);

            Vector3 horizontalRightDir = AngleToDirZ(-horizontalViewHalfAngle + viewRotateZ);
            Vector3 horizontalLeftDir = AngleToDirZ(horizontalViewHalfAngle + viewRotateZ);
            Vector3 lookDir = AngleToDirZ(viewRotateZ);

            Debug.DrawRay(originPos, horizontalLeftDir * viewRadius, Color.cyan);
            Debug.DrawRay(originPos, lookDir * viewRadius, Color.green);
            Debug.DrawRay(originPos, horizontalRightDir * viewRadius, Color.cyan);
        }
    }
}
