using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
	[SerializeField][Range(0, 1)][Tooltip("GunHolder�� Slerp ��")]
	private float slerpHolder;
	[SerializeField]
	private Transform gun;
	private Weapon currentWeapon;
	private WeaponRenderer weaponRenderer;

	private int currentWeaponNumber;

	private bool canFire = true;

	public UnityEvent OnAttack;

	private void Start()
	{
		currentWeaponNumber = 2;
		AssignWeapon(0);
	}

	private void Update()
	{
		Attack();
		ChangeWeapon();

		if (currentWeapon.weaponData != null)
		{
			Debug.DrawRay(currentWeapon.weaponData.muzzle.position, currentWeapon.weaponData.muzzle.right * 100, Color.red);
		}
	}

	private void AssignWeapon(int weaponNum) //���� ������ WeaponRenderer ��������
	{
		currentWeaponNumber = weaponNum;
		PlayerData.Instance.primaryGun?.gameObject.SetActive(false);
		PlayerData.Instance.secondaryGun?.gameObject.SetActive(false);
		PlayerData.Instance.melee?.gameObject.SetActive(false);
		switch (currentWeaponNumber)
		{
			case 0:
				currentWeapon = PlayerData.Instance?.primaryGun;
				break;
			case 1:
				currentWeapon = PlayerData.Instance?.secondaryGun;
				break;
			case 2:
				currentWeapon = PlayerData.Instance?.melee;
				break;
			default:
				currentWeapon = PlayerData.Instance?.primaryGun;
				break;
		}
		currentWeapon.gameObject.SetActive(true);
		weaponRenderer = currentWeapon.gameObject.GetComponent<WeaponRenderer>();
	}

	private void ChangeWeapon()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			AssignWeapon(currentWeaponNumber < 2 ? ++currentWeaponNumber : 0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			AssignWeapon(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			AssignWeapon(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			AssignWeapon(2);
		}
	}

	private void Attack() //������ ��밡�� ���� Ȯ�� �� ���⸦ ���
	{
		if (KeyPress() && currentWeapon.TryAttack() && !PlayerData.Instance.isRunning)
		{
			StartCoroutine(FireDelay());
			currentWeapon.Attack();
			OnAttack?.Invoke();
		}
	}

	private bool KeyPress() //����, �ܹ� ��� ���� Ȯ��
	{
		bool fire = currentWeapon.weaponData.isAuto ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
		return fire && canFire;
	}

	IEnumerator FireDelay() //�߻� ������ �ڷ�ƾ
	{
		canFire = false;
		yield return new WaitForSeconds(currentWeapon.weaponData.fireRate);
		canFire = true;
	}

	public void Aiming(Vector2 pointPos) //WeaponHolder ���� �Լ�
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
