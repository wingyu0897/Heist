using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cmCam;
	[SerializeField]
	private float duration = 0.2f;

	private CinemachineBasicMultiChannelPerlin noise;

	private void Awake()
	{
		noise = cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
	}

	public void ShakeCamera(float amlitude)
	{
		noise.m_AmplitudeGain = amlitude;
		StartCoroutine(Shake(amlitude));
	}

	IEnumerator Shake(float amlitude)
	{
		float time = duration;

		while (time > 0)
		{
			noise.m_AmplitudeGain = Mathf.Lerp(0, amlitude, time / duration);
			yield return null;
			time -= Time.deltaTime;
		}
		noise.m_AmplitudeGain = 0;
	}
}
