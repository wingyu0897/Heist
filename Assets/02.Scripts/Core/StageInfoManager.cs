using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageInfoManager : MonoBehaviour
{
	[SerializeField] 
	private TextMeshProUGUI earnMoneyText;
	[SerializeField] 
	private TextMeshProUGUI playTimeText;
	[SerializeField] 
	private TextMeshProUGUI moneyText;

	private void FixedUpdate()
	{
		if (moneyText)
		{
			moneyText.text = string.Format("{0:#,##0$}", PlayerData.Instance.Money);
		}
	}

	public void ShowEndStats(bool isSuccess, int money, TimeSpan clearTime)
	{
		playTimeText.text = $"Play Time : {string.Format("{0:00}", clearTime.Minutes)}:{string.Format("{0:00}", clearTime.Seconds)}";

		if (isSuccess)
		{
			earnMoneyText.text = $"Earn Money : {string.Format("{0:#,##0}", money)}$";
		}
		else
		{
			earnMoneyText.text = "0$";
		}
	}
}
