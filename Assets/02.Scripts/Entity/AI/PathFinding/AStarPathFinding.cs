using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeOld
{
	public int x, y;
	public int g, h;
	public bool isWall;
	public NodeOld parentNode;

	public NodeOld(bool isWall, int x, int y)
	{
		this.isWall = isWall;
		this.x = x;
		this.y = y;
	}

	public int F { get => g + h; }
}

public class AStarPathFinding : MonoBehaviour
{
	public Vector2Int bottomLeft;
	public Vector2Int topRight;
	public Vector2Int startPos;
	public Vector2Int targetPos;
	public List<NodeOld> finalNodeList;
	public bool allowDiagonal;
	public bool dontCrossCorner;

	[SerializeField] private LayerMask wallLayer;

	private int sizeX, sizeY;
	private NodeOld[,] nodeArray;
	private NodeOld startNode, targetNode, currentNode;
	private List<NodeOld> openList;
	private List<NodeOld> closeList;

	private void Update()
	{
		PathFinding();
	}

	public void PathFinding()
	{
		sizeX = topRight.x - bottomLeft.x + 1;
		sizeY = topRight.y - bottomLeft.y + 1;
		nodeArray = new NodeOld[sizeX, sizeY];

		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				bool isWall = false;
				foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
				{
					if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) 
					{ 
						isWall = true;
					}
				}

				nodeArray[i, j] = new NodeOld(isWall, i + bottomLeft.x, j + bottomLeft.y);
			}
		}

		startNode = nodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
		targetNode = nodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

		openList = new List<NodeOld>() { startNode };
		closeList = new List<NodeOld>();
		finalNodeList = new List<NodeOld>();

		while (openList.Count > 0)
		{
			// ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
			currentNode = openList[0];
			for (int i = 1; i < openList.Count; i++)
			{
				if (openList[i].F <= currentNode.F && openList[i].h < currentNode.h) currentNode = openList[i];
			}

			openList.Remove(currentNode);
			closeList.Add(currentNode);


			// ������
			if (currentNode == targetNode)
			{
				NodeOld TargetCurNode = targetNode;
				while (TargetCurNode != startNode)
				{
					finalNodeList.Add(TargetCurNode);
					TargetCurNode = TargetCurNode.parentNode;
				}
				finalNodeList.Add(startNode);
				finalNodeList.Reverse();

				return;
			}


			// �֢آע�
			if (allowDiagonal)
			{
				OpenListAdd(currentNode.x + 1, currentNode.y + 1);
				OpenListAdd(currentNode.x - 1, currentNode.y + 1);
				OpenListAdd(currentNode.x - 1, currentNode.y - 1);
				OpenListAdd(currentNode.x + 1, currentNode.y - 1);
			}

			// �� �� �� ��
			OpenListAdd(currentNode.x, currentNode.y + 1);
			OpenListAdd(currentNode.x + 1, currentNode.y);
			OpenListAdd(currentNode.x, currentNode.y - 1);
			OpenListAdd(currentNode.x - 1, currentNode.y);
		}
	}

	void OpenListAdd(int checkX, int checkY)
	{
		// �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
		if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !closeList.Contains(nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
		{
			// �밢�� ����, �� ���̷� ��� �ȵ�
			if (allowDiagonal)
			{ 
				if (nodeArray[currentNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && nodeArray[checkX - bottomLeft.x, currentNode.y - bottomLeft.y].isWall)
				{
					return;
				}
			}

			// �ڳʸ� �������� ���� ������, �̵� �߿� �������� ��ֹ��� ������ �ȵ�
			if (dontCrossCorner) 
			{
				if (nodeArray[currentNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || nodeArray[checkX - bottomLeft.x, currentNode.y - bottomLeft.y].isWall)
				{
					return;
				} 
			}

			// �̿���忡 �ְ�, ������ 10, �밢���� 14���
			NodeOld NeighborNode = nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
			int MoveCost = currentNode.g + (currentNode.x - checkX == 0 || currentNode.y - checkY == 0 ? 10 : 14);

			// �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
			if (MoveCost < NeighborNode.g || !openList.Contains(NeighborNode))
			{
				NeighborNode.g = MoveCost;
				NeighborNode.h = (Mathf.Abs(NeighborNode.x - targetNode.x) + Mathf.Abs(NeighborNode.y - targetNode.y)) * 10;
				NeighborNode.parentNode = currentNode;

				openList.Add(NeighborNode);
			}
		}
	}

	void OnDrawGizmos()
	{
		if (finalNodeList.Count != 0) for (int i = 0; i < finalNodeList.Count - 1; i++)
				Gizmos.DrawLine(new Vector2(finalNodeList[i].x, finalNodeList[i].y), new Vector2(finalNodeList[i + 1].x, finalNodeList[i + 1].y));
	}
}
