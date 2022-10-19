using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGameObject : Interactable
{
	[SerializeField] private GameObject activeObject;
	[SerializeField] private float interactionTime;
	public override float InteractionTime => interactionTime;


	public override bool CanInteractable()
	{
		return true;
	}

	public override void Initialization()
	{
		activeObject.SetActive(false);
	}

	public override void OnInteraction()
	{
		activeObject.SetActive(true);
	}
}
