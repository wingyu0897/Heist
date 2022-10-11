using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : Interactable
{
	[Header("Reference")]
	[SerializeField] private float interactionTime;
	[SerializeField] private Sprite defaultSprite;
	[SerializeField] private Sprite packedSprite;
	public override float InteractionTime => interactionTime;

	private SpriteRenderer spriteRenderer;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Initialization();
	}

	private void Update()
	{
		Drop();
	}

	public override void Initialization() //초기화
	{
		transform.parent = null;
		transform.localRotation = Quaternion.identity;
		spriteRenderer.sprite = defaultSprite;
	}

	public override bool CanInteractable() //상호작용 가능 여부
	{
		return transform.parent != null ? false : PlayerData.Instance.backPacks == 0 ? true : false;
	}

	public override void OnInteraction() //상호작용할 시
	{
		++PlayerData.Instance.backPacks;
		transform.SetParent(backPackHolder);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		spriteRenderer.sprite = packedSprite;
	}

	private void Drop() //가방 내려놓기
	{
		if (transform.parent != null && Input.GetKeyDown(KeyCode.G))
		{
			PlayerData.Instance.backPacks -= 1;
			transform.parent = null;
			transform.position = transform.localPosition - new Vector3(0, 0.5f, 0);
			transform.localRotation = Quaternion.identity;
		}
	}
}
