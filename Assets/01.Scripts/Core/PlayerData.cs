using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]
//public class SaveData
//{
//	public int money;
//}

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

	[Header("--Data--")]
	[SerializeField] private int money;
	public int Money 
	{
		get => money;
		set => money = Mathf.Clamp(value, 0, 999999999);
	}

	[Header("--Properties--")]
	public bool isRunning = false;
	public bool canRun = true;
	public int backPacks = 0;

	//[Header("--Weapon--")]
	//public Weapon currentWeapon;
	//public Weapon primaryDefault;
	//public Weapon secondaryDefault;
	//public Weapon meleeDefault;

	//public Weapon primaryWeapon;
	//public Weapon secondaryWeapon;
	//public Weapon meleeWeapon;

	[SerializeField] private int maxHealth = 1000;
	//[SerializeField] private Transform weaponParent;
	public int MaxHealth { get => maxHealth; set => maxHealth = Mathf.Clamp(value, 0, int.MaxValue); }

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
			AddMoney(0);
		}
	}

	public void ReadyGame()
	{
		//weaponParent = MissionData.Instance.player.transform?.Find("WeaponHolder");
		//currentWeapon = primaryWeapon;
		//weaponMaterial = weaponImage.material;

		//primaryWeapon = primaryDefault;
		//secondaryWeapon = secondaryDefault;
		//meleeWeapon = meleeDefault;
	}

	public void RunGame()
	{
		//CreateWeaponGameObject();
	}

	#region #Setting
	//public void SetPrimaryWeapon(Weapon primary = null) //주무기 설정
	//{
	//	primaryWeapon = primary == null ? primaryWeapon : primary;
	//}
	
	//public void SetSecondaryWeapon(Weapon secondary = null) //보조무기 설정
	//{
	//	secondaryWeapon = secondary == null ? secondaryWeapon : secondary;
	//}
	
	//public void SetMeleeWeapon(Weapon melee = null) //근접무기 설정
	//{
	//	meleeWeapon = melee == null ? meleeWeapon : melee;
	//}

	//public void SetCurrentWeapon(Weapon weapon) //현재 무기 설정
	//{
	//	currentWeapon = weapon;
	//}
	#endregion

	public void AddMoney(int value)
	{
		Money = money + value;
	}
}
