using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipPlayer : AudioPlayer
{
	public AudioClip clip;

    public void PlayClipSound()
	{
		PlayClipWithVariablePitch(clip);
	}
}
