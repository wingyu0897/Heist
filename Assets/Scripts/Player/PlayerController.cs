using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	public UnityEvent<float> OnMovementPress;
	public UnityEvent<Vector2> OnPointerPositionChange;

	private void FixedUpdate()
	{
		Movement();
		MousePosition();
	}

	private void Movement()
	{
		float x = Input.GetAxisRaw("Horizontal");

		OnMovementPress?.Invoke(x);
	}

	private void MousePosition()
	{
		Vector2 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		OnPointerPositionChange?.Invoke(mousePos);
	}
}
