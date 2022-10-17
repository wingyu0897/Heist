using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
	None,
	Menu,
	Ready,
	Runnding,
	Ended
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

	public bool readyOnAwake = false;

	[Header("Reference")]
	[SerializeField] private PoolingListSO poolingList;
	[SerializeField] private GameObject player;
	public GameObject Player => player;

	[Header("Properties")]
	public GameState currentGameState = GameState.None;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);

			PoolManager.instance = new PoolManager(transform);

			if (SceneManager.GetActiveScene().buildIndex == 0)
			{
				SceneManager.LoadScene(1);
			}
		}

		if (readyOnAwake)
		{
			ReadyGame();
		}
	}

	private void CreatePool()
	{
		foreach (PoolingSet ps in poolingList.list)
		{
			PoolManager.instance.CreatePool(ps.prefab, ps.count);
		}
	}

	public void StartGame()
	{
		SceneManager.LoadScene(2);
	}

	public void ReadyGame()
	{
		currentGameState = GameState.Ready;
		player = Instantiate(player, null);
		player.name = "Player";
		player?.SetActive(false);
		GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().m_Follow = player.transform;
		PlayerData.Instance.ReadyGame();
		CreatePool();
	}

	public void RunningGame()
	{
		currentGameState = GameState.Runnding;
		player?.SetActive(true);
		transform.Find("PlayerCanvas").gameObject.SetActive(true);
		PlayerData.Instance.RunGame();
		MissionData.Instance?.RunTheGame();
	}

	public void EndGame()
	{
		currentGameState = GameState.Ended;
		GameObject.FindGameObjectWithTag("Entity")?.SetActive(false);
		player.SetActive(false);
		transform.Find("PlayerCanvas").gameObject.SetActive(false);
		MissionData.Instance?.EndTheGame();
	}
}
