using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
	[Header("--References--")]
    [SerializeField] private float interactableRange;
	[SerializeField] private int interactableLayer;
	[SerializeField] private Image fillImage;
	[SerializeField] private TextMeshProUGUI infoText;
	[SerializeField] private GameObject interactionObject;

	private float time = 0;
	private Vector2 pointer;
	private RaycastHit2D hit;
	private Interactable interactable;

	private void Start()
	{
		interactableLayer = 1 << LayerMask.NameToLayer("Interactable");
	}

	private void Update()
	{
		Interactive();
	}

	private void Interactive()
	{
		hit = Physics2D.Raycast(pointer, Vector2.zero, 10, interactableLayer);

		//��ȣ�ۿ� ���� �Ÿ� ���� ������
		if (Vector3.Distance(transform.position, pointer) <= interactableRange && hit.collider != null)
		{
			//��ȣ�ۿ��ϴ� ������Ʈ�� �ٲ�
			if (hit.collider.gameObject != interactable?.gameObject)
			{
				interactable = hit.collider.GetComponent<Interactable>();
				time = 0;
			}

			//��ȣ�ۿ��� ����
			if (interactable.CanInteractable() == true)
			{
				interactionObject.transform.position = Camera.main.WorldToScreenPoint(pointer);
				interactionObject.SetActive(true);
				infoText.text = interactable?.InfoText;
				fillImage.fillAmount = time / interactable.InteractionTime;

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
			}
			else
			{
				time = 0;
				interactionObject.SetActive(false);
			}
		}
		else
		{
			time = 0;
			interactionObject.SetActive(false);
		}
	}

	//���콺 ������ ������Ʈ
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
