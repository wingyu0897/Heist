using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float interactableRange;
	[SerializeField] private GameObject interactionUI;
	private Image keyUI;

	private int layerMask;
	private float time = 0;
	private Vector2 pointer;
	private RaycastHit2D hit;
	private Interactable interactable;

	private void Start()
	{
		keyUI = interactionUI.transform.GetComponentInChildren<Image>();
		layerMask = 1 << LayerMask.NameToLayer("Interactable");
	}

	private void Update()
	{
		Interactive();
	}

	private void Interactive()
	{
		hit = Physics2D.Raycast(pointer, Vector2.zero, 10, layerMask);
		if (Vector3.Distance(transform.position, pointer) <= interactableRange && hit.collider != null)
		{
			interactable = hit.collider.GetComponent<Interactable>();
			keyUI.fillAmount = time / interactable.InteractionTime;
			if (hit.collider.gameObject != interactable?.gameObject)
			{
				time = 0;
			}
			if (interactable.CanInteractable() == true)
			{
				if (Input.GetKey(KeyCode.F))
				{
					time += Time.deltaTime;
					if (time >= interactable.InteractionTime)
					{
						interactable.OnInteraction();
						time = 0;
					}
				}
				else
				{
					time = 0;
				}
				interactionUI.SetActive(true);
				interactionUI.transform.position = Camera.main.WorldToScreenPoint(pointer);
			}
			else
			{
				interactionUI.SetActive(false);
				time = 0;
			}
		}
		else
		{
			interactionUI.SetActive(false);
			time = 0;
		}
	}

	public void PointerPosition(Vector2 pointer)
	{
		this.pointer = pointer;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, interactableRange);
		Gizmos.color = Color.white;
	}
}
