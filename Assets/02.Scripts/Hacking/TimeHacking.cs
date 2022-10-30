using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeHacking : Hacking
{
	[Tooltip("기본값은 1f/sec 입니다")]
	[SerializeField] private Transform slider;
	[SerializeField] private float maxValue;
	[SerializeField] private float progress = 0;

	public bool isActive = false;

	public UnityEvent OnStartHacking;
	public UnityEvent OnComplete;

	private void OnEnable()
	{
		Initialize();
	}

	private void Update()
	{
		if (isActive)
		{
			progress += Time.deltaTime;
			progress = Mathf.Clamp(progress, 0, maxValue);
			slider.localScale = new Vector3(progress / maxValue, slider.localScale.y);

			if (progress >= maxValue)
			{
				OnComplete?.Invoke();
			}
		}
	}

	public override void Initialize()
	{
		isActive = false;
		progress = 0;
		slider.localScale = new Vector3(0, slider.localScale.y);
	}

	public override void StartHacking()
	{
		isActive = true;

		OnStartHacking?.Invoke();
	}
}
