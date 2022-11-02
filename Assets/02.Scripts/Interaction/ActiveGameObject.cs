using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveGameObject : Interactable
{
	[SerializeField] private float interactionTime;
	public override float InteractionTime => interactionTime;
	[SerializeField] private string infoText = "Hold [F] To Active Object";
	public override string InfoText => infoText;
	[SerializeField] private bool canMultipleInteraction;

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
