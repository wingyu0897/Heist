using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIBrain))]
public class CCTVBreak : Interactable
{
	[SerializeField]
	private float interactionTime = 3f;
	public override float InteractionTime => interactionTime;
	[SerializeField]
	private string infoText;
	public override string InfoText => infoText;

	private AIBrain brain;

	private void Start()
	{
		brain = GetComponent<AIBrain>();
	}

	public override bool CanInteractable()
	{
		return !brain.IsDead && brain.enabled;
	}

	public override void Initialization()
	{
		
	}

	public override void OnInteraction()
	{
		brain.Dead();
	}
}
