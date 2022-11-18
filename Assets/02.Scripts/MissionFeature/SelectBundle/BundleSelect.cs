using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BundleSelect : MonoBehaviour
{
	[Header("--Reference--")]
	[SerializeField] private Image bundleImage;
	[SerializeField] private WeaponBundle defaultWeaponBundle;
	[SerializeField] private TextMeshProUGUI equipButtonText;

	[Header("--Variables--")]
	[SerializeField] private WeaponBundle currentWeaponBundle;
    [SerializeField] private Bundle currentSelection;

	[Header("--Events--")]
	public UnityEvent OnInitialize;

	public void Initialize()
	{
		currentSelection = defaultWeaponBundle;
		bundleImage.sprite = currentSelection.BundleImage;
		OnInitialize?.Invoke();
		Equip();
	}

	public void Selection(Bundle bundle)
	{
		currentSelection = bundle;
		currentSelection.OnSelection();
		bundleImage.sprite = currentSelection.BundleImage;
		equipButtonText.text = currentSelection.IsEquiped ? "EQUIPED" : "EQUIP";
	}

	public void Equip()
	{
		if (currentSelection.TryGetComponent(out WeaponBundle currentWeapon))
		{
			if (currentWeapon.CanEquip())
			{
				currentWeaponBundle?.OnUnEquip();
				currentWeaponBundle = currentWeapon;
			}
		}

		currentSelection?.OnEquip();
		equipButtonText.text = currentSelection.IsEquiped ? "EQUIPED" : "EQUIP";
	}
}
