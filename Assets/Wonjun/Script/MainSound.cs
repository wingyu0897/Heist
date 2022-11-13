using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainSound : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float sliderVal)
    {
        mixer.SetFloat("LobbySoundRara", Mathf.Log10(sliderVal) * 20);
    }
}
