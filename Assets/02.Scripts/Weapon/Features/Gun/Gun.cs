using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Gun : Weapon, IWeaponinfo
{
	[SerializeField] 
	private GunSO gunData;
	public GunSO WeaponData { get => gunData; }
	public Transform muzzle;
	[SerializeField] 
	private bool doRecoil = true;

	private int currentAmmo;
	private int magAmmo;
	public float reloadTime = 0;
	[SerializeField]
	private int fireCount = 0;

	public bool canAttack = true;
	public bool isReloading = false;
	private bool isFire = false;
	public bool IsFire { get => isFire; set => isFire = value; }

	public string MainInfo => currentAmmo.ToString();
	public string SubInfo => magAmmo.ToString();
	public Texture2D WeaponImage => gunData.weaponSprite;

	private Transform weaponHolder;
	private WeaponAudio gunAudio;
	private WeaponManager weaponManager;
	private Animator myAnimator;
	private readonly int shootHash = Animator.StringToHash("Shoot");
	private readonly int reloadingHash = Animator.StringToHash("Reloading");

	private Camera mainCam;

	[Header("--Events--")]
	public UnityEvent OnShoot;

	private void Awake()
	{
		weaponManager = transform.parent.GetComponent<WeaponManager>();	
		mainCam = Camera.main;
	}

	private void Start()
	{
		Init();
	}

	private void OnEnable()
	{
		StopAllCoroutines(); //Ȱ��ȭ �Ǿ��� �� ��� �ڷ�ƾ ���� (������, �߻� ������)
		weaponManager?.muzzleFlash?.flash?.transform.SetPositionAndRotation(muzzle.position, Quaternion.identity);
		canAttack = true;  //�߻� ����
		isFire = false;
		isReloading = false;
}

	private void Update()
	{
		if (isFire)
		{
			Attack();
		}

		Reloading();
	}

	public override void Aiming(Vector2 pointerPos) //���� �ý��� �Լ� //WeaponHolder�� ���� �ý��۰� ����
	{
		Vector3 gunDir = (Vector3)pointerPos - muzzle.position;
		float gunAngle = Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg;

		if (Vector3.Distance(transform.position, pointerPos) > gunData.minAimRange)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(gunAngle, Vector3.forward), gunData.gunSlerpSpeed);
		}
		else
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.AngleAxis(0, Vector3.forward), gunData.gunSlerpSpeed);
		}
	}

	#region #Attack

	public override void StopAttack()
	{
		isFire = false;
		fireCount = 0;
	}

	public override void StartAttack() //���� ���� �Լ�
	{
		isFire = true;
	}

	private void Attack()
	{
		if (gunData.isBurst && fireCount >= gunData.burstCount) 
			return;

		if (canAttack && !isReloading && magAmmo > 0) //�������� �ƴ� �� && ������ ���� �ƴ� �� && ���� �Ѿ��� ���� ��
		{
			if (!gunData.canSilence || !StageManager.Instance.isSilencer)
			{
				StageManager.Instance.isDetected = true;
				gunAudio?.PlayClips(gunData.shootClip);						//���� ���� ���
			}
			else
			{
				gunAudio?.PlayClips(gunData.silenceShootAudioClip);         //������ ���� ���� ���� ���
			}

			myAnimator?.SetTrigger(shootHash);								//�߻� �ִϸ��̼� ���

			++fireCount;

			--magAmmo;														//�Ѿ� - 1
			canAttack = false;												//���� �Ұ�
			isFire = gunData.isAuto ? magAmmo == 0 ? false : true : false;	//�߻� blabla

			SpawnBullet();													//�Ѿ� �߻�
			Recoil();                                                       //�ݵ�

			weaponManager.camShake?.StopAllCoroutines();
			weaponManager.camShake?.ShakeCamera(gunData.camShake);			//ī�޶� ��鸲
			weaponManager.muzzleFlash?.ToggleLight();
			StartCoroutine(FireDelay());									//�߻� ������

			OnShoot?.Invoke();
		}
		else if (magAmmo == 0 && isFire && !isReloading) //���� �Ѿ��� ���� �� && �߻� && ������ ���� �ƴ� ��
		{
			isFire = false; //�߻� ����
			Reload();		//������
		}
	}

	private void SpawnBullet() //Ǯ�Ŵ�¡���� �Ѿ� ����
	{
		for (int i = 0; i < gunData.bulletPerShot; i++)
		{
			Bullet bullet = PoolManager.Instance.Pop(gunData.bulletData.prefab.name) as Bullet;
			if (bullet)
			{
				bullet.BulletData = gunData?.bulletData;
				bullet.SetPositionAndRotation(muzzle.position, muzzle.rotation);
				bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, bullet.transform.eulerAngles.z + Random.Range(-gunData.bulletSpreadAngle, gunData.bulletSpreadAngle)));
			}
		}
	}

	public override bool TryAttack() //���� ���� ���θ� Ȯ���ϴ� �Լ�
	{
		bool result = canAttack && !isReloading;
		if (gunData.isBurst)
		{
			if (fireCount >= gunData.burstCount)
			{
				result = false;
			}
		}

		return result;
	}

	private void Recoil() //�ݵ��� ���ϴ� �Լ�
	{
		if (doRecoil)
		{
			float spread = weaponHolder.localScale.y > 0 ? gunData.recoilSpreadAngle : -gunData.recoilSpreadAngle;
			Quaternion spreadRot = Quaternion.Euler(new Vector3(0, 0, spread));
			transform.rotation *= spreadRot;
		}
	}

	public override void Reload() //������ �Լ�
	{
		if (!isReloading && magAmmo < gunData.magSize && currentAmmo > 0)
		{
			isReloading = true;
			myAnimator?.SetTrigger(reloadingHash);
			reloadTime = 0;
		}
	}

	private void Reloading()
	{
		if (isReloading)
		{
			reloadTime += Time.deltaTime;
			if (weaponManager.reloadImg)
			{
				Transform parent = weaponManager.reloadImg.transform.parent;
				parent.transform.position = mainCam.WorldToScreenPoint(transform.position);
				parent.gameObject.SetActive(true);
				weaponManager.reloadImg.fillAmount = reloadTime / gunData.reloadTime;
			}
			if (reloadTime >= gunData.reloadTime)
			{
				if (weaponManager.reloadImg)
				{
					weaponManager.reloadImg.transform.parent.gameObject.SetActive(false);
				}

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
		}
	}

	IEnumerator FireDelay() //�߻� ������ �ڷ�ƾ
	{
		yield return new WaitForSeconds(gunData.fireRate);

		canAttack = true;
	}
	#endregion

	public override void Init()
	{
		//��ź�� �ʱ�ȭ
		magAmmo = gunData.magSize;
		currentAmmo = gunData.currentAmmo;

		gunData.muzzle = transform.Find("Muzzle");
		gunAudio = transform.GetComponentInParent<WeaponAudio>();
		myAnimator = GetComponent<Animator>();
		weaponHolder = transform.parent;
	}

	public override GameObject GetPrefab()
	{
		return gunData.prefab;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos() //���� �ּҰŸ��� ��Ÿ���� �����(���� ��)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(muzzle.position, transform.right * 10);
		Gizmos.DrawWireSphere(transform.position, gunData.minAimRange);
		Gizmos.color = Color.white;
	}
#endif
}
