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
    public static GameManager instance;

	[Header("Reference")]
	[SerializeField] private PoolingListSO poolingList;
	[SerializeField] private GameObject player;
	public GameObject Player => player;

	[Header("Properties")]
	public GameState currentGameState = GameState.None;
	public bool isDetected = false;
	public bool isLoud = false;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

			PoolManager.instance = new PoolManager(transform);

			if (SceneManager.GetActiveScene().buildIndex == 0)
			{
				StartGame();
			}
		}
	}

	private void Start()
	{
		ReadyGame();
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
		if (SceneManager.GetActiveScene().buildIndex == 2)
		{
			currentGameState = GameState.Ready;
			player = Instantiate(player, null);
			player.name = "Player";
			player?.SetActive(false);
			GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().m_Follow = player.transform;
			PlayerData.Instance.ReadyGame();
			CreatePool();
		}
	}

	public void RunningGame()
	{
		currentGameState = GameState.Runnding;
		player?.SetActive(true);
		transform.Find("PlayerCanvas").gameObject.SetActive(true);
		PlayerData.Instance.StartGame();
	}

	public void EndGame()
	{
		currentGameState = GameState.Ended;
		GameObject.FindGameObjectWithTag("Entity").SetActive(false);
	}
}
