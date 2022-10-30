using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DATA/WEAPON/Bullet")]
public class BulletSO : ScriptableObject
{
	[Header("Reference")]
	public GameObject prefab;

	[Header("Deal")]
	public int damage;
	public float speed;
	public float lifeTime;
	public LayerMask targetLayer;
}
