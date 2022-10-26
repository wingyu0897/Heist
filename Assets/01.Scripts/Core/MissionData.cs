using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class MissionData : MonoBehaviour
{
	public static MissionData Instance;

	[Header("--References--")]
	[SerializeField] private PoolingListSO poolingList;
	[SerializeField] private CinemachineVirtualCamera playerCamera;
	[SerializeField] private TextMeshProUGUI earnMoney;
	[SerializeField] private TextMeshProUGUI moneyText;

	[Header("--Properties--")]
	public GameObject player;
	public bool isSilencer = false;
	public bool isDetected = false;
	public bool isLoud = false;

	[Header("--Datas--")]
	public List<Package> gains;

	[Header("--Events--")]
	public UnityEvent OnLouded;
	public UnityEvent OnReadyGame;
	public UnityEvent OnRunGame;
	public UnityEvent OnEndGame;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("ERROR: Multiple MissionData on running");
		}
		else
		{
			Instance = this;

			if (PoolManager.Instance == null)
			{
				PoolManager.Instance = new PoolManager(GameManager.Instance.transform);
			}

			try
			{
				GameManager.Instance.ReadyGame();
			}
			catch
			{
				Debug.Log("ERROR:MissionData: Missing GameManager");
			}
		}
	}

	private void Start()
	{
		CreatePool();
		player.SetActive(false);
		OnReadyGame?.Invoke();
	}

	private void CreatePool()
	{
		foreach (PoolingSet ps in poolingList.list)
		{
			PoolManager.Instance.CreatePool(ps.prefab, ps.count);
		}
	}

	private void FixedUpdate()
	{
		moneyText.text = string.Format("{0:#,##0$}", PlayerData.Instance.Money);
	}

	public void Louded()
	{
		if (!isLoud)
		{
			isLoud = true;
			OnLouded?.Invoke();
		}
	}

	public void StartGame()
	{
		GameManager.Instance.StartGame();

		player.SetActive(true);
		playerCamera.Priority = 11;

		gains = new List<Package>();


		OnRunGame?.Invoke();
	}

	public void EndTheGame(bool isSuccess)
	{
		PoolManager.Instance.DestroyPools();

		player.SetActive(false);
		playerCamera.Priority = 9;

		earnMoney.text = $"Earn Money : {string.Format("{0:#,##0}", EarnedMoney())}";
		OnEndGame?.Invoke();
	}

	public int EarnedMoney()
	{
		int money = 0;

		foreach (Package elem in gains)
		{
			money += elem.Price;
		}

		return money;
	}
}
