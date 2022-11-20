using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DATA/ENTITY/Movement")]
public class MovememtSO : ScriptableObject
{
    [Header("Data")]
    [Range(0, 0.9f)]
    public float speedRandomness = 0;
    public float maxWalkSpeed = 0;
    public float maxRunSpeed = 0;
    public float acceleration = 0;
    public float deAcceleration = 0;
}
