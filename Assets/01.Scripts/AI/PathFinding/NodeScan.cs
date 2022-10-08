using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScan : MonoBehaviour
{
	public Vector2Int bottomLeft;
	public Vector2Int topRight;
	public LayerMask obstacleLayer;

	public int sizeX;
	public int sizeY;
	public Node[,] nodeArray;

	[Header("NodeDesign")]
	[SerializeField] private bool drawGizmos = true;
	[Header("Path / 지나갈 수 있는 노드")]
	[SerializeField] private Color pathNodeColor;
	[SerializeField] private Vector2 pathNodeSize;
	[Header("Wall / 지나갈 수 없는 노드")]
	[SerializeField] private Color wallNodeColor;
	[SerializeField] private Vector2 wallNodeSize;

	private void Awake()
	{
		ScanNodes();
	}

	private void ScanNodes()
	{
		//nodeArray의 사이즈 지정, isWall, x, y 대입
		sizeX = topRight.x - bottomLeft.x + 1;
		sizeY = topRight.y - bottomLeft.y + 1;
		nodeArray = new Node[sizeX, sizeY];

		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				bool isWall = false;
				foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(bottomLeft.x + i, bottomLeft.y + j), 0.45f, obstacleLayer))
				{
					if (col)
					{
						isWall = true;
					}
				}

				nodeArray[i, j] = new Node(isWall, bottomLeft.x + i, bottomLeft.y + j);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					Vector2 position = new Vector2(nodeArray[i, j].x, nodeArray[i, j].y);
					Gizmos.DrawWireCube(position, new Vector2(1f, 1f));
					if (nodeArray[i, j].isWall)
					{
						Gizmos.color = wallNodeColor;
						Gizmos.DrawCube(position, wallNodeSize);
						Gizmos.color = Color.white;
					}
					else
					{
						Gizmos.color = pathNodeColor;
						Gizmos.DrawCube(position, pathNodeSize);
						Gizmos.color = Color.white;
					}
				}
			}
		}
	}
}
