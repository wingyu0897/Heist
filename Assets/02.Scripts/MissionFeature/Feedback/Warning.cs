using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class Warning : MonoBehaviour
{
	[Header("--Reference--")]
    [SerializeField]
    private Image redScreen;
    [SerializeField]
    private RectTransform siren;
    [SerializeField]
    private AudioClip warningSound;

	[Header("--Setting--")]
	[SerializeField]
	private int redscrLoops;
	[SerializeField]
	private float sirenDuration = 5f;

    private AudioSource _audio;

	private void Awake()
	{
		_audio = GetComponent<AudioSource>();
	}

	public void LoudWarning()
	{
		redScreen.gameObject.SetActive(true);
		siren.gameObject.SetActive(true);
		_audio.PlayOneShot(warningSound);
		redScreen.DOFade(0f, 2f).SetLoops(redscrLoops, LoopType.Yoyo);

		Sequence seq = DOTween.Sequence();
		seq.Append(siren.DOMoveX(siren.sizeDelta.x, 1f));
		seq.AppendInterval(sirenDuration);
		seq.Append(siren.DOMoveX(0, 1f));
	}
}
