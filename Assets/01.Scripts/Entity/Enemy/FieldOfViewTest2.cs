using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewTest2 : MonoBehaviour
{
    [SerializeField] private bool debugMode = false;

    [Header("View Config")]
    [Range(0f, 360f)]
    [SerializeField] private float horizontalViewAngle = 0f;
    [SerializeField] private float viewRadius = 1f;
    [Range(-180f, 180f)]
    [SerializeField] private float viewRotateZ = 0f;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask obstacleLayerMask;

    private float horizontalViewHalfAngle = 0f;

	private void Awake()
	{
		horizontalViewHalfAngle = horizontalViewAngle * 0.5f;
	}

	private void Update()
	{
		FindViewTargets();
	}

	private void FindViewTargets() //플레이어 감지 함수
	{
        Vector2 originPos = transform.position;
        Collider2D hitedTargets = Physics2D.OverlapCircle(originPos, viewRadius, playerLayerMask); //OverlapCircle을 이용해 범위 내 적을 감지

        if (hitedTargets != null) //범위 내에 적(플레이어)이 있을 경우
		{
            Vector2 targetPos = hitedTargets.transform.position;
            Vector2 dir = (targetPos - originPos).normalized;
            float dirAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + viewRotateZ - 90;

            Debug.DrawRay(originPos, AngleToDirZ(-dirAngle + viewRotateZ + 5f) * Vector2.Distance(originPos, targetPos), Color.green);
            Debug.DrawRay(originPos, AngleToDirZ(-dirAngle + viewRotateZ - 5f) * Vector2.Distance(originPos, targetPos), Color.green);

            if (Mathf.Abs(dirAngle) <= horizontalViewHalfAngle)
			{
                RaycastHit2D rayHitedTarget = Physics2D.Raycast(originPos, dir, viewRadius + 1, playerLayerMask);
                RaycastHit2D rayHitedObstacle = Physics2D.Raycast(originPos, dir, rayHitedTarget.distance, obstacleLayerMask);

                if (rayHitedObstacle)
				{
                    Debug.DrawLine(originPos, rayHitedObstacle.point, Color.red);
				}
				else
				{
					Debug.DrawLine(originPos, rayHitedTarget.point, Color.green);
                }
            }
		}
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
            Vector3 lookDir = AngleToDirZ(viewRotateZ);

            Debug.DrawRay(originPos, horizontalLeftDir * viewRadius, Color.cyan);
            Debug.DrawRay(originPos, lookDir * viewRadius, Color.green);
            Debug.DrawRay(originPos, horizontalRightDir * viewRadius, Color.cyan);
        }
    }
}
