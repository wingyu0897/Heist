using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponRenderer : MonoBehaviour
{
	[Header("Order in Layer")]
	[SerializeField][Tooltip("�÷��̾��� Order in Layer�� ��")]
	private int playerLayer;
	[SerializeField][Tooltip("�� ������ ���� ������ �� �÷��̾��� �ڷ� �̵�")]
	private float minAngle;
	[SerializeField][Tooltip("�� ������ ���� ������ �� �÷��̾��� �ڷ� �̵�")]
	private float maxAngle;

	private SpriteRenderer mySpriteRenderer;

	public void Init()
	{
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void LayerOrder(float angle) //angle�� ���߾� ��������Ʈ�� ��ġ�� ��, �ڷ� �̵�
	{
		if (mySpriteRenderer != null)
		{
			if (angle > minAngle && angle < maxAngle)
			{
				mySpriteRenderer.sortingOrder = playerLayer - 1;
			}
			else
			{
				mySpriteRenderer.sortingOrder = playerLayer + 1;
			}
		}
	}
}
