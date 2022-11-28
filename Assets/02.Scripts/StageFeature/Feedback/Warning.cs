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

		Tween sirenOut = DOTween.To(() => siren.anchoredPosition, x => siren.anchoredPosition = x, new Vector2(0, -(Screen.height * 0.5f)), 2f);
		Tween sirenIn = DOTween.To(() => siren.anchoredPosition, x => siren.anchoredPosition = x, Vector2.zero, 1f);

		Sequence seq = DOTween.Sequence();
		print(siren.position.x);
		seq.Append(sirenOut);
		seq.AppendInterval(sirenDuration);
		seq.Append(sirenIn);
	}
}
