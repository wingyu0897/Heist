using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPackage : Interactable
{
	[Header("Reference")]
	[SerializeField] private PackageSO packageData;
	public int Price => packageData.price;
	public bool canInteract = true;
	public bool isHolding = false;

	public override float InteractionTime => packageData.interactiveTime;
	[SerializeField] private string infoText = "Hold [F] To Grab Package";
	public override string InfoText => infoText;

	private SpriteRenderer spriteRenderer;
	private Collider2D mCollider;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		mCollider = GetComponent<Collider2D>();
		Initialization();
	}

	private void Update()
	{
		Drop();
	}

	public override void Initialization() //초기화
	{
		backPackHolder = StageManager.Instance.player.transform.Find("BackPackHolder").transform;
		transform.parent = null;
		transform.localRotation = Quaternion.identity;
		spriteRenderer.sprite = packageData.defaultSprite;
		isHolding = false;
		mCollider.enabled = true;
	}

	public override bool CanInteractable() //상호작용 가능 여부
	{
		return !isHolding && canInteract && (transform.parent != null ? false : PlayerData.Instance.backPacks == 0 ? true : false);
	}

	public override void OnInteraction() //상호작용할 시
	{
		++PlayerData.Instance.backPacks;
		transform.SetParent(backPackHolder);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		spriteRenderer.sprite = packageData.packedSprite;
		isHolding = true;
		mCollider.enabled = false;
	}

	private void Drop() //가방 내려놓기
	{
		if (transform.parent != null && Input.GetKeyDown(KeyCode.G))
		{
			PlayerData.Instance.backPacks -= 1;
			transform.parent = null;
			transform.position = transform.localPosition - new Vector3(0, 0.5f, 0);
			transform.localRotation = Quaternion.identity;
			isHolding = false;
			mCollider.enabled = true;
		}
	}
}
