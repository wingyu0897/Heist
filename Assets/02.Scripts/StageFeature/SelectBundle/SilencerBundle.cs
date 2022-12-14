using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SilencerBundle : Bundle
{
	[SerializeField] private bool canUnEquip;
	[SerializeField] private Sprite bundleImage;
	[SerializeField] private int price;
	public override bool CanUnEquip => canUnEquip;
	public override Sprite BundleImage => bundleImage;

	[SerializeField] private bool isEquiped = false;
	private Image equipChecker;
	public override bool IsEquiped => isEquiped;

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
