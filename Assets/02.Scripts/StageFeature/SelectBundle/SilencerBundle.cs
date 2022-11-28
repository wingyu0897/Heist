using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SilencerBundle : Bundle
{
	[SerializeField] 
	private bool canUnEquip;
	public override bool CanUnEquip => canUnEquip;
	[SerializeField] 
	private Sprite bundleImage;
	public override Sprite BundleImage => bundleImage;
	[SerializeField]
	private string bundleName;
	public override string BundleName => bundleName;
	[SerializeField]
	private string bundleDesc;
	public override string BundleDesc => bundleDesc;
	[SerializeField] 
	private int price;
	public override int Price => price;

	[SerializeField] 
	private bool isEquiped = false;
	public override bool IsEquiped => isEquiped;
	private Image equipChecker;


	private void Awake()
	{
		equipChecker = transform.Find("Image")?.GetComponent<Image>();
	}

	public void Initialize()
	{
		isEquiped = false;
		equipChecker.color = Color.grey;

		StageManager.Instance.isSilencer = false;
	}

	public override void OnEquip()
	{
		if (!StageManager.Instance.isSilencer)
		{
			if (PlayerData.Instance.UseMoney(price))
			{
				PlayerData.Instance.AddMoney(-price);
				StageManager.Instance.isSilencer = true;
				isEquiped = true;
				equipChecker.color = Color.white;
			}
		}
	}

	public override void OnUnEquip()
	{
		isEquiped = false;
		equipChecker.color = Color.grey;

		if (StageManager.Instance.isSilencer)
		{
			PlayerData.Instance?.AddMoney(+price);
			StageManager.Instance.isSilencer = false;
		}
	}

	public override void OnSelection()
	{

	}

	public override bool CanEquip()
	{
		return !StageManager.Instance.isSilencer && PlayerData.Instance.UseMoney(price);
	}
}
