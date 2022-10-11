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

	public override void Initialization() //�ʱ�ȭ
	{
		transform.parent = null;
		transform.localRotation = Quaternion.identity;
		spriteRenderer.sprite = defaultSprite;
	}

	public override bool CanInteractable() //��ȣ�ۿ� ���� ����
	{
		return transform.parent != null ? false : PlayerData.Instance.backPacks == 0 ? true : false;
	}

	public override void OnInteraction() //��ȣ�ۿ��� ��
	{
		++PlayerData.Instance.backPacks;
		transform.SetParent(backPackHolder);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		spriteRenderer.sprite = packedSprite;
	}

	private void Drop() //���� ��������
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
