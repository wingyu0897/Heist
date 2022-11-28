using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoManager : MonoBehaviour
{
	[SerializeField]
	private Image clearImage;
	[SerializeField]
	private Sprite successImg;
	[SerializeField]
	private Sprite failImg;
	[SerializeField] 
	private TextMeshProUGUI earnMoneyText;
	[SerializeField] 
	private TextMeshProUGUI playTimeText;
	[SerializeField] 
	private TextMeshProUGUI moneyText;
	[SerializeField]
	private TextMeshProUGUI tipText;

	private void Start()
	{
		tipText.text = StageManager.Instance.currentStageData.tip;
	}

	private void FixedUpdate()
	{
		if (moneyText)
		{
			moneyText.text = $"∫∏¿Ø ¿Á»≠  {string.Format("{0:#,##0$}", PlayerData.Instance.Money)}";
		}
	}

	public void ShowEndStats(bool isSuccess, int money, TimeSpan clearTime)
	{
		playTimeText.text = $"«√∑π¿Ã Ω√∞£  {string.Format("{0:00}", clearTime.Minutes)}:{string.Format("{0:00}", clearTime.Seconds)}";

		if (isSuccess)
		{
			clearImage.sprite = successImg;
			earnMoneyText.text = $"»πµÊ  {string.Format("{0:#,##0}", money)}$";
		}
		else
		{
			clearImage.sprite = failImg;
			earnMoneyText.text = "»πµÊ  0$";
		}
	}
}
