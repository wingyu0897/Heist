using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
	public int x, y;
	public int g, h;
	public bool isWall;
	public Node parentNode;

	public Node(bool isWall, int x, int y)
	{
		this.isWall = isWall;
		this.x = x;
		this.y = y;
	}

	public int F { get => g + h; }
}

public class AIPathFinding : MonoBehaviour
{
    [SerializeField] private Vector2Int startPos;
    [SerializeField] private Vector2Int targetPos;
	[SerializeField] private List<Node> finalNodeList;

	private Node startNode;
	private Node targetNode;
	private Node currentNode;

	private List<Node> openList;
	private List<Node> closedList;

	private AIBrain brain;
	private NodeScan node;

	private Vector2Int bottomLeft;
	private Vector2Int topRight;

	private void Start()
	{
		brain = GetComponent<AIBrain>();
		node = GameObject.Find("Map").GetComponent<NodeScan>();
		bottomLeft = node.bottomLeft;
		topRight = node.topRight;
	}

	public void MoveToTarget(Vector2Int targetPos, Vector2 point)
	{
		this.targetPos = targetPos;
		startPos = new Vector2Int(Mathf.RoundToInt(brain.BasePosition.position.x), Mathf.RoundToInt(brain.BasePosition.position.y));

		if (node.nodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y] != targetNode ||
			node.nodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y] != startNode)
		{
			if (!node.nodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y].isWall)
			{
				PathFinding();
			}
		}

		if (finalNodeList.Count > 1)
		{
			Vector2 direction = new Vector2(finalNodeList[1].x, finalNodeList[1].y) - (Vector2)brain.BasePosition.position;
			brain.MoveByDirection(direction, point == Vector2.zero ? new Vector2(finalNodeList[1].x, finalNodeList[1].y) : point);
			Debug.DrawRay(new Vector2(finalNodeList[finalNodeList.Count - 1].x, finalNodeList[finalNodeList.Count - 1].y), targetPos - new Vector2(finalNodeList[finalNodeList.Count - 1].x, finalNodeList[finalNodeList.Count - 1].y), Color.yellow);
		}
		else
		{
			brain.MoveByDirection(Vector2.zero, Vector2.zero);
		}
	}

	private void PathFinding() //길 찾기 함수
	{
		if (targetPos.x - bottomLeft.x < node.sizeX && targetPos.x - bottomLeft.x >= 0 && targetPos.y - bottomLeft.y < node.sizeY && targetPos.y - bottomLeft.y >= 0)
		{
			targetNode = node.nodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];
		}
		if (startPos.x - bottomLeft.x < node.sizeX && startPos.x - bottomLeft.x >= 0 && startPos.y - bottomLeft.y < node.sizeY && startPos.y - bottomLeft.y >= 0)
		{
			startNode = node.nodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
			startNode.h = (Mathf.Abs(startNode.x - targetNode.x) + Mathf.Abs(startNode.y - targetNode.y)) * 10;
		}

		openList = new List<Node>() { startNode };
		closedList = new List<Node>();
		finalNodeList = new List<Node>();

		while (openList.Count > 0)
		{
			currentNode = openList[0];
			for (int i = 0; i < openList.Count; i++)
			{
				if (openList[i].F <= currentNode.F && openList[i].h <= currentNode.h)
				{
					currentNode = openList[i];
				}
			}

			openList.Remove(currentNode);
			closedList.Add(currentNode);
			
			//마지막 노드
			if (currentNode == targetNode)
			{
				Node targetCurNode = currentNode;
				while (targetCurNode != startNode)
				{
					finalNodeList.Add(targetCurNode);
					targetCurNode = targetCurNode.parentNode;
				}
				finalNodeList.Add(startNode);
				finalNodeList.Reverse();

				return;
			}

			OpenListAdd(currentNode.x + 1, currentNode.y + 1);
			OpenListAdd(currentNode.x - 1, currentNode.y + 1);
			OpenListAdd(currentNode.x - 1, currentNode.y - 1);
			OpenListAdd(currentNode.x + 1, currentNode.y - 1);

			OpenListAdd(currentNode.x, currentNode.y + 1);
			OpenListAdd(currentNode.x + 1, currentNode.y);
			OpenListAdd(currentNode.x, currentNode.y - 1);
			OpenListAdd(currentNode.x - 1, currentNode.y);
		}

		if (finalNodeList.Count == 0)
		{
			currentNode = closedList[0];
			
			for (int i = targetNode.x - bottomLeft.x - 3; i < targetNode.x - bottomLeft.x + 3; i++)
			{
				for (int j = targetNode.y - bottomLeft.y - 3; j < targetNode.y - bottomLeft.y + 3; j++)
				{
					if (node.nodeArray[i, j]?.parentNode != null)
					{
						RaycastHit2D hit = Physics2D.Raycast(new Vector2(node.nodeArray[i, j].x, node.nodeArray[i, j].y), targetPos - new Vector2(node.nodeArray[i, j].x, node.nodeArray[i, j].y), 1f, node.obstacleLayer);
						if (node.nodeArray[i, j].h <= currentNode.h && hit.collider == null)
						{
							currentNode = node.nodeArray[i, j];						
						}
					}
				}
			}

			Node targetCurNode = currentNode;
			while (targetCurNode != startNode)
			{
				finalNodeList.Add(targetCurNode);
				targetCurNode = targetCurNode.parentNode;
			}
			finalNodeList.Add(startNode);
			finalNodeList.Reverse();
		}
	}

	private void OpenListAdd(int checkX, int checkY)
	{
		//범위를 벗어나지 않고, 장애물이 아니면서, 닫힌 리스트가 아닐 때
		if (checkX >= bottomLeft.x && checkX <= topRight.x && checkY >= bottomLeft.y && 
			checkY <= topRight.y && !node.nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && 
			!closedList.Contains(node.nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
		{
			if (node.nodeArray[currentNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || node.nodeArray[checkX - bottomLeft.x, currentNode.y - bottomLeft.y].isWall)
			{
				return;
			}

			Node neighborNode = node.nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
			int moveCost = currentNode.g + (currentNode.x - checkX == 0 || currentNode.y - checkY == 0 ? 10 : 14);

			if (moveCost < currentNode.g || !openList.Contains(neighborNode))
			{
				neighborNode.g = moveCost;
				neighborNode.h = Mathf.RoundToInt(Vector2.Distance(new Vector2(targetNode.x, targetNode.y), new Vector2(neighborNode.x, neighborNode.y)) * 10);
				neighborNode.parentNode = currentNode;
				openList.Add(neighborNode);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (finalNodeList.Count != 0)
		{
			for (int i = 0; i < finalNodeList.Count - 1; i++)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(new Vector2(finalNodeList[i].x, finalNodeList[i].y), new Vector2(finalNodeList[i + 1].x, finalNodeList[i + 1].y));
			}
		}
	}
}
