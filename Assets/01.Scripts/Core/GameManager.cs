using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameState
{
	None,
	Menu,
	Ready,
	Runnding,
	End
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
	[Header("--Flags--")]
	[SerializeField] private bool readyOnAwake;

	[Header("--Properties--")]
	public GameState currentGameState = GameState.None;

	[Header("--Event--")]
	public UnityEvent OnStartGame;
	public UnityEvent OnReadyGame;
	public UnityEvent OnRunningGame;
	public UnityEvent OnEndGame;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this);
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);

			if (SceneManager.GetActiveScene().buildIndex == 0)
			{
				SceneManager.LoadScene(1);
			}

			if (readyOnAwake) ReadyGame();
		}
	}

	public void StartGame(string scene)
	{
		SceneManager.LoadScene(scene);
		OnStartGame?.Invoke();
	}

	public void ReadyGame()
	{
		currentGameState = GameState.Ready;

		transform.Find("GameReadyCanvas").gameObject.SetActive(true);
		PlayerData.Instance.ReadyGame();

		OnReadyGame?.Invoke();
	}

	public void RunningGame()
	{
		currentGameState = GameState.Runnding;

		transform.Find("PlayerCanvas").gameObject.SetActive(true);
		PlayerData.Instance.RunGame();
		MissionData.Instance?.RunTheGame();

		OnRunningGame?.Invoke();
	}

	public void EndGame(bool isSuccess)
	{
		currentGameState = GameState.End;

		transform.Find("PlayerCanvas").gameObject.SetActive(false);
		MissionData.Instance?.EndTheGame(isSuccess);

		OnEndGame?.Invoke();
	}
}
