using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	//[SerializeField] FieldOfView fieldOfView;

	public UnityEvent<Vector2> OnMovementPress;
	public UnityEvent<Vector2> OnPointerPositionChange;

	private Vector2 mousePos;

	private void FixedUpdate()
	{
		MousePosition();		
		Movement();
	}

	//private void Update()
	//{
	//	fieldOfView.SetAimDirection(((Vector3)mousePos - transform.position).normalized);
	//	fieldOfView.SetOrigin(transform.position);
	//}

	private void Movement()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		OnMovementPress?.Invoke(new Vector2(x, y));
	}

	private void MousePosition()
	{
		mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		OnPointerPositionChange?.Invoke(mousePos);
	}
}
