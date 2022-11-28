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
	[SerializeField] 
	private int money;
	public int Money 
	{
		get => money;
		set => money = Mathf.Clamp(value, 0, 999999999);
	}
	public int clearStageCount;

	[Header("--Properties--")]
	public bool isRunning = false;
	public bool canRun = true;
	public int backPacks = 0;
	public int maxPack = 2;

	[SerializeField] 
	private int maxHealth = 1000;
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
		}
	}

	public void AddMoney(int value)
	{
		Money = money + value;
	}

	public bool UseMoney(int value)
	{
		return money >= value;
	}
}
