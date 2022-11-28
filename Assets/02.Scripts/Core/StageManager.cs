using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.UI;
using System;

public class StageManager : MonoBehaviour
{
	public static StageManager Instance;

	[Header("--References--")]
	[SerializeField] 
	private PoolingListSO poolingList;
	[SerializeField] 
	private CinemachineVirtualCamera playerCamera;
	public StageSO currentStageData;
	public DetectGuage detectManager;

	private StageInfoManager endManager;

	[Header("--Parameters--")]
	public float detectTime;
	public float detectGauge = 0f;

	[Header("--Properties--")]
	public GameObject player;
	public NodeScan nodeScanner;
	public bool isSilencer = false;
	public bool isDetected = false;
	public bool isDetecting = false;
	public bool isLoud = false;
	public bool isEnd = false;

	[Header("--Datas--")]
	public List<PickUpPackage> gains;

	[Header("--Events--")]
	public UnityEvent OnLouded;
	public UnityEvent OnReadyGame;
	public UnityEvent OnRunGame;
	public UnityEvent OnEndGame;
	public UnityEvent OnGameFailure;
	public UnityEvent OnGameSuccess;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("ERROR: Multiple MissionData on running");
		}
		else
		{
			Instance = this;

			try
			{
				GameManager.Instance.ReadyGame();
			}
			catch
			{
				Debug.Log("ERROR:MissionData: Missing GameManager");
			}
		}

		endManager = GetComponent<StageInfoManager>();
		detectManager = GetComponent<DetectGuage>();
	}

	private void Start()
	{
		if (PoolManager.Instance == null)
		{
			PoolManager.Instance = new PoolManager(GameManager.Instance.transform);
		}
		CreatePool();
		if (player)
		{
			player?.SetActive(false);
		}
		OnReadyGame?.Invoke();
	}

	private void CreatePool()
	{
		foreach (PoolingSet ps in poolingList.list)
		{
			PoolManager.Instance.CreatePool(ps.prefab, ps.count);
		}
	}

	private void LateUpdate()
	{
		if (!isDetecting && !isDetected && !isLoud)
		{
			detectGauge -= Time.deltaTime * 0.5f;
			detectGauge = Mathf.Clamp(detectGauge, 0, detectTime);
		}

		isDetecting = false;

		detectManager.Guage(detectGauge);
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
		player.SetActive(true);
		playerCamera.Priority = 11;

		gains = new List<PickUpPackage>();

		OnRunGame?.Invoke();

		GameManager.Instance.StartGame();
	}

	public void EndTheGame(bool isSuccess)
	{
		isEnd = true;

		player.SetActive(false);
		playerCamera.Priority = 9;

		float playTime = GetComponent<StopWatch>().playTime;
		TimeSpan time = TimeSpan.FromSeconds(playTime + 1);
		endManager.ShowEndStats(isSuccess, EarnedMoney(), time);

		if (isSuccess)
		{
			PlayerData.Instance?.AddMoney(EarnedMoney());
			if (PlayerData.Instance.clearStageCount == currentStageData.stageNumber)
				PlayerData.Instance.clearStageCount++;
			OnGameSuccess?.Invoke();
		}
		else
		{
			OnGameFailure?.Invoke();
		}

		OnEndGame?.Invoke();

		GameManager.Instance.EndGame();
		PoolManager.Instance.DestroyPools();
	}

	public int EarnedMoney()
	{
		int money = 0;

		foreach (PickUpPackage elem in gains)
		{
			money += elem.Price;
		}

		return money;
	}
}
