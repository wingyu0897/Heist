using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	public UnityEvent<Vector2> OnMovementPress;
	public UnityEvent<Vector2> OnPointerPositionChange;

	private void FixedUpdate()
	{
		MousePosition();		
		Movement();
	}

	private void Movement()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		OnMovementPress?.Invoke(new Vector2(x, y));
	}

	private void MousePosition()
	{
		Vector2 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		OnPointerPositionChange?.Invoke(mousePos);
	}
}
