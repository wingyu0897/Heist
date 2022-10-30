using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveGameObject : Interactable
{
	[SerializeField] private float interactionTime;
	[SerializeField] private bool canMultipleInteraction;
	public override float InteractionTime => interactionTime;

	private bool canInteract = true;

	public UnityEvent OnInteract;


	public override bool CanInteractable()
	{
		return canInteract;
	}

	public override void Initialization()
	{
		canInteract = true;
	}

	public override void OnInteraction()
	{
		if (!canMultipleInteraction)
		{
			canInteract = false;
		}
		OnInteract?.Invoke();
	}
}
