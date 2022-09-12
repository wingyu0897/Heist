using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DATA/Movement")]
public class MovememtSO : ScriptableObject
{
    [Header("Data")]
    public float maxSpeed = 0;
    public float acceleration = 0;
    public float deAcceleration = 0;
    public float jumpPower = 0;
    public float jumpRate = 0;
    [HideInInspector]
    public float currentVelocity = 0;
    [HideInInspector]
    public float currentDirection = 0;
}
