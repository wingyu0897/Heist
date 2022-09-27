using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private MovememtSO moveData;
	[SerializeField] private KeyCode sprintingKeyCode;
	private Rigidbody2D myRigid;

	public UnityEvent<float> OnVelocityChange;

	private void Awake()
	{
		myRigid = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (Input.GetKey(sprintingKeyCode))
		{
			PlayerData.Instance.isRunning = true;
		}
		else
		{
			PlayerData.Instance.isRunning = false;
		}
	}

	private void FixedUpdate()
	{
		myRigid.velocity = moveData.currentVelocity * moveData.currentDirection;
	}

	public void Movement(Vector2 moveInput)
	{
		if (moveInput.sqrMagnitude > 0)
		{
			if (Vector2.Dot(moveInput, moveData.currentDirection) < 0)
			{
				moveData.currentVelocity = 0;
			}
			moveData.currentDirection = moveInput.normalized;
		}

		moveData.currentVelocity = SetSpeed(moveInput);

		OnVelocityChange?.Invoke(moveData.currentVelocity);
	}

	private float SetSpeed(Vector2 velocity)
	{
		if (velocity.sqrMagnitude > 0)
		{
			moveData.currentVelocity += moveData.acceleration * Time.deltaTime;
		}
		else
		{
			moveData.currentVelocity -= moveData.deAcceleration * Time.deltaTime;
		}

		return Mathf.Clamp(moveData.currentVelocity, 0, PlayerData.Instance.isRunning ? moveData.maxRunSpeed : moveData.maxWalkSpeed);
	}
}
