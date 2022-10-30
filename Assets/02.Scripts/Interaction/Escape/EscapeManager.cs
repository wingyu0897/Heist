using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeManager : Interactable
{
	[SerializeField] private float interactionTime;
	public override float InteractionTime => interactionTime;
	[SerializeField] private List<EscapeCondition> decisionsFirst;
	[SerializeField] private List<EscapeCondition> decisionsSecond;

	public bool canInteract = true;

	public override bool CanInteractable()
	{
		bool conditionFir = false;
		bool conditionSec = false;

		foreach (EscapeCondition de in decisionsFirst)
		{
			conditionFir = de.Result();
		}
		
		foreach (EscapeCondition de in decisionsSecond)
		{
			conditionSec = de.Result();
		}

		return canInteract && (conditionFir || conditionSec);
	}

	public override void Initialization()
	{
		canInteract = true;
	}

	public override void OnInteraction()
	{
		MissionData.Instance.EndTheGame(true);
		canInteract = false;
	}
}
