using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon, IWeaponinfo
{
	[SerializeField] private MeleeSO meleeData;
	[SerializeField] private Transform parentTransform;
	[SerializeField] private bool canMultipleAttack;
	
	private bool isAttack = false;
	private bool canDamage = true;
	private Collider2D myCollider;
	private Animator animator;
	private readonly int attackHash = Animator.StringToHash("Attack");

	public string MainInfo => "---";
	public string SubInfo => "--";
	public Texture2D WeaponImage => meleeData.weaponSprite;

	private void Start()
	{
		Init();
	}

	private void OnEnable()
	{
		isAttack = false;
		myCollider.enabled = false;
	}

	public override void Aiming(Vector2 pointerPos) //만들기
	{
		Vector3 meleeDir = (Vector3)pointerPos - parentTransform.position;
		float meleeAngle = Mathf.Atan2(meleeDir.y, meleeDir.x) * Mathf.Rad2Deg;

		if (Vector3.Distance(parentTransform.position, pointerPos) > meleeData.minAimRange)
		{
			parentTransform.rotation = Quaternion.Slerp(parentTransform.rotation, Quaternion.AngleAxis(meleeAngle, Vector3.forward), meleeData.weaponSlerpSpeed);
		}
		else
		{
			parentTransform.localRotation = Quaternion.Slerp(parentTransform.localRotation, Quaternion.AngleAxis(0, Vector3.forward), meleeData.weaponSlerpSpeed);
		}
	}

	public void EndAttack()
	{
		animator.enabled = false;
	}

	#region #Attack
	public override void StartAttack()
	{
		animator.enabled = true;
		animator.SetTrigger(attackHash);
		StartCoroutine(AttackDelay());
	}

	public override void StopAttack()
	{
		return;
	}

	IEnumerator AttackDelay()
	{
		isAttack = true;

		yield return new WaitForSeconds(meleeData.attackDelay);

		isAttack = false;
	}

	public override void Reload()
	{
		return;
	}

	public override bool TryAttack()
	{
		return !isAttack;
	}

	public void OnCollider() //애니메이션 중간에 활성화하여 공격을 가능하게 한다
	{
		myCollider.enabled = true;
		canDamage = true;
	}

	public void OffCollider() //애니메이션 중간에 활성화하여 공격을 불가능하게 한다
	{
		myCollider.enabled = false;
	}
	#endregion

	public override void Init()
	{
		myCollider = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();
		meleeData.attackPoint = transform.Find("Point");
	}

	public override GameObject GetPrefab()
	{
		return meleeData.prefab;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out IDamageable obj) && canDamage)
		{
			canDamage = canMultipleAttack ? true : false;
			obj.GetHit(meleeData.damage);
		}
	}

	private void OnDrawGizmos()
	{
		if (meleeData.attackPoint)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, meleeData.minAimRange);
			Gizmos.DrawRay(meleeData.attackPoint.position, meleeData.attackPoint.right * meleeData.range);
			Gizmos.color = Color.white;
		}
	}
}
