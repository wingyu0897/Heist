using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
	[SerializeField] 
	private float minAimRange = 0;
	[SerializeField][Range(0, 1)][Tooltip("GunHolder의 Slerp 값")]
	private float slerpHolder;
	[SerializeField][Range(0, 1)][Tooltip("Gun의 Slerp 값")]
	private float slerpGun;
	public Transform muzzle;
	private Vector2 mousePos;
	public Transform gun;

	private void Update()
	{
		mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		Debug.DrawRay(muzzle.position, muzzle.right * 100, Color.red);
	}

	public void Aiming(Vector2 pointPos)
	{
		Vector3 direction = (Vector3)pointPos - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), slerpHolder);

		Vector3 direction2 = (Vector3)pointPos - muzzle.position;
		float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
		if (Vector3.Distance(transform.position, pointPos) > minAimRange	)
		{
			gun.rotation = Quaternion.Slerp(gun.rotation, Quaternion.AngleAxis(angle2, Vector3.forward), slerpGun);
		}
		else
		{
			gun.localEulerAngles = gun.localEulerAngles;
		}

		Vector3 localScale = Vector3.one;
		localScale.y = transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270 ? -1 : 1;
		transform.localScale = localScale;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (UnityEditor.Selection.activeObject == gameObject)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, minAimRange);
			Gizmos.color = Color.white;
		}
	}
#endif
}
