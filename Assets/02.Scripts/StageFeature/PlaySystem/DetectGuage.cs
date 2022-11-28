using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectGuage : MonoBehaviour
{
	[Header("--Slider--")]
    [SerializeField]
    private Slider guageSlider;

	[Header("--Icon--")]
	[SerializeField]
	private Image detectIcon;
	[SerializeField]
	private Sprite qusMark;
	[SerializeField]
	private Sprite exclMark;

	private float detectTime;

	private void Start()
	{
		detectTime = StageManager.Instance.detectTime;
	}

	public void DetectInput(float value)
	{
		StageManager.Instance.isDetecting = true;

		if (value > StageManager.Instance.detectGauge)
		{
			StageManager.Instance.detectGauge = value;
		}

		StageManager.Instance.detectGauge = Mathf.Clamp(StageManager.Instance.detectGauge, 0, detectTime);
	}

	public void Guage(float detectGauge)
	{
		if (detectGauge > 0 && !StageManager.Instance.isLoud)
		{
			guageSlider?.gameObject.SetActive(true);
		}
		else
		{
			guageSlider?.gameObject.SetActive(false);
		}

		if (guageSlider)
		{
			guageSlider.value = detectGauge / detectTime;
		}

		if (detectGauge >= detectTime)
		{
			detectIcon.sprite = exclMark;
		}
		else
		{
			detectIcon.sprite = qusMark;
		}
	}
}
