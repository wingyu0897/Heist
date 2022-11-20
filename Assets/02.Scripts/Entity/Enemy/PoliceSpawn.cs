using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSpawn : MonoBehaviour
{
	[SerializeField]
	private Vector3 spawnPosition;
	[SerializeField]
	private Transform entityParent;

    [SerializeField]
    private GameObject pistolPolicePrefab;

    public void SpawnPolice()
	{
        for (int i = 0; i < 3; i++)
		{
			Enemy enemy = PoolManager.Instance.Pop(pistolPolicePrefab.name) as Enemy;
			enemy.transform.position = spawnPosition;
			enemy.transform.SetParent(entityParent);
		}
	}
}
