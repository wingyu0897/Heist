using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DATA/Movement")]
public class MovememtSO : ScriptableObject
{
    [Header("Data")]
    public float maxWalkSpeed = 0;
    public float maxRunSpeed = 0;
    public float acceleration = 0;
    public float deAcceleration = 0;
}
