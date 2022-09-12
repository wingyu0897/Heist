using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private MovememtSO moveData;

	public UnityEvent<float> OnVelocityChange;

	private Rigidbody2D myRigid;
	private Collider2D myCollider;

	private float currentTime;

	private void Awake()
	{
		myRigid = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
	}

	private void FixedUpdate()
	{
		myRigid.velocity = new Vector2(moveData.currentVelocity * moveData.currentDirection, myRigid.velocity.y);
	}

	private void Update() //지우기
	{
		Jump();
		currentTime += Time.deltaTime;
		Debug.DrawRay(myCollider.bounds.center, Vector2.down * (myCollider.bounds.extents.y + 0.1f), CanJump() ? Color.green : Color.red);
	}

	public void Movement(float velocity)
	{
		if (velocity != 0)
		{
			if (moveData.currentDirection != velocity)
			{
				moveData.currentVelocity = 0;
			}
			moveData.currentDirection = velocity;
		}

		moveData.currentVelocity = SetSpeed(velocity);

		OnVelocityChange?.Invoke(moveData.currentVelocity);
	}

	private void Jump() //임시
	{
		if (Input.GetKeyDown(KeyCode.W) && CanJump())
		{
			currentTime = 0;
			myRigid.AddForce(Vector2.up * moveData.jumpPower, ForceMode2D.Impulse);
		}
	}

	private float SetSpeed(float velocity)
	{
		if (velocity != 0)
		{
			moveData.currentVelocity += moveData.acceleration * Time.deltaTime;
		}
		else
		{
			moveData.currentVelocity -= moveData.deAcceleration * Time.deltaTime;
		}

		return Mathf.Clamp(moveData.currentVelocity, 0, moveData.maxSpeed);
	}

	private bool CanJump()
	{
		int layerMask = 1 << LayerMask.NameToLayer("Ground");
		RaycastHit2D rayHit = Physics2D.BoxCast(myCollider.bounds.center, myCollider.bounds.size - new Vector3(0.1f, 0, 0), 0f, Vector2.down, 0.01f, layerMask);
		return rayHit.collider != null && rayHit.collider.gameObject != gameObject && currentTime >= moveData.jumpRate;
	}
}
