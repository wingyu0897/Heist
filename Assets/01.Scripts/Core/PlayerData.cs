using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

	[Header("Properties")]
	public bool isRunning = false;
	public bool canRun = true;
	public int backPacks = 0;
	public int interactionSpeed = 1;
	public Slider healthSlider;
	public GameObject interactionUI;

	[Header("Weapon")]
	public Weapon currentWeapon;
	public Weapon primaryWeapon;
	public Weapon secondaryWeapon;
	public Weapon meleeWeapon;

	[SerializeField] private int maxHealth = 1000;
	[SerializeField] private TextMeshProUGUI mainText;
	[SerializeField] private TextMeshProUGUI subText;
	[SerializeField] private Image weaponImage; 
	[SerializeField] private Transform weaponParent;
	public int MaxHealth { get => maxHealth; set => maxHealth = Mathf.Clamp(value, 0, int.MaxValue); }

	private Material weaponMaterial;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	private void FixedUpdate()
	{
		if (currentWeapon) //���� ���� ����
		{
			if (currentWeapon.TryGetComponent(out IWeaponinfo currentInfo))
			{
				mainText.text = currentInfo.MainInfo;
				subText.text = currentInfo.SubInfo;
				weaponMaterial.SetTexture("_MainTexture", currentInfo.WeaponImage);
			}
			else
			{
				mainText.text = "--";
				subText.text = "---";
			}
		}
	}

	public void ReadyGame()
	{
		weaponParent = GameManager.Instance.Player.transform?.Find("WeaponHolder");
		currentWeapon = primaryWeapon;
		weaponMaterial = weaponImage.material;
	}

	public void RunGame()
	{
		CreateWeaponGameObject();
	}

	#region #Setting
	public void SetPrimaryWeapon(Weapon primary = null) //�ֹ��� ����
	{
		primaryWeapon = primary == null ? primaryWeapon : primary;
	}
	
	public void SetSecondaryWeapon(Weapon secondary = null) //�������� ����
	{
		secondaryWeapon = secondary == null ? secondaryWeapon : secondary;
	}
	
	public void SetMeleeWeapon(Weapon melee = null) //�������� ����
	{
		meleeWeapon = melee == null ? meleeWeapon : melee;
	}

	public void SetCurrentWeapon(Weapon weapon) //���� ���� ����
	{
		currentWeapon = weapon;
	}
	#endregion

	public void CreateWeaponGameObject() //���� ����
	{
		primaryWeapon = Instantiate(primaryWeapon.GetPrefab(), weaponParent).GetComponent<Weapon>();
		secondaryWeapon = Instantiate(secondaryWeapon.GetPrefab(), weaponParent).GetComponent<Weapon>();
		meleeWeapon = Instantiate(meleeWeapon.GetPrefab(), weaponParent).GetComponent<Weapon>();
	}
}
