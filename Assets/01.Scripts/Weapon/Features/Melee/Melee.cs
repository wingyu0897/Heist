using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
	[SerializeField] private MeleeSO meleeData;
	
	private bool isAttack = false;
	private Transform parentTransform;
	private Collider2D myCollider;
	private Animator animator;
	private readonly int attackHash = Animator.StringToHash("Attack");

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

	#region #Attack
	public override void StartAttack()
	{
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
	}

	public void OffCollider() //애니메이션 중간에 활성화하여 공격을 불가능하게 한다
	{
		myCollider.enabled = false;
	}
	#endregion

	public override void Init()
	{
		myCollider = GetComponentInChildren<Collider2D>();
		animator = GetComponent<Animator>();
		meleeData.attackPoint = transform.Find("Point");
		parentTransform = transform.parent;
	}

	public override GameObject GetPrefab()
	{
		return meleeData.prefab;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out IDamageable obj))
		{
			obj.GetHit(meleeData.damage);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, meleeData.minAimRange);
		Gizmos.DrawRay(meleeData.attackPoint.position, meleeData.attackPoint.right * meleeData.range);
		Gizmos.color = Color.white;
	}
}
