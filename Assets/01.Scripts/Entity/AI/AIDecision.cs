using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecision : MonoBehaviour
{
    protected AIBrain brain;

    protected virtual void Awake()
	{
        brain = transform.parent.parent.parent.GetComponent<AIBrain>();
	}

    public abstract bool Result(); //���� ���� ���� �Լ�
}
