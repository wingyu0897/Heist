using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Gun : Weapon, IWeaponinfo
{
	[SerializeField] private GunSO gunData;
	public GunSO WeaponData { get => gunData; }

	public Transform muzzle;
	[SerializeField] private bool doRecoil = true;

	private int currentAmmo;
	private int magAmmo;
	[SerializeField]
	private int fireCount = 0;

	private bool canAttack = true;
	private bool isReloading = false;
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

	[Header("--Events--")]
	public UnityEvent OnShoot;

	private void Awake()
	{
		weaponManager = transform.parent.GetComponent<WeaponManager>();		
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
		{
			return;
		}

		if (canAttack && !isReloading && magAmmo > 0) //공격중이 아닐 때 && 재장전 중이 아닐 때 && 남은 총알이 있을 때
		{
			++fireCount;

			myAnimator?.SetTrigger(shootHash);								//발사 애니메이션 재생
			gunAudio?.PlayClips(gunData.shootClip);							//공격 사운드 재생

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
			StartCoroutine(Reloading());
		}
	}

	IEnumerator Reloading() //탄창 재장전 실행 코루틴
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
