using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPackage : Interactable
{
	[Header("Reference")]
	[SerializeField] 
	private PackageSO packageData;
	public int Price => packageData.price;
	public bool canInteract = true;
	public bool isHolding = false;
	public override float InteractionTime => packageData.interactiveTime;
	[SerializeField] 
	private string infoText = "Hold [F] To Grab Package";
	public override string InfoText => infoText;

	protected PackageHolder holder;
	protected SpriteRenderer spriteRenderer;
	protected Collider2D mCollider;

	protected virtual void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		mCollider = GetComponent<Collider2D>();
		Initialization();
	}

	protected void Update()
	{
		Drop();
	}

	public override void Initialization() //�ʱ�ȭ
	{
		holder = StageManager.Instance.player.transform.Find("BackPackHolder").GetComponent<PackageHolder>();
		transform.parent = null;
		transform.localRotation = Quaternion.identity;
		isHolding = false;
		mCollider.enabled = true;
	}

	public override bool CanInteractable() //��ȣ�ۿ� ���� ����
	{
		return !isHolding && canInteract && (transform.parent != null ? false : PlayerData.Instance.backPacks < PlayerData.Instance.maxPack ? true : false);
	}

	public override void OnInteraction() //��ȣ�ۿ��� ��
	{
		++PlayerData.Instance.backPacks;
		holder.packs.Add(this);
		transform.SetParent(holder.transform);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		spriteRenderer.sprite = packageData.packedSprite;
		isHolding = true;
		mCollider.enabled = false;
	}

	protected void Drop() //���� ��������
	{
		if (transform.parent != null && Input.GetKeyDown(KeyCode.G))
		{
			PlayerData.Instance.backPacks -= 1;
			holder.packs.Remove(this);
			transform.parent = null;
			transform.position = transform.localPosition - new Vector3(0, 0.5f, 0);
			transform.localRotation = Quaternion.identity;
			isHolding = false;
			mCollider.enabled = true;
		}
	}
}
