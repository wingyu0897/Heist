using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MissionData : MonoBehaviour
{
	public static MissionData Instance;

	[Header("--Properties--")]
	public bool isSilencer = false;
	public bool isDetected = false;
	public bool isLoud = false;

	[Header("--Datas--")]
	public List<Package> gainPackages;

	[Header("--Events--")]
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
		}
		GameManager.Instance?.ReadyGame();
	}

	public void RunTheGame()
	{
		gainPackages = new List<Package>();

		OnRunGame?.Invoke();
	}

	public void EndTheGame()
	{
		OnEndGame?.Invoke();
	}
}
