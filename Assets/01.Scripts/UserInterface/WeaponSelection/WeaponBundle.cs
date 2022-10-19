using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBundle : Bundle
{
	[Header("--References--")]
	[SerializeField] private bool canUnEquip;
	[SerializeField] private Weapon weaponPrimary;
	[SerializeField] private Weapon weaponSecondary;
	[SerializeField] private Weapon weaponMelee;
	[SerializeField] private Sprite bundleImage;
	public override bool CanUnEquip => canUnEquip;
	public Weapon Primary => weaponPrimary;
	public Weapon Secondary => weaponSecondary;
	public Weapon Melee => weaponMelee;
	public override Sprite BundleImage => bundleImage;

	
	[SerializeField] private bool isEquiped = false;
	private Image equipChecker;
	public override bool IsEquiped => isEquiped;

	private void Awake()
	{
		equipChecker = transform.Find("Image")?.GetComponent<Image>();
	}

	public override void OnEquip()
	{
		equipChecker.color = Color.white;
		PlayerData.Instance?.SetPrimaryWeapon(weaponPrimary);
		PlayerData.Instance?.SetSecondaryWeapon(weaponSecondary);
		PlayerData.Instance?.SetMeleeWeapon(weaponMelee);
	}

	public override void OnUnEquip()
	{
		equipChecker.color = Color.grey;
	}

	public override void OnSelection()
	{
		
	}
}
