using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHacking : Interactable
{
	[SerializeField] private float interactionTime;
	public override float InteractionTime => interactionTime;
	[SerializeField] private Hacking hacking;

	private bool isClosed = false;

	public override bool CanInteractable()
	{
		return !isClosed;
	}

	public override void Initialization()
	{
		
	}

	public override void OnInteraction()
	{
		hacking.StartHacking();
		isClosed = true;
	}
}
