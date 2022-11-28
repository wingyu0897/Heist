using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
	[HideInInspector]
	public CameraShake camShake;

	[Header("--Weapon--")]
	[SerializeField] 
	private List<Weapon> weapons = new List<Weapon>();
	public Weapon currentWeapon;
	[SerializeField] 
	private Weapon primaryWeapon;
	[SerializeField] 
	private Weapon secondaryWeapon;
	[SerializeField] 
	private Weapon meleeWeapon;
	public MuzzleFlash muzzleFlash;

	[Header("--Properties--")]
	public Transform basePosition;
	public Image reloadImg;
	[SerializeField][Range(0, 1)][Tooltip("GunHolder의 Slerp 값")] 
	private float slerpHolder;
	public bool isAttack = false;

	private WeaponRenderer weaponRenderer;

	public UnityEvent OnAttack;
	public UnityEvent<Weapon> OnChangeWeapon;

	private void Awake()
	{
		camShake = GetComponent<CameraShake>();
		muzzleFlash = GetComponent<MuzzleFlash>();

		if (weapons.Count == 0)
			SetWeapon(primaryWeapon, secondaryWeapon, meleeWeapon);		
	}

	private void Start()
	{
		currentWeapon?.Init();
	}

	private void Update()
	{
		if (isAttack)
		{
			OnAttack?.Invoke();
		}
	}

	public void SetWeapon(Weapon primary = null, Weapon secondary = null, Weapon melee = null)
	{
		for (int i = 0; i < 3; i++)
		{
			if (weapons.Count > i && weapons[i]) //기존에 존재하는 무기를 비활성화
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

	public void AssignWeapon(int weaponNum) //현재 무기의 WeaponRenderer 가져오기
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

		reloadImg?.transform.parent.gameObject.SetActive(false);
		OnChangeWeapon?.Invoke(currentWeapon);
	}

	public void StartAttack() //무기의 사용가능 여부 확인 및 무기를 사용
	{
		if (currentWeapon.TryAttack())
		{
			isAttack = true;
			currentWeapon?.StartAttack();
		}
		else
		{
			isAttack = false;
		}
	}

	public void StopAttack()
	{
		isAttack = false;
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
