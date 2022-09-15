using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DATA/Gun")]
public class GunSO : ScriptableObject
{
    [Header("Info")]
    public string gunName;

    [Header("Ammo")]
    public int magSize;
    public int maxAmmo;
    public int currentAmmo;
    public float reloadTime;

    [Header("Shoot")]
    public float fireRate;
    [Range(0, 360)]
    public float spreadAngle;
    public float minAimRange;
    [Range(0, 1)]
    public float gunSlerpSpeed;
}
