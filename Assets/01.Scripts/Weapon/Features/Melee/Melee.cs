using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
	[SerializeField]
	private MeleeSO meleeData;

	private bool isAttack = false;
	private Animator animator;
	private readonly int attackHash = Animator.StringToHash("Attack");

	private void Awake()
	{
		animator = GetComponent<Animator>();
		meleeData.attackPoint = transform.Find("Point");
	}

	private void OnEnable()
	{
		isAttack = false;
		transform.localPosition = new Vector3(0.4f, 0, 0);
	}

	private void FixedUpdate()
	{
		WeaponAiming();
	}

	public override void Attack()
	{
		Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(meleeData.attackPoint.position, new Vector2(meleeData.range*2, 0.15f), transform.eulerAngles.z);
		foreach (Collider2D collider in hitEnemies)
		{
			IDamageable damageable = collider.GetComponent<IDamageable>();
			damageable?.GetHit(meleeData.damage);
		}
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

	private void WeaponAiming() //에임 시스템 함수 //WeaponHolder의 에임 시스템과 별개
	{
		Vector2 pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 gunDir = (Vector3)pointer - transform.position;
		float gunAngle = Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg;
		if (Vector3.Distance(transform.position, pointer) > meleeData.minAimRange)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(gunAngle, Vector3.forward), 0.1f);
		}
		else
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.AngleAxis(0, Vector3.forward), 0.1f);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, meleeData.minAimRange);
		Gizmos.DrawRay(meleeData.attackPoint.position, transform.right * 0.4f);
		Gizmos.color = Color.white;
	}
}
