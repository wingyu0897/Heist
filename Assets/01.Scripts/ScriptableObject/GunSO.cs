using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none,
    melee,
    secondary,
    primary,
    special
}

[CreateAssetMenu(menuName = "SO/DATA/WEAPON/Gun")]
public class GunSO : ScriptableObject
{
    [Header("Info")]
    public string gunName;
    public WeaponType weaponType;
    public GameObject prefab;

    [Header("Ammo")]
    public int magSize;
    public int maxAmmo;
    public int currentAmmo;
    public float reloadTime;
    public BulletSO bulletData;

    [Header("Shoot")]
    public bool isAuto;
    [HideInInspector]
    public Transform muzzle;
    public float fireRate;
    [Range(0, 360)]
    public float spreadAngle;
    public float minAimRange;
    [Range(0, 1)]
    public float gunSlerpSpeed;

    [Header("AudioClip")]
    public AudioClip shootClip;
    public AudioClip noAmmoClip;
    public AudioClip reloadClip;
}
