using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
	[SerializeField] private MovememtSO moveData;
	private bool isRunning = false;
	public bool IsRunning { get => isRunning; set => isRunning = value; }

	private float currentVelocity;
	private Vector2 currentDirection;

	private Rigidbody2D myRigid;

	public UnityEvent<float> OnVelocityChange;

	private void Awake()
	{
		myRigid = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		myRigid.velocity = currentVelocity * currentDirection;
	}

	public void Move(Vector2 moveInput)
	{
		if (moveInput.sqrMagnitude > 0)
		{
			if (Vector2.Dot(moveInput, currentDirection) < 0)
			{
				currentVelocity = 0;
			}
			currentDirection = moveInput.normalized;
		}

		currentVelocity = SetSpeed(moveInput);

		OnVelocityChange?.Invoke(currentVelocity);
	}

	private float SetSpeed(Vector2 velocity)
	{
		if (velocity.sqrMagnitude > 0)
		{
			currentVelocity += moveData.acceleration * Time.deltaTime;
		}
		else
		{
			currentVelocity -= moveData.deAcceleration * Time.deltaTime;
		}

		return Mathf.Clamp(currentVelocity, 0, isRunning ? moveData.maxRunSpeed : moveData.maxWalkSpeed);
	}
}
