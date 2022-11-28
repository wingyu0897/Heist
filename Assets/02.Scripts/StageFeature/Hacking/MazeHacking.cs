using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Maze
{
	public GameObject mapObj;
	public Image targetSprite;
}

public class MazeHacking : Hacking
{
	[Header("--Reference--")]
	[SerializeField] private Movement playerMovement;
	[SerializeField] private List<Maze> mazes = new List<Maze>();

	[Header("--Setting--")]
	[SerializeField] private Vector2 basePosition;
	[SerializeField] private string targetLayer;
	[SerializeField] private string obstacleLayer;
	[Tooltip("해킹이 성공하거나 실패할 때 해킹 패널이 닫히는 시간")]
	//[SerializeField] private float setOffDelay = 0f;
	[SerializeField] private float speed;
	[SerializeField] private Vector2 direction;

	[Header("--Properties--")]
	public int currentLevel = 1;

	private bool isActive = false;
	private bool canMove = true;
	private bool isClear = false;

	[Header("--Events--")]
	public UnityEvent OnSuccessHacking;
	public UnityEvent OnFailureHacking;

	private void Start()
	{
		Initialize();
	}

	private void Update()
	{
		if (isActive)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				direction = direction.x == 0 ? new Vector2(-direction.y, 0) : new Vector2(0, direction.x);
			}
		}
	}

	private void FixedUpdate()
	{
		if (canMove && isActive)
		{
			playerMovement.Move(direction);
		}
		else
		{
			playerMovement.Move(Vector2.zero);
		}
	}

	public override void Initialize() //초기화
	{
		StopAllCoroutines();
		isClear = false;
		isActive = false;
		canMove = true;
		currentLevel = 1;
		direction = Vector2.right;
		playerMovement.transform.localPosition = basePosition;
	}

	public override void StartHacking()
	{
		isActive = true;
		canMove = true;
	}

	public void FailureHacking()
	{
		OnFailureHacking?.Invoke();
		StartCoroutine(OnClear());
	}

	public void SuccessHacking()
	{
		Debug.Log("Success!");
	}

	public void MoveToNextLevel()
	{
		mazes[currentLevel-1].targetSprite.color = Color.green;

		if (currentLevel < mazes.Count)
		{
			isActive = true;
			StartCoroutine(ActiveNextLevel());
		}
		else
		{
			isClear = true;
			OnSuccessHacking?.Invoke();
			StartCoroutine(OnClear());
		}
	}

	public void TriggerEnter(Collider2D collision)
	{
		if (isActive)
		{
			canMove = false;
			isActive = false;
			if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayer))
			{
				MoveToNextLevel();
			}
			else
			{
				FailureHacking();
			}
		}
	}

	private IEnumerator OnClear()
	{
		yield return new WaitForSeconds(1f);

		gameObject.SetActive(false);
	}

	private IEnumerator ActiveNextLevel()
	{
		yield return new WaitForSeconds(1f);

		mazes[currentLevel - 1].mapObj.SetActive(false);
		mazes[currentLevel].mapObj.SetActive(true);
		currentLevel++;

		playerMovement.transform.localPosition = basePosition;
		direction = Vector2.right;
		canMove = true;
	}
}
