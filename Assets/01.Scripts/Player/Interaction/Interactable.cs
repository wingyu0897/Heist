using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	protected Transform backPackHolder;

	public virtual void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer("Interactable");
	}

	public abstract float InteractionTime { get; }

	public abstract void Initialization();

	public abstract void OnInteraction();

	public abstract bool CanInteractable();
}
