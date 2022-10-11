using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DATA/WEAPON/Melee")]
public class MeleeSO : ScriptableObject
{
	[Header("Info")]
	public string meleeName;
	public WeaponType weaponType;
	public GameObject prefab;
	public Texture2D weaponSprite;

	[Header("Attack")]
	public int damage;
	public float minAimRange;
	public float range;
	public float attackDelay;
	[HideInInspector]
	public Transform attackPoint;
	[Range(0, 1)]
	public float weaponSlerpSpeed;

	[Header("AudioClip")]
	public AudioClip attackClip;
}
