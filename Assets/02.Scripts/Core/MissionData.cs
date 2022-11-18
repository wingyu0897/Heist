using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.UI;

public class MissionData : MonoBehaviour
{
	public static MissionData Instance;

	[Header("--References--")]
	[SerializeField] private PoolingListSO poolingList;
	[SerializeField] private CinemachineVirtualCamera playerCamera;
	[SerializeField] private TextMeshProUGUI earnMoney;
	[SerializeField] private TextMeshProUGUI moneyText;
	[SerializeField] private Slider detectGaugeSlider;

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

		DetectGauge();
	}

	private void FixedUpdate()
	{
		if (moneyText)
		{
			moneyText.text = string.Format("{0:#,##0$}", PlayerData.Instance.Money);
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

	public void StartGame()
	{
		GameManager.Instance.StartGame();

		player.SetActive(true);
		playerCamera.Priority = 11;

		gains = new List<PickUpPackage>();


		OnRunGame?.Invoke();
	}

	public void EndTheGame(bool isSuccess)
	{
		GameManager.Instance.EndGame();
		PoolManager.Instance.DestroyPools();

		player.SetActive(false);
		playerCamera.Priority = 9;


		if (isSuccess)
		{
			earnMoney.text = $"Earn Money : {string.Format("{0:#,##0}", EarnedMoney())}";
			OnGameSuccess?.Invoke();
		}
		else
		{
			earnMoney.text = "0";
			OnGameFailure?.Invoke();
		}

		OnEndGame?.Invoke();
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

	private void DetectGauge()
	{
		if (detectGauge > 0 && !isLoud)
		{
			detectGaugeSlider?.gameObject.SetActive(true);
		}
		else
		{
			detectGaugeSlider?.gameObject.SetActive(false);
		}

		isDetecting = false;

		if (detectGaugeSlider)
		{
			detectGaugeSlider.value = detectGauge / detectTime;
		}
	}

	public void DetectInput(float value)
	{
		isDetecting = true;

		if (value > detectGauge)
		{
			detectGauge = value;
		}

		detectGauge = Mathf.Clamp(detectGauge, 0, detectTime);
	}
}
