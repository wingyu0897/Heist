using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	public UnityEvent<Vector2> OnMovementPress;
	public UnityEvent<Vector2> OnPointerPositionChange;

	public UnityEvent OnAttackButtonPressed;
	public UnityEvent OnAttackButtonReleased;
	public UnityEvent OnReloadWeapon;
	public UnityEvent<int> OnChangeWeapon;

	private Movement movement;
	private Vector2 mousePos;

	private void Awake()
	{
		movement = GetComponent<Movement>();
	}

	private void Start()
	{		
		transform.Find("WeaponHolder").GetComponent<WeaponManager>().SetWeapon(PlayerData.Instance.primaryWeapon, PlayerData.Instance.secondaryWeapon, PlayerData.Instance.melee);
	}

	private void FixedUpdate()
	{
		MousePosition();		
		Movement();
	}

	private void Update()
	{
		ChangeWeapon();
		UseWeapon();
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

	private void ChangeWeapon()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			OnChangeWeapon?.Invoke(PlayerData.Instance.CurrentWeaponNum < 2 ? ++PlayerData.Instance.CurrentWeaponNum : PlayerData.Instance.CurrentWeaponNum = 0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			OnChangeWeapon?.Invoke(PlayerData.Instance.CurrentWeaponNum = 0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			OnChangeWeapon?.Invoke(PlayerData.Instance.CurrentWeaponNum = 1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			OnChangeWeapon?.Invoke(PlayerData.Instance.CurrentWeaponNum = 2);
		}
	}

	private void UseWeapon()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			OnAttackButtonPressed?.Invoke();
		}
		
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			OnAttackButtonReleased?.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			OnReloadWeapon?.Invoke();
		}
	}
}
