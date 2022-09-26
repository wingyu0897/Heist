using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : AudioPlayer
{
	public void PlayClips(AudioClip clip)
	{
		PlayClipWithVariablePitch(clip);
	}
}
