using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    [Range(0, 1)]
    [SerializeField] private float lookAtLerp;
    private float horizontalViewHalfAngle = 0f;

    [Header("Detect Player")]
    [SerializeField] private Slider detectiveGaugeSlider;
    [SerializeField] private float detectTime;
    [SerializeField] private float minDistance;
    private bool findPlayer = false;
    private GameObject spotLight;

    public UnityEvent OnDetectionPlayer;

	private void Start()
	{
		horizontalViewHalfAngle = horizontalViewAngle * 0.5f;
        brain = GetComponent<AIBrain>();
        spotLight = transform.Find("Light")?.gameObject;
    }

	private void Update()
	{
		FindViewTargets();
        //detectiveGaugeSlider.value = brain.isNotice ? (brain.DetectiveGauge = detectTime) / detectTime : brain.DetectiveGauge / detectTime;
        //detectiveGaugeSlider?.gameObject.SetActive(brain.DetectiveGauge > 0);
    }

	private void FindViewTargets() //플레이어 감지 함수
	{
        Vector2 originPos = transform.position;
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
                RaycastHit2D rayHitedTarget = Physics2D.Raycast(originPos, dir, viewRadius + 1, playerLayerMask);
                RaycastHit2D rayHitedObstacle = Physics2D.Raycast(originPos, dir, rayHitedTarget.distance, obstacleLayerMask);

                if (rayHitedObstacle)
				{
                    Debug.DrawLine(originPos, rayHitedObstacle.point, Color.red);
                    findPlayer = false;
                }
				else
				{
					Debug.DrawLine(originPos, rayHitedTarget.point, Color.green);
                    brain.TargetPos = brain.Target.position;
                    findPlayer = true;

					if (rayHitedTarget.distance < minDistance)
					{
						brain.Notice();
					}
				}
            }
		}

        FindPlayer();
	}

    private void FindPlayer()
	{
        brain.DetectiveGauge = brain.isNotice ? detectTime : brain.DetectiveGauge;

        if (findPlayer == true)
		{
            brain.DetectiveGauge += Time.deltaTime;
            brain.IsPlayerInView = true;
		}
		else
		{
            brain.DetectiveGauge -= Time.deltaTime * 0.5f;
            brain.IsPlayerInView = false;
		}

        brain.DetectiveGauge = Mathf.Clamp(brain.DetectiveGauge, 0, detectTime);

		if (brain.DetectiveGauge >= detectTime)
		{
			brain.Notice();
		}
	}

    public void LookAt(Vector2 targetPos)
	{
        Vector2 dir = (targetPos - (Vector2)brain.BasePosition.position).normalized;
        float targetAngle = -(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
        viewRotateZ = Mathf.LerpAngle(viewRotateZ, targetAngle, lookAtLerp);
        spotLight.transform.localRotation = Quaternion.Euler(0, 0, -viewRotateZ);
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

            Vector3 originPos = transform.position;

            Gizmos.DrawWireSphere(originPos, viewRadius);

            Vector3 horizontalRightDir = AngleToDirZ(-horizontalViewHalfAngle + viewRotateZ);
            Vector3 horizontalLeftDir = AngleToDirZ(horizontalViewHalfAngle + viewRotateZ);

            Debug.DrawRay(originPos, horizontalLeftDir * viewRadius, Color.cyan);
            Debug.DrawRay(originPos, horizontalRightDir * viewRadius, Color.cyan);
        }
    }
}
