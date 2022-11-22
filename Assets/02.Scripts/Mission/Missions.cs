using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Missions : MonoBehaviour
{
    public abstract string Text { get; }

    public abstract bool Condition();
}
