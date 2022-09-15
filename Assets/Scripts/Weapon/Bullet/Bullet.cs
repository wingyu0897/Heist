using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private Rigidbody2D myRigid;

	private void Awake()
	{
		myRigid = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		myRigid.velocity = transform.right * 25;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		IDamageable damageable = collision.GetComponent<IDamageable>();
		damageable?.GetHit(10);

		Destroy(gameObject);
	}
}
