using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StopWatch : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI timeText;

    bool isActive = false;
    private float currentTime;

	private void Start()
	{
		currentTime = 0;
	}

	private void Update()
	{
		if (isActive)
		{
			currentTime += Time.deltaTime;
		}
		TimeSpan time = TimeSpan.FromSeconds(currentTime);
		timeText.text = $"{time.Minutes} : {time.Seconds}";
	}

	public void SetStopWatch(bool active)
	{
		isActive = active;
	}
}
