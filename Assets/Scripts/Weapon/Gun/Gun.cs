using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Gun : Weapon
{
	public GunSO WeaponData { get => weaponData; }
	[SerializeField] 
	private GameObject bulletPrefab;
	[SerializeField] 
	private Transform muzzle;
	[SerializeField] 
	private TextMeshProUGUI magAmmoText;
	[SerializeField] 
	private TextMeshProUGUI currentAmmoText;

	private int currentAmmo;
	private int magAmmo;
	private bool canShooting = true;
	private bool isReloading = false;
	private Animator myAnimator;
	private readonly int shootHash = Animator.StringToHash("Shoot");
	private readonly int reloadingHash = Animator.StringToHash("Reloading");

	private Transform weaponHolder;

	private void Start()
	{
		weaponData.muzzle = transform.Find("Muzzle");
		myAnimator = GetComponent<Animator>();
		weaponHolder = transform.parent;
		currentAmmo = weaponData.currentAmmo;
		magAmmo = weaponData.magSize;
	}

	private void Update()
	{
		Reload();
		magAmmoText.text = $"{magAmmo} /";
		currentAmmoText.text = $"{currentAmmo}";
	}

	private void FixedUpdate()
	{
		GunAiming();
	}

	private void OnEnable()
	{
		StopAllCoroutines();
		isReloading = false;
	}

	private void GunAiming()
	{
		Vector2 pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 gunDir = (Vector3)pointer - muzzle.position;
		float gunAngle = Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg;
		if (Vector3.Distance(transform.position, pointer) > WeaponData.minAimRange)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(gunAngle, Vector3.forward), WeaponData.gunSlerpSpeed);
		}
		else
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.AngleAxis(0, Vector3.forward), WeaponData.gunSlerpSpeed);
		}
	}

	public override void Attack()
	{
		if (canShooting && !isReloading && magAmmo > 0)
		{
			myAnimator?.SetTrigger(shootHash);
			magAmmo--;
			GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
			Destroy(bullet, 3);
			StartCoroutine(FireDelay());
			Recoil();
		}
	}

	public override bool TryAttack() //발사 가능 여부
	{
		bool result = canShooting && !isReloading && magAmmo > 0;
		return result;
	}

	private void Recoil() //반동
	{
		float spread = weaponHolder.localScale.y > 0 ? WeaponData.spreadAngle : -WeaponData.spreadAngle;
		Quaternion spreadRot = Quaternion.Euler(new Vector3(0, 0, spread));
		transform.rotation *= spreadRot;
	}

	public void Reload()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (!isReloading && magAmmo < weaponData.magSize && currentAmmo > 0)
			{
				isReloading = true;
				myAnimator?.SetTrigger(reloadingHash);
				StartCoroutine(Reloading());
			}
		}
	}

	IEnumerator Reloading()
	{
		yield return new WaitForSeconds(weaponData.reloadTime);

		if (currentAmmo >= weaponData.magSize - magAmmo)
		{
			currentAmmo -= weaponData.magSize - magAmmo;
			magAmmo = weaponData.magSize;
		}
		else
		{
			magAmmo += currentAmmo;
			currentAmmo = 0;
		}
		isReloading = false;
	}

	IEnumerator FireDelay() //발사 딜레이
	{
		canShooting = false;

		yield return new WaitForSeconds(weaponData.fireRate);

		canShooting = true;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, WeaponData.minAimRange);
		Gizmos.color = Color.white;
	}
}
