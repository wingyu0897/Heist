using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
	[SerializeField] private MeleeSO meleeData;

	private bool isAttack = false;
	private Collider2D myCollider;
	private Animator animator;
	private readonly int attackHash = Animator.StringToHash("Attack");

	private void Awake()
	{
		myCollider = GetComponentInChildren<Collider2D>();
		animator = GetComponent<Animator>();
		meleeData.attackPoint = transform.Find("Point");
		GetComponentInParent<SmoothAim>()?.SetValues(meleeData.minAimRange, meleeData.weaponSlerpSpeed);		
	}

	private void OnEnable()
	{
		isAttack = false;
		myCollider.enabled = false;
	}

	public override void Attack()
	{
		animator.SetTrigger(attackHash);
		StartCoroutine(AttackDelay());
	}

	IEnumerator AttackDelay()
	{
		isAttack = true;

		yield return new WaitForSeconds(meleeData.attackDelay);

		isAttack = false;
	}

	public override bool TryAttack()
	{
		return !isAttack;
	}

	public override bool KeyPress()
	{
		return Input.GetKeyDown(KeyCode.Mouse0);
	}

	public void OnCollider()
	{
		myCollider.enabled = true;
	}

	public void OffCollider()
	{
		myCollider.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent<IDamageable>(out IDamageable obj))
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
