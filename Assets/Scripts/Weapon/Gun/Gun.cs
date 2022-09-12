using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
	[SerializeField] 
	private GunSO gunData;
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

	private void Start()
	{
		myAnimator = GetComponent<Animator>();
		currentAmmo = gunData.currentAmmo;
		magAmmo = gunData.magSize;
	}

	private void Update()
	{
		Shoot();
		Reload();
		magAmmoText.text = $"{magAmmo} /";
		currentAmmoText.text = $"{currentAmmo}";
	}
	//컨트롤러에 연결하기
	public void Shoot()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (canShooting && !isReloading && magAmmo > 0)
			{
				myAnimator?.SetTrigger(shootHash);
				magAmmo--;
				GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
				Destroy(bullet, 3);
				StartCoroutine(FireDelay());
			}
		}
	}

	public void Reload()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (!isReloading && magAmmo < gunData.magSize && currentAmmo > 0)
			{
				isReloading = true;
				myAnimator?.SetTrigger(reloadingHash);
				StartCoroutine(Reloading());
			}
		}
	}

	IEnumerator Reloading()
	{
		yield return new WaitForSeconds(gunData.reloadTime);

		if (currentAmmo >= gunData.magSize - magAmmo)
		{
			currentAmmo -= gunData.magSize - magAmmo;
			magAmmo = gunData.magSize;
		}
		else
		{
			magAmmo += currentAmmo;
			currentAmmo = 0;
		}
		isReloading = false;
	}

	IEnumerator FireDelay()
	{
		canShooting = false;

		yield return new WaitForSeconds(gunData.fireRate);

		canShooting = true;
	}
}
