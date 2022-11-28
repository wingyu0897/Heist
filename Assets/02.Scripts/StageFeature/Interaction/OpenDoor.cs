using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : Interactable
{
	[SerializeField]
	private float time = 1f;
	public override float InteractionTime => time;
	[SerializeField]
	private bool openAtLoud = true;
	[SerializeField]
	private string infoText;
	public override string InfoText => infoText;
	[SerializeField]
	private Sprite openImage;

	private bool isOpen = false;

	[SerializeField]
	private Collider2D col;

	private SpriteRenderer spriteRen;

	public override void Awake()
	{
		base.Awake();
		spriteRen = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (StageManager.Instance.isLoud && openAtLoud)
		{
			col.enabled = false;
			spriteRen.sprite = openImage;
		}
	}

	public override bool CanInteractable()
	{
		return !isOpen;
	}

	public override void Initialization()
	{
		isOpen = false;
		col.enabled = true;
	}

	public override void OnInteraction()
	{
		isOpen = true;
		col.enabled = false;
		spriteRen.sprite = openImage;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.TryGetComponent(out Enemy enemy))
		{
			OnInteraction();
		}
	}
}
