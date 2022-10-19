using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
	[Header("--Reference--")]
	[SerializeField] private Image bundleImage;
	[SerializeField] private WeaponBundle defaultBundle;
	[SerializeField] private SilencerBundle silencerBundle;

	[Header("--Variables--")]
	[SerializeField] private WeaponBundle currentWeaponBundle;
    [SerializeField] private Bundle currentSelection;

	private void Start()
	{
		Initialize();
	}

	public void Initialize()
	{
		currentSelection = defaultBundle;
		silencerBundle?.Initialize();
		Equip();
	}

	public void Selection(Bundle bundle)
	{
		currentSelection = bundle;
		currentSelection.OnSelection();
		bundleImage.sprite = currentSelection.BundleImage;
	}

	public void Equip()
	{
		if (currentSelection.gameObject.TryGetComponent(out WeaponBundle currentWeapon))
		{
			currentWeaponBundle?.OnUnEquip();
			currentWeaponBundle = currentWeapon;
		}

		currentSelection?.OnEquip();
	}
}
