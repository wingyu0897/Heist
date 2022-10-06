using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
	[SerializeField] private List<Weapon> weapons = new List<Weapon>();
	[SerializeField] private Weapon currentWeapon;
	[SerializeField] private Transform basePosition;
	[Range(0, 1)][Tooltip("GunHolder의 Slerp 값")]
	[SerializeField] private float slerpHolder;

	private WeaponRenderer weaponRenderer;

	public UnityEvent OnAttack;

	private void Start()
	{
		currentWeapon?.Init();
	}

	public void SetWeapon(Weapon primary = null, Weapon secondary = null, Weapon melee = null)
	{
		primary.Init();
		secondary.Init();
		melee.Init();

		weapons.Add(primary);
		weapons.Add(secondary);
		weapons.Add(melee);

		AssignWeapon(0);
	}

	public void AssignWeapon(int weaponNum) //현재 무기의 WeaponRenderer 가져오기
	{
		foreach (Weapon we in weapons)
		{
			we.gameObject.SetActive(false);
		}

		if (weaponNum <= weapons.Count - 1)
		{
			currentWeapon = weapons[weaponNum];
			currentWeapon.gameObject.SetActive(true);
			weaponRenderer = currentWeapon.transform.GetComponent<WeaponRenderer>();
			weaponRenderer.Init();
		}
	}

	public void StartAttack() //무기의 사용가능 여부 확인 및 무기를 사용
	{
		if (currentWeapon.TryAttack())
		{
			currentWeapon?.StartAttack();
			OnAttack?.Invoke();
		}
	}

	public void StopAttack()
	{
		currentWeapon?.StopAttack();
	}

	public void Reload()
	{
		currentWeapon?.Reload();
	}

	public void HolderAiming(Vector2 pointPos) //WeaponHolder 조준 함수
	{
		Vector3 holderDir = (Vector3)pointPos - basePosition.position;
		float holderAngle = Mathf.Atan2(holderDir.y, holderDir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(holderAngle, Vector3.forward), slerpHolder);
		FlipScale();
		AdjustWeaponRendering();
	}

	public void WeaponAiming(Vector2 pointerPos)
	{
		currentWeapon?.Aiming(pointerPos);
	}

	private void FlipScale()
	{
		Vector3 localScale = Vector3.one;
		localScale.y = transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270 ? -1 : 1;
		transform.localScale = localScale;
	}

	private void AdjustWeaponRendering()
	{
		weaponRenderer?.LayerOrder(transform.eulerAngles.z);
	}
}
