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

	[SerializeField]
	private Collider2D col;

	public override void Awake()
	{
		base.Awake();
	}

	private void Update()
	{
		if (MissionData.Instance.isLoud)
		{
			col.enabled = false;
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
		MissionData.Instance.nodeScanner.ScanNodes();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.TryGetComponent(out Enemy enemy))
		{
			OnInteraction();
		}
	}
}
