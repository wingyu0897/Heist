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

	[Header("--Reference--")]
	[SerializeField] private PoolingListSO poolingList;
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject player;
	public GameObject Player => player;

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

			if (PoolManager.Instance == null)
			{
				PoolManager.Instance = new PoolManager(transform);
			}

			if (readyOnAwake) ReadyGame();
		}
	}

	private void CreatePool()
	{
		foreach (PoolingSet ps in poolingList.list)
		{
			PoolManager.Instance.CreatePool(ps.prefab, ps.count);
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

		player = Instantiate(playerPrefab, null);
		player.name = "Player";
		player.SetActive(false);
		transform.Find("GameReadyCanvas").gameObject.SetActive(true);
		GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().m_Follow = player.transform;
		PlayerData.Instance.ReadyGame();

		CreatePool();
		OnReadyGame?.Invoke();
	}

	public void RunningGame()
	{
		currentGameState = GameState.Runnding;

		player.SetActive(true);
		transform.Find("PlayerCanvas").gameObject.SetActive(true);
		PlayerData.Instance.RunGame();
		MissionData.Instance?.RunTheGame();

		OnRunningGame?.Invoke();
	}

	public void EndGame(bool isSuccess)
	{
		currentGameState = GameState.End;

		player.SetActive(false);
		player = null;
		transform.Find("PlayerCanvas").gameObject.SetActive(false);
		PoolManager.Instance.DestroyPools();
		MissionData.Instance?.EndTheGame(isSuccess);

		OnEndGame?.Invoke();
	}
}
