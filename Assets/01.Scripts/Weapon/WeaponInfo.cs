using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
	[Header("--Info--")]
	[SerializeField] private TextMeshProUGUI mainText;
	[SerializeField] private TextMeshProUGUI subText;
	[SerializeField] private Material weaponMaterial;

	private WeaponManager weaponManager;
	private IWeaponinfo weaponInfo;

	private void Start()
	{
		weaponManager = GetComponent<WeaponManager>();
	}

	private void Update()
	{
		if (weaponManager.currentWeapon)
		{
			if (weaponInfo == null || weaponInfo.gameObject != weaponManager.currentWeapon.gameObject)
			{
				if (weaponManager.currentWeapon.TryGetComponent(out IWeaponinfo currernWeapon))
				{
					weaponInfo = currernWeapon;
				}
			}

			if (weaponInfo != null)
			{
				mainText.text = weaponInfo?.MainInfo;
				subText.text = weaponInfo?.SubInfo;
				weaponMaterial.SetTexture("_MainTexture", weaponInfo?.WeaponImage);
			}
			else
			{
				mainText.text = "--";
				subText.text = "---";
			}
		}
	}
}
