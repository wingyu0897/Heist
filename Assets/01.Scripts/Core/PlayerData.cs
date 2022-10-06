using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

	public bool isRunning = false;
	public bool canRun = true;
	public int backPacks = 0;
	public int interactionSpeed = 1;

	public Weapon currentWeapon;
	public Weapon primaryWeapon;
	public Weapon secondaryWeapon;
	public Weapon melee;

	[SerializeField] private int maxHealth = 1000;
	[SerializeField] private int currentWeaponNum = 0;
	[SerializeField] private TextMeshProUGUI mainText;
	[SerializeField] private TextMeshProUGUI subText;
	[SerializeField] private Transform weaponParent;
	public int MaxHealth { get => maxHealth; set => maxHealth = Mathf.Clamp(value, 0, int.MaxValue); }
	public int CurrentWeaponNum { get => currentWeaponNum; set => currentWeaponNum = value; }

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("ERROR: Multiple PlayerData is running");
		}
		else
		{
			Instance = this;
		}

		CreateWeaponGameObject();
		currentWeapon = primaryWeapon;
	}

	public void SetWeapon(Weapon primary = null, Weapon secondary = null, Weapon melee = null)
	{
		primaryWeapon = primary == null ? primaryWeapon : primary;
		secondaryWeapon = secondary == null ? secondaryWeapon : secondary;
		this.melee = melee == null ? this.melee : melee;
	}

	public void CreateWeaponGameObject()
	{
		primaryWeapon =  Instantiate(primaryWeapon.GetPrefab(), weaponParent).GetComponent<Weapon>();
		secondaryWeapon = Instantiate(secondaryWeapon.GetPrefab(), weaponParent).GetComponent<Weapon>();
		melee = Instantiate(melee.GetPrefab(), weaponParent).GetComponent<Weapon>();
	}

	private void FixedUpdate()
	{
		if (currentWeapon.TryGetComponent(out IWeaponinfo currentInfo))
		{
			mainText.text = currentInfo.MainInfo;
			subText.text = currentInfo.SubInfo;
		}
	}
}
