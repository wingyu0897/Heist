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
	[SerializeField] private int timerDefaultTime;
	[SerializeField] private Color StopWatchColor;
	[SerializeField] private Color TimerColor;

    private bool isActive = false;
	private bool isTimer = false;

    private float currentTime;

	private void Start()
	{
		currentTime = 0;
		timeText.color = StopWatchColor;
	}

	private void Update()
	{
		if (isActive)
		{
			currentTime += isTimer ? -Time.deltaTime : Time.deltaTime;

			if (isTimer && currentTime <= 0)
			{
				GameManager.Instance?.EndGame(false);
			}
		}
		TimeSpan time = TimeSpan.FromSeconds(currentTime + 1);
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
			currentTime = timerDefaultTime;
			timeText.color = TimerColor;
		}
		else
		{
			timeText.color = StopWatchColor;
		}
	}
}
