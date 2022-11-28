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
		StopAllCoroutines(); //활성화 되었을 때 모든 코루틴 정지 (재장전, 발사 딜레이)
		weaponManager?.muzzleFlash?.flash?.transform.SetPositionAndRotation(muzzle.position, Quaternion.identity);
		canAttack = true;  //발사 가능
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

	public override void Aiming(Vector2 pointerPos) //에임 시스템 함수 //WeaponHolder의 에임 시스템과 별개
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

	public override void StartAttack() //공격 실행 함수
	{
		isFire = true;
	}

	private void Attack()
	{
		if (gunData.isBurst && fireCount >= gunData.burstCount) 
			return;

		if (canAttack && !isReloading && magAmmo > 0) //공격중이 아닐 때 && 재장전 중이 아닐 때 && 남은 총알이 있을 때
		{
			if (!gunData.canSilence || !StageManager.Instance.isSilencer)
			{
				StageManager.Instance.isDetected = true;
				gunAudio?.PlayClips(gunData.shootClip);						//공격 사운드 재생
			}
			else
			{
				gunAudio?.PlayClips(gunData.silenceShootAudioClip);         //소음기 착용 공격 사운드 재생
			}

			myAnimator?.SetTrigger(shootHash);								//발사 애니메이션 재생

			++fireCount;

			--magAmmo;														//총알 - 1
			canAttack = false;												//공격 불가
			isFire = gunData.isAuto ? magAmmo == 0 ? false : true : false;	//발사 blabla

			SpawnBullet();													//총알 발사
			Recoil();                                                       //반동

			weaponManager.camShake?.StopAllCoroutines();
			weaponManager.camShake?.ShakeCamera(gunData.camShake);			//카메라 흔들림
			weaponManager.muzzleFlash?.ToggleLight();
			StartCoroutine(FireDelay());									//발사 딜레이

			OnShoot?.Invoke();
		}
		else if (magAmmo == 0 && isFire && !isReloading) //남은 총알이 없을 때 && 발사 && 재장전 중이 아닐 때
		{
			isFire = false; //발사 중지
			Reload();		//재장전
		}
	}

	private void SpawnBullet() //풀매니징으로 총알 생성
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

	public override bool TryAttack() //공격 가능 여부를 확인하는 함수
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

	private void Recoil() //반동을 가하는 함수
	{
		if (doRecoil)
		{
			float spread = weaponHolder.localScale.y > 0 ? gunData.recoilSpreadAngle : -gunData.recoilSpreadAngle;
			Quaternion spreadRot = Quaternion.Euler(new Vector3(0, 0, spread));
			transform.rotation *= spreadRot;
		}
	}

	public override void Reload() //재장전 함수
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

	IEnumerator FireDelay() //발사 딜레이 코루틴
	{
		yield return new WaitForSeconds(gunData.fireRate);

		canAttack = true;
	}
	#endregion

	public override void Init()
	{
		//장탄수 초기화
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
	private void OnDrawGizmos() //에임 최소거리를 나타내는 기즈모(빨간 원)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(muzzle.position, transform.right * 10);
		Gizmos.DrawWireSphere(transform.position, gunData.minAimRange);
		Gizmos.color = Color.white;
	}
#endif
}
