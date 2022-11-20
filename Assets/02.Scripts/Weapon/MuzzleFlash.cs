using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class MuzzleFlash : MonoBehaviour
{
    public Light2D flash;
    [SerializeField]
    private float lightOnDelay;
    [SerializeField]
    private float lightOffDelay;

    public void ToggleLight()
	{
        StopAllCoroutines();
        StartCoroutine(SetLight(lightOnDelay, true,
            () => StartCoroutine(SetLight(lightOffDelay, false))
        ));
	}

    IEnumerator SetLight(float time, bool result, Action CallBack = null)
	{
        yield return new WaitForSeconds(time);
        flash.enabled = result;
        CallBack?.Invoke();
	}
}
