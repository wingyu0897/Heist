using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	[SerializeField] private PoolingListSO poolingList;

	private void Awake()
	{
		if (instance != null || instance != this)
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
	}

	private void CreatePool()
	{
		foreach (PoolingSet ps in poolingList.list)
		{
			PoolManager.instance.CreatePool(ps.prefab, ps.count);
		}
	}
}
