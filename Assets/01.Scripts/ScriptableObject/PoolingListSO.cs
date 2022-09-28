using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolingSet
{
	public Poolable prefab;
	public int count;
}

[CreateAssetMenu(menuName = "SO/DATA/PoolingList")]
public class PoolingListSO : ScriptableObject
{
	public List<PoolingSet> list;
}
