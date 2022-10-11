using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponinfo
{
    public string MainInfo { get; }
    public string SubInfo { get; }
    public Texture2D WeaponImage { get; }
}
