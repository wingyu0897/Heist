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
	public bool isOption;

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

	public void LoadGame(string scene)
	{
		LoadingSceneController.LoadScene(scene);

		OnStartGame?.Invoke();
	}

	public void ReadyGame()
	{
		currentGameState = GameState.Ready;

		OnReadyGame?.Invoke();
	}

	public void StartGame()
	{
		currentGameState = GameState.Runnding;

		OnRunningGame?.Invoke();
	}

	public void EndGame()
	{
		currentGameState = GameState.End;

		OnEndGame?.Invoke();
	}
}
