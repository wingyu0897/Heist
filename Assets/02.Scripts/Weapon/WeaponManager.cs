using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
	[Header("--Weapon--")]
	[SerializeField] private List<Weapon> weapons = new List<Weapon>();
	public Weapon currentWeapon;
	[SerializeField] Weapon primaryWeapon;
	[SerializeField] Weapon secondaryWeapon;
	[SerializeField] Weapon meleeWeapon;

	[Header("--Properties--")]
	[SerializeField] private Transform basePosition;
	[Range(0, 1)][Tooltip("GunHolder�� Slerp ��")]
	[SerializeField] private float slerpHolder;

	private WeaponRenderer weaponRenderer;

	public UnityEvent OnAttack;
	public UnityEvent<Weapon> OnChangeWeapon;

	private void Awake()
	{
		if (weapons.Count == 0)
			SetWeapon(primaryWeapon, secondaryWeapon, meleeWeapon);		
	}

	private void Start()
	{
		currentWeapon?.Init();
	}

	public void SetWeapon(Weapon primary = null, Weapon secondary = null, Weapon melee = null)
	{
		for (int i = 0; i < 3; i++)
		{
			if (weapons.Count > i && weapons[i]) //������ �����ϴ� ���⸦ ��Ȱ��ȭ
				weapons[i]?.gameObject?.SetActive(false);
		}

		primaryWeapon = primary;
		secondaryWeapon = secondary;
		meleeWeapon = melee;

		primary?.Init();
		secondary?.Init();
		melee?.Init();

		weapons.Clear();

		weapons.Add(primary);
		weapons.Add(secondary);
		weapons.Add(melee);

		AssignWeapon(0);
	}

	public void AssignWeapon(int weaponNum) //���� ������ WeaponRenderer ��������
	{
		foreach (Weapon item in weapons)
		{
			item?.gameObject.SetActive(false);
		}

		if (weaponNum <= weapons.Count - 1)
		{
			currentWeapon = weapons[weaponNum];
			currentWeapon.gameObject.SetActive(true);
			weaponRenderer = currentWeapon.transform.GetComponent<WeaponRenderer>();
			weaponRenderer.Init();
		}

		OnChangeWeapon?.Invoke(currentWeapon);
	}

	public void StartAttack() //������ ��밡�� ���� Ȯ�� �� ���⸦ ���
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

	public void HolderAiming(Vector2 pointPos) //WeaponHolder ���� �Լ�
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
