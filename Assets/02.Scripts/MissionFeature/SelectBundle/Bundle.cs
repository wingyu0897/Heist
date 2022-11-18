using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bundle : MonoBehaviour
{
    public abstract void OnSelection();
    public abstract void OnEquip();
    public abstract void OnUnEquip();
    public abstract bool CanEquip();

    public abstract Sprite BundleImage { get; }
    public abstract bool IsEquiped { get; }
    public abstract bool CanUnEquip { get; }
}
