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
	[SerializeField] private GameObject resultPanel;
	[SerializeField] private CinemachineVirtualCamera playerCamera;

	[Header("--Properties--")]
	public GameObject player;
	public bool isSilencer = false;
	public bool isDetected = false;
	public bool isLoud = false;

	[Header("--Datas--")]
	public List<Package> gains;

	[Header("--Events--")]
	public UnityEvent OnLouded;
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
	}

	private void CreatePool()
	{
		foreach (PoolingSet ps in poolingList.list)
		{
			PoolManager.Instance.CreatePool(ps.prefab, ps.count);
		}
	}

	public void Louded()
	{
		if (!isLoud)
		{
			isLoud = true;
			OnLouded?.Invoke();
		}
	}

	public void RunTheGame()
	{
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
		int money = 0;

		if (isSuccess)
		{
			foreach (Package elem in gains)
			{
				money += elem.Price;
			}
		}

		resultPanel.transform.Find("ResultPanel").Find("EarnMoney").GetComponent<TextMeshProUGUI>().text = $"Earn Money : {string.Format("{0:#,##0}", money)}";
		OnEndGame?.Invoke();
	}
}
