using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntitySpawn
{
	public GameObject prefab;
	public int spawnCount;
}

[System.Serializable]
public class WaveEntities
{
	public List<EntitySpawn> spawns = new List<EntitySpawn>();
	[Tooltip("경보가 울린 후 웨이브가 시작되는 시간 ex)sapwnTime = 40 == 40초 후 생성")]
	public float spawnTime;
	public bool isSpawn = false;
}

public class PoliceSpawn : MonoBehaviour
{
	[SerializeField]
	private List<WaveEntities> waves = new List<WaveEntities>();

	[SerializeField]
	private Transform entityParent;

	private float time = 0;
	private bool active = false;

	private void Start()
	{
		time = 0;
		active = false;
	}

	public void StartSpawn()
	{
		time = 0;
		active = true;
	}

	private void Update()
	{
		if (active)
		{
			time += Time.deltaTime;

			foreach (WaveEntities we in waves)
			{
				if (we.spawnTime <= time && we.isSpawn == false)
				{
					we.isSpawn = true;
					foreach (EntitySpawn es in we.spawns)
					{
						for (int i = 0; i < es.spawnCount; i++)
						{
							Enemy enemy = PoolManager.Instance.Pop(es.prefab.name) as Enemy;
							enemy.transform.position = transform.position;
							enemy.transform.SetParent(entityParent);
						}
					}
				}
			}
		}
	}
}
