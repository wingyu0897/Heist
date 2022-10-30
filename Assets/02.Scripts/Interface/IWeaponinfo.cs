using UnityEngine;

public interface IWeaponinfo
{
    public GameObject gameObject { get; }
    public string MainInfo { get; }
    public string SubInfo { get; }
    public Texture2D WeaponImage { get; }
}
