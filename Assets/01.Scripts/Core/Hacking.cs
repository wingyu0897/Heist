using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hacking : MonoBehaviour
{
	[Tooltip("기본값은 1f/sec 입니다")]
	[SerializeField] private float maxValue;
	[SerializeField] private Transform slider;
	[SerializeField] private float progress = 0;

	public bool isActive = false;

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

	private void Initialize()
	{
		isActive = false;
		progress = 0;
		slider.localScale = new Vector3(0, slider.localScale.y);
	}

	public void Active()
	{
		isActive = true;
	}
}
