using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothAim : MonoBehaviour
{
	[SerializeField] private float minAimRange;
	[SerializeField] private float weaponSlerpSpeed;

	private void FixedUpdate()
	{
		WeaponAiming();
	}

	private void WeaponAiming() //에임 시스템 함수 //WeaponHolder의 에임 시스템과 별개
	{
		Vector2 pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 meleeDir = (Vector3)pointer - transform.position;
		float meleeAngle = Mathf.Atan2(meleeDir.y, meleeDir.x) * Mathf.Rad2Deg;
		if (Vector3.Distance(transform.position, pointer) > minAimRange)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(meleeAngle, Vector3.forward), weaponSlerpSpeed);
		}
		else
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.AngleAxis(0, Vector3.forward), weaponSlerpSpeed);
		}
	}

	public void SetValues(float minAimRange, float weaponSlerpSpeed)
	{
		this.minAimRange = minAimRange;
		this.weaponSlerpSpeed = weaponSlerpSpeed;
	}
}
