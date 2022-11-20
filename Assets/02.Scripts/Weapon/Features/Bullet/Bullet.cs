using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Poolable
{
	private BulletSO bulletData;
	public BulletSO BulletData
	{
		get => bulletData;
		set => bulletData = value;
	}

	private bool isDead = false;

	private Rigidbody2D myRigid;
	private float time;

	public override void Initialize()
	{
		isDead = false;
		myRigid.velocity = Vector2.zero;
		time = 0;
	}

	private void Awake()
	{
		myRigid = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		myRigid.velocity = transform.right * bulletData.speed;
		time += Time.fixedDeltaTime;

		if (time > bulletData.lifeTime)
		{
			PoolManager.Instance.Push(this);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bulletData.targetLayer & (1 << collision.gameObject.layer)) > 0 && !isDead)
		{
			isDead = true;

			IDamageable damageable = collision.GetComponent<IDamageable>();
			damageable?.GetHit(bulletData.damage);

			PoolManager.Instance.Push(this);
		}
	}

	public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
	{
		transform.position = position;
		transform.rotation = rotation;
	}
}
