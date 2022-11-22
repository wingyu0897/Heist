using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public AudioMixer effectMixer;
    public AudioMixer bgmMixer;

    public void SetSoundEffectMixer(float sliderVal)
    {
        effectMixer.SetFloat("LobbySoundRara", Mathf.Log10(sliderVal) * 20);
    }

    public void SetBGMMixer(float sliderVal)
    {
        bgmMixer.SetFloat("BGM", Mathf.Log10(sliderVal) * 20);
    }
}
