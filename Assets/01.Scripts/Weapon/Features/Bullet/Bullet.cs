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

	private Rigidbody2D myRigid;
	private float time;

	public override void Initialize()
	{
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
			PoolManager.instance.Push(this);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		IDamageable damageable = collision.GetComponent<IDamageable>();
		damageable?.GetHit(bulletData.damage);

		PoolManager.instance.Push(this);
	}

	public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
	{
		transform.position = position;
		transform.rotation = rotation;
	}
}
