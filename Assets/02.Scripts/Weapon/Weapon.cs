using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void StopAttack();

    public abstract void StartAttack();

    public abstract bool TryAttack();

    public abstract void Reload();

    public abstract void Aiming(Vector2 pointerPos);

    public abstract void Init();

    public abstract GameObject GetPrefab();
}
