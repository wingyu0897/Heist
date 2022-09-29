using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable
{
	private int health = 1000;
	public int Health { get => health; set => health = value; }

	public void GetHit(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}
}
