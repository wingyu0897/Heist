using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MazeHacking : Hacking
{
	[Header("--Reference--")]
	[SerializeField] private Movement playerMovement;
	[SerializeField] private List<GameObject> maps = new List<GameObject>();

	[Header("--Setting--")]
	[SerializeField] private Vector2 basePosition;
	[SerializeField] private string targetLayer;
	[SerializeField] private string obstacleLayer;
	[Tooltip("해킹이 성공하거나 실패할 때 해킹 패널이 닫히는 시간")]
	[SerializeField] private float setOffDelay = 0f;
	[SerializeField] private float speed;
	[SerializeField] private Vector2 direction;

	[Header("--Properties--")]
	public int currentLevel = 1;

	private bool isActive = false;
	private bool canMove = true;
	private bool isClear = false;

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
		if (isActive)
		{
			if (canMove)
			{
				playerMovement.Move(direction);
			}
			else
			{
				playerMovement.Move(Vector2.zero);
			}
		}
	}

	public override void Initialize() //초기화
	{
		playerMovement.transform.localPosition = basePosition;
		isClear = false;
		isActive = false;
		canMove = true;
		direction = Vector2.right;
		currentLevel = 1;
	}

	public override void StartHacking()
	{
		isActive = true;
	}

	public void FailureHacking()
	{
		OnFailureHacking?.Invoke();
	}

	public void SuccessHacking()
	{
		Debug.Log("Success!");
	}

	public void MoveToNextLevel()
	{
		if (currentLevel < maps.Count)
		{
			maps[currentLevel-1].SetActive(false);
			maps[currentLevel].SetActive(true);
			currentLevel++;

			playerMovement.transform.localPosition = basePosition;
			direction = Vector2.right;
		}
		else
		{
			canMove = false;
			isClear = true;
			OnSuccessHacking?.Invoke();
		}
	}

	public void TriggerEnter(Collider2D collision)
	{
		if (isActive)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayer))
			{
				MoveToNextLevel();
			}
			else
			{
				canMove = false;
				FailureHacking();
			}
		}
	}
}
