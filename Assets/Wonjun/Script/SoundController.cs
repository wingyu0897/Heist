using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public AudioMixer effectMixer;
    public Slider effectSlider;
    public AudioMixer bgmMixer;
    public Slider bgmSlider;

    public static float effectVolume;
    public static float bgmVolume;

	private void Start()
	{
		effectVolume = DataManager.Instance.nowData.effectVolume;
		bgmVolume = DataManager.Instance.nowData.bgmVolume;
        effectSlider.value = effectVolume;
        bgmSlider.value = bgmVolume;
	}

	private void FixedUpdate()
	{
        if (effectSlider.value != effectVolume)
		{
            SetSoundEffectMixer(effectVolume);
		}
        if (bgmSlider.value != bgmVolume)
		{
            SetBGMMixer(bgmVolume);
		}
	}

	public void SetSoundEffectMixer(float sliderVal)
    {
        effectSlider.value = sliderVal;
        effectVolume = sliderVal;
        effectMixer.SetFloat("LobbySoundRara", Mathf.Log10(sliderVal) * 20);
        DataManager.Instance?.SaveData();
    }

    public void SetBGMMixer(float sliderVal)
    {
        bgmSlider.value = sliderVal;
        bgmVolume = sliderVal;
        bgmMixer.SetFloat("BGM", Mathf.Log10(sliderVal) * 20);
        DataManager.Instance?.SaveData();
    }
}
