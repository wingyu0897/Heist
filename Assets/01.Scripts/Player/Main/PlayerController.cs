using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	public UnityEvent<Vector2> OnMovementPress;
	public UnityEvent<Vector2> OnPointerPositionChange;

	private Movement movement;

	private Vector2 mousePos;

	private void Awake()
	{
		movement = GetComponent<Movement>();
	}

	private void FixedUpdate()
	{
		MousePosition();		
		Movement();
	}

	private void Movement()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		PlayerData.Instance.isRunning = Input.GetKey(KeyCode.LeftShift);

		movement.IsRunning = PlayerData.Instance.isRunning;
		movement.Move(new Vector2(x, y));
	}

	private void MousePosition()
	{
		mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		OnPointerPositionChange?.Invoke(mousePos);
	}
}
