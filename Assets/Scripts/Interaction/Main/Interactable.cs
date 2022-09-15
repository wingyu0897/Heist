using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public GameObject Objectgame { get; }
    public float InteractionTime { get; }
    public bool CanInteractive { get; set; }

    public void Init();
    public void Action();
}
