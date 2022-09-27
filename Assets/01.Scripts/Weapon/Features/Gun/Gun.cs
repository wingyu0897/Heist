using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Gun : Weapon
{
	[SerializeField] private GunSO gunData;
	public GunSO WeaponData { get => gunData; }
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform muzzle;
	[SerializeField] private TextMeshProUGUI magAmmoText;
	[SerializeField] private TextMeshProUGUI currentAmmoText;

	private int currentAmmo;
	private int magAmmo;
	private bool canShooting = true;
	private bool isReloading = false;
	private WeaponAudio gunAudio;
	private Animator myAnimator;
	private readonly int shootHash = Animator.StringToHash("Shoot");
	private readonly int reloadingHash = Animator.StringToHash("Reloading");

	private Transform weaponHolder;

	private void Awake()
	{
		gunData.muzzle = transform.Find("Muzzle");
		gunAudio = transform.GetComponentInParent<WeaponAudio>();
		myAnimator = GetComponent<Animator>();
	}

	private void Start()
	{
		weaponHolder = transform.parent;
		currentAmmo = gunData.currentAmmo;
		magAmmo = gunData.magSize;
	}

	private void Update()
	{
		Reload();
		magAmmoText.text = $"{magAmmo} /";
		currentAmmoText.text = $"{currentAmmo}";
		Debug.DrawRay(muzzle.position, transform.right * 10, Color.red);
	}

	private void FixedUpdate()
	{
		GunAiming();
	}

	private void OnEnable()
	{
		StopAllCoroutines();
		isReloading = false;
		canShooting = true;
	}

	private void GunAiming() //���� �ý��� �Լ� //WeaponHolder�� ���� �ý��۰� ����
	{
		Vector2 pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 gunDir = (Vector3)pointer - muzzle.position;
		float gunAngle = Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg;
		if (Vector3.Distance(transform.position, pointer) > gunData.minAimRange)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(gunAngle, Vector3.forward), WeaponData.gunSlerpSpeed);
		}
		else
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.AngleAxis(0, Vector3.forward), WeaponData.gunSlerpSpeed);
		}
	}

	public override void Attack() //���� ���� ���� Ȯ�� �� ���� �Լ� **�ϵ��ڵ��� **�����ʿ�
	{
		if (canShooting && !isReloading && magAmmo > 0)
		{
			myAnimator?.SetTrigger(shootHash);
			magAmmo--;
			gunAudio.PlayClips(gunData.shootClip);

			GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
			Destroy(bullet, 3);
			StartCoroutine(FireDelay());
			Recoil();
		}
	}

	public override bool KeyPress() //���� ��� ���θ� ��ȯ�ϴ� �Լ�(�ܹ�, ���߰� ���� ��Ȳ)
	{
		return gunData.isAuto ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
	}

	public override bool TryAttack() //�߻� ���� ����
	{
		bool result = canShooting && !isReloading && magAmmo > 0;
		return result;
	}

	private void Recoil() //�ݵ��� ���ϴ� �Լ�
	{
		float spread = weaponHolder.localScale.y > 0 ? WeaponData.spreadAngle : -WeaponData.spreadAngle;
		Quaternion spreadRot = Quaternion.Euler(new Vector3(0, 0, spread));
		transform.rotation *= spreadRot;
	}

	public void Reload() //������ �Լ� *�����ʿ�*
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

	IEnumerator Reloading() //������ ���� �ڷ�ƾ
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

	IEnumerator FireDelay() //�߻� ������ �ڷ�ƾ
	{
		canShooting = false;

		yield return new WaitForSeconds(gunData.fireRate);

		canShooting = true;
	}

	private void OnDrawGizmos() //���� �ּҰŸ��� ��Ÿ���� �����(���� ��)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, WeaponData.minAimRange);
		Gizmos.color = Color.white;
	}
}
