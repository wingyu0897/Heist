using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	private bool isButtonDown = false;

	public List<GameObject> resultObject;
	public List<RaycastResult> resultList;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{

		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isButtonDown = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isButtonDown = false;
	}
}
