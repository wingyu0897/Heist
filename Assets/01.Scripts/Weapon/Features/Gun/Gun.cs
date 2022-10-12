using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Gun : Weapon, IWeaponinfo
{
	[SerializeField] private GunSO gunData;
	public GunSO WeaponData { get => gunData; }

	[SerializeField] private Transform muzzle;
	[SerializeField] private bool doRecoil = true;

	private int currentAmmo;
	private int magAmmo;

	private bool canAttack = true;
	private bool isReloading = false;
	private bool isFire = false;
	public bool IsFire { get => isFire; set => isFire = value; }

	public string MainInfo => currentAmmo.ToString();
	public string SubInfo => magAmmo.ToString();
	public Texture2D WeaponImage => gunData.weaponSprite;

	private Transform weaponHolder;
	private WeaponAudio gunAudio;
	private Animator myAnimator;
	private readonly int shootHash = Animator.StringToHash("Shoot");
	private readonly int reloadingHash = Animator.StringToHash("Reloading");

	private void Start()
	{
		Init();
	}

	private void OnEnable()
	{
		StopAllCoroutines(); //Ȱ��ȭ �Ǿ��� �� ��� �ڷ�ƾ ���� (������, �߻� ������)
		isReloading = false; //������ ����
		canAttack = true;  //�߻� ����
		isFire = false;
}

	private void Update()
	{
		if (isFire)
		{
			Attack();
		}
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
	}

	public override void StartAttack() //���� ���� �Լ�
	{
		isFire = true;
	}

	private void Attack()
	{
		if (canAttack && !isReloading && magAmmo > 0) //�������� �ƴ� �� && ������ ���� �ƴ� �� && ���� �Ѿ��� ���� ��
		{
			myAnimator?.SetTrigger(shootHash);								//�߻� �ִϸ��̼� ���
			gunAudio?.PlayClips(gunData.shootClip);							//���� ���� ���

			--magAmmo;														//�Ѿ� - 1
			canAttack = false;												//���� �Ұ�
			isFire = gunData.isAuto ? magAmmo == 0 ? false : true : false;	//�߻� blabla

			SpawnBullet();													//�Ѿ� �߻�
			Recoil();														//�ݵ�
			StartCoroutine(FireDelay());									//�߻� ������
		}
		else if (magAmmo == 0 && isFire && !isReloading) //���� �Ѿ��� ���� �� && �߻� && ������ ���� �ƴ� ��
		{
			isFire = false; //�߻� ����
			Reload();		//������
		}
	}

	private void SpawnBullet() //Ǯ�Ŵ�¡���� �Ѿ� ����
	{
		Bullet bullet = PoolManager.instance.Pop(gunData.bulletData.prefab.name) as Bullet;
		if (bullet)
		{
			bullet.BulletData = gunData?.bulletData;
			bullet.SetPositionAndRotation(muzzle.position, muzzle.rotation);
		}
	}

	public override bool TryAttack() //���� ���� ���θ� Ȯ���ϴ� �Լ�
	{
		bool result = canAttack && !isReloading;
		return result;
	}

	private void Recoil() //�ݵ��� ���ϴ� �Լ�
	{
		if (doRecoil)
		{
			float spread = weaponHolder.localScale.y > 0 ? gunData.spreadAngle : -gunData.spreadAngle;
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
			StartCoroutine(Reloading());
		}
	}

	IEnumerator Reloading() //źâ ������ ���� �ڷ�ƾ
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

	private void OnDrawGizmos() //���� �ּҰŸ��� ��Ÿ���� �����(���� ��)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(muzzle.position, transform.right * 10);
		Gizmos.DrawWireSphere(transform.position, gunData.minAimRange);
		Gizmos.color = Color.white;
	}
}
