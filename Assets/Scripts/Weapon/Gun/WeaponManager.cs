using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
	[SerializeField][Range(0, 1)][Tooltip("GunHolder의 Slerp 값")]
	private float slerpHolder;
	[SerializeField]
	private Transform muzzle;
	[SerializeField]
	private Transform gun;
	private Weapon currentWeapon;
	private WeaponRenderer weaponRenderer;

	public UnityEvent OnAttack;

	private void Awake()
	{
		AssignWeapon();
		currentWeapon = GetComponentInChildren<Gun>();
	}

	private void Update()
	{
		Attack();

		Debug.DrawRay(muzzle.position, muzzle.right * 100, Color.red);
	}

	private void AssignWeapon()
	{
		weaponRenderer = GetComponentInChildren<WeaponRenderer>();
	}

	private void Attack() //무기의 사용가능 여부 확인 및 무기를 사용
	{
		if (Input.GetKeyDown(KeyCode.Mouse0) && currentWeapon.TryAttack() && !PlayerData.Instance.isRunning)
		{
			currentWeapon.Attack();
			OnAttack?.Invoke();
		}
	}

	public void Aiming(Vector2 pointPos) //WeaponHolder 조준 함수
	{
		Vector3 holderDir = (Vector3)pointPos - transform.position;
		float holderAngle = Mathf.Atan2(holderDir.y, holderDir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(holderAngle, Vector3.forward), slerpHolder);
		FlipScale();
		AdjustWeaponRendering();
	}

	private void FlipScale()
	{
		Vector3 localScale = Vector3.one;
		localScale.y = transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270 ? -1 : 1;
		transform.localScale = localScale;
	}

	private void AdjustWeaponRendering()
	{
		weaponRenderer.LayerOrder(transform.eulerAngles.z);
	}
}
