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
	[SerializeField] private GameObject resultPanel;
	[SerializeField] private CinemachineVirtualCamera playerCamera;

	[Header("--Properties--")]
	public bool isSilencer = false;
	public bool isDetected = false;
	public bool isLoud = false;

	[Header("--Datas--")]
	public List<Package> gainPackages;

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
		playerCamera.Priority = 11;

		gainPackages = new List<Package>();


		OnRunGame?.Invoke();
	}

	public void EndTheGame(bool isSuccess)
	{
		playerCamera.Priority = 9;
		int money = 0;

		if (isSuccess)
		{
			foreach (Package elem in gainPackages)
			{
				money += elem.Price;
			}
		}

		resultPanel.transform.Find("ResultPanel").Find("EarnMoney").GetComponent<TextMeshProUGUI>().text = $"Earn Money : {string.Format("{0:#,##0}", money)}";
		OnEndGame?.Invoke();
	}
}
