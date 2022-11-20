using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StopWatch : MonoBehaviour
{
	[Header("--Reference--")]
	[SerializeField] private TextMeshProUGUI timeText;

	[Header("--Properties--")]
	[SerializeField] 
	private int timerDefaultTime;
	[SerializeField] 
	private Color StopWatchColor;
	[SerializeField] 
	private Color TimerColor;

    private bool isActive = false;
	private bool isTimer = false;

    public float playTime;
	private float timerTime;

	private void Start()
	{
		playTime = 0;
		timeText.color = StopWatchColor;
	}

	private void Update()
	{
		if (isActive)
		{
			playTime += Time.deltaTime;
			timerTime += isTimer ? -Time.deltaTime : 0;

			if (isTimer && timerTime <= 0)
			{
				MissionData.Instance?.EndTheGame(false);
			}
		}
		TimeSpan time = TimeSpan.FromSeconds((isTimer ? timerTime : playTime) + 1);
		timeText.text = $"{time.Minutes} : {time.Seconds}";
	}

	public void SetStopWatch(bool active)
	{
		isActive = active;
	}

	public void Timer(bool isTimer)
	{
		this.isTimer = isTimer;

		if (this.isTimer)
		{
			timerTime = timerDefaultTime;
			timeText.color = TimerColor;
		}
		else
		{
			timeText.color = StopWatchColor;
		}
	}
}
