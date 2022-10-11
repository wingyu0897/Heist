using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITransition : MonoBehaviour
{
    public List<AIDecision> decisions = new List<AIDecision>();

    public AIState postiveState;
    public AIState negativeState;
}
