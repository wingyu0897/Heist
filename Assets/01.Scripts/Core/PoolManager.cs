using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public static PoolManager Instance;
    private Dictionary<string, Pool<Poolable>> pools = new Dictionary<string, Pool<Poolable>>();

    private Transform parent;
    public PoolManager(Transform parent)
    {
        this.parent = parent;
    }

	public void CreatePool(Poolable prefab, int count = 10)
	{
		Pool<Poolable> pool = new Pool<Poolable>(prefab, parent, count);
		pools.Add(prefab.gameObject.name, pool);
	}

	public void Push(Poolable obj)
	{
		if (pools.ContainsKey(obj.gameObject.name)) 
		{ 
			pools[obj.gameObject.name].Push(obj); 
		}
		else 
		{ 
			Debug.Log($"ERROR:PoolManager: {obj.gameObject.name}은 풀에 존재하지 않습니다.");
			obj.gameObject.SetActive(false);
		}
	}

	public Poolable Pop(string objName)
	{
		if (pools.ContainsKey(objName) == false)
		{
			Debug.Log($"Error:PoolManager: {objName}이라는 풀은 없습니다.");
			return null;
		}

		Poolable item = pools[objName].Pop();
		item.Initialize();
		return item;
	}

	public void DestroyPools()
	{
		foreach (Pool<Poolable> item in pools.Values)
		{
			item.DestroyPool();
		}
		pools.Clear();
	}
}
