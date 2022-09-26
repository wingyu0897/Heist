using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DATA/Melee")]
public class MeleeSO : ScriptableObject
{
	[Header("Info")]
	public string meleeName;
	public WeaponType weaponType;

	[Header("Attack")]
	public int damage;
	public float minAimRange;
	public float range;
	public float attackDelay;
	[HideInInspector]
	public Transform attackPoint;

	[Header("AudioClip")]
	public AudioClip attackClip;
}
