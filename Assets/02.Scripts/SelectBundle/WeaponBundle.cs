using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBundle : Bundle
{
	[Header("--Setting--")]
	[SerializeField] private bool canUnEquip;
	public override bool CanUnEquip => canUnEquip;

	[Header("--Parameter--")]
	[SerializeField] private int price;
	[SerializeField] private Weapon weaponPrimary;
	public Weapon Primary => weaponPrimary;
	[SerializeField] private Weapon weaponSecondary;
	public Weapon Secondary => weaponSecondary;
	[SerializeField] private Weapon weaponMelee;
	public Weapon Melee => weaponMelee;
	[SerializeField] private Sprite bundleImage;
	public override Sprite BundleImage => bundleImage;
	private Image equipChecker;
	[SerializeField] private WeaponManager weaponManager;
	
	[Header("--Property--")]
	[SerializeField] private bool isEquiped = false;
	public override bool IsEquiped => isEquiped;

	private void Awake()
	{
		isEquiped = false;
		equipChecker = transform.Find("Image")?.GetComponent<Image>();
	}

	private void Start()
	{
		weaponManager = MissionData.Instance.player.transform.Find("WeaponManager").GetComponent<WeaponManager>();
	}

	public override void OnEquip()
	{
		if (PlayerData.Instance.UseMoney(price))
		{
			PlayerData.Instance.AddMoney(-price);
			isEquiped = true;
			equipChecker.color = Color.white;
			weaponManager?.SetWeapon(weaponPrimary, weaponSecondary, weaponMelee);
		}
	}

	public override void OnUnEquip()
	{
		PlayerData.Instance.AddMoney(price);
		isEquiped = false;
		equipChecker.color = Color.grey;
	}

	public override void OnSelection()
	{
		
	}

	public override bool CanEquip()
	{
		return PlayerData.Instance.UseMoney(price);
	}
}
