using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : Interactable
{
	[SerializeField]
	private float time = 1f;
	public override float InteractionTime => time;
	[SerializeField]
	private string infoText;
	public override string InfoText => infoText;

	private bool isOpen = false;

	private Collider2D col;

	public override void Awake()
	{
		base.Awake();
		col = GetComponent<Collider2D>();
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
		MissionData.Instance.nodeScanner.ScanNodes();
	}
}
