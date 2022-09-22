using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GunSO weaponData;
    public abstract void Attack();
    public abstract bool TryAttack();
}
