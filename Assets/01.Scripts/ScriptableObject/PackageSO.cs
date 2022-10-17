using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DATA/ENTITY/Package")]
public class PackageSO : ScriptableObject
{
    [Header("Reference")]
    public int price;
    [Range(1, 100)]
    public int weight;

    public float interactiveTime;

    public Sprite defaultSprite;
    public Sprite packedSprite;
}