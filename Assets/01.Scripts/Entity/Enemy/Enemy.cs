using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable
{
	private int health = 1000;
	public int Health { get => health; set => health = value; }
	private AIBrain brain;

	private void Awake()
	{
		brain = GetComponent<AIBrain>();
	}

	public void GetHit(int damage)
	{
		brain.Notice();

		health -= damage;
		if (health <= 0)
		{
			brain.Dead();
		}
	}
}
