using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponRenderer : MonoBehaviour
{
	[Header("Order in Layer")]
	[SerializeField][Tooltip("플레이어의 Order in Layer의 값")]
	private int playerLayer;
	[SerializeField][Tooltip("이 값보다 높은 각도일 때 플레이어의 뒤로 이동")]
	private float minAngle;
	[SerializeField][Tooltip("이 값보다 낮은 각도일 때 플레이어의 뒤로 이동")]
	private float maxAngle;

	private SpriteRenderer mySpriteRenderer;

	public void Init()
	{
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void LayerOrder(float angle) //angle에 맞추어 스프라이트의 위치를 앞, 뒤로 이동
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
