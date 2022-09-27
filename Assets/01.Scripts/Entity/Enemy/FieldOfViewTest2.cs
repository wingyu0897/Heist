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

	private void FindViewTargets()
	{
        Vector2 originPos = transform.position;
        Collider2D hitedTargets = Physics2D.OverlapCircle(originPos, viewRadius, playerLayerMask);

        if (hitedTargets != null)
		{
            Vector2 targetPos = hitedTargets.transform.position;
            Vector2 dir = (targetPos - originPos).normalized;
            Vector2 lookDir = AngleToDirZ(viewRotateZ);
            
            float dirAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + viewRotateZ - 90;

            Debug.Log(dirAngle);
            float angle = Vector3.Angle(lookDir, dir);
            angle = dirAngle > 0 || dirAngle < -180 ? -angle : angle;

            Debug.DrawRay(originPos, AngleToDirZ(angle + viewRotateZ + 5f) * Vector2.Distance(originPos, targetPos), Color.red);
            Debug.DrawRay(originPos, AngleToDirZ(angle + viewRotateZ - 5f) * Vector2.Distance(originPos, targetPos), Color.red);

            if (Mathf.Abs(angle) <= horizontalViewHalfAngle)
			{
                RaycastHit2D rayHitedObstacle = Physics2D.Raycast(originPos, dir, viewRadius + 1, obstacleLayerMask);
                RaycastHit2D rayHitedTarget = Physics2D.Raycast(originPos, dir, viewRadius + 1, playerLayerMask);

                rayHitedObstacle.distance = rayHitedObstacle ? rayHitedObstacle.distance : viewRadius + 1;

                if (rayHitedObstacle.distance < rayHitedTarget.distance)
				{
                    Debug.DrawLine(originPos, rayHitedObstacle.point, Color.yellow);
				}
				else
				{
					Debug.DrawLine(originPos, rayHitedTarget.point, Color.red);
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
