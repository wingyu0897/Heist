using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
	private Movement movement;
	private Vector2 mousePos;
	[SerializeField] private Slider healthSlider;
	private float health;
	public int Health { get => (int)health; set => health = value; }

	[Header("--Events--")]
	public UnityEvent<Vector2> OnMovementPress;
	public UnityEvent<Vector2> OnPointerPositionChange;

	public UnityEvent OnAttackButtonPressed;
	public UnityEvent OnAttackButtonReleased;
	public UnityEvent OnReloadWeapon;
	public UnityEvent<int> OnChangeWeapon;

	private void Awake()
	{
		movement = GetComponent<Movement>();
	}

	private void Start()
	{
		health = PlayerData.Instance.MaxHealth;
		healthSlider.value = (health / PlayerData.Instance.MaxHealth) * 0.75f;
	}

	private void FixedUpdate()
	{
		Movement();
		MousePosition();		
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
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			OnChangeWeapon?.Invoke(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			OnChangeWeapon?.Invoke(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			OnChangeWeapon?.Invoke(2);
		}
	}

	private void UseWeapon()
	{
		if (!PlayerData.Instance.isRunning && !GameManager.Instance.isOption)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				OnAttackButtonPressed?.Invoke();
			}
		}
		if (Input.GetKeyUp(KeyCode.Mouse0) || PlayerData.Instance.isRunning || GameManager.Instance.isOption)
		{
			OnAttackButtonReleased?.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			OnReloadWeapon?.Invoke();
		}
	}

	public void GetHit(int damage)
	{
		health -= damage;
		healthSlider.value = (health / PlayerData.Instance.MaxHealth) * 0.75f;

		if (health <= 0)
		{
			MissionData.Instance.EndTheGame(false);
		}
	}
}
