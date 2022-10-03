using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public static PoolManager instance;

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
			Debug.LogError($"{obj.gameObject.name}�� Ǯ�� �������� �ʽ��ϴ�."); 
		}
	}

	public Poolable Pop(string objName)
	{
		if (pools.ContainsKey(objName) == false)
		{
			Debug.LogError($"Pop Error: {objName}�̶�� Ǯ�� �����ϴ�.");
			return null;
		}

		Poolable item = pools[objName].Pop();
		item.Initialize();
		return item;
	}
}
