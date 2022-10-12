using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRenderer : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private readonly int speedHash = Animator.StringToHash("Speed");

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	public void Flip(Vector2 input)
	{
		float direction = input.x - transform.position.x;
		if (direction > 0)
		{
			spriteRenderer.flipX = true;
		}
		else if (direction < 0)
		{
			spriteRenderer.flipX = false;
		}
	}

	public void Animation(float speed)
	{
		animator?.SetFloat(speedHash, speed);
	}
}
