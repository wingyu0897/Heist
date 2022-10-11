using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	[SerializeField] private PoolingListSO poolingList;

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
		}

		PoolManager.instance = new PoolManager(transform);
		CreatePool();
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			SceneManager.LoadScene(2);
		}
	}

	private void CreatePool()
	{
		foreach (PoolingSet ps in poolingList.list)
		{
			PoolManager.instance.CreatePool(ps.prefab, ps.count);
		}
	}
}
