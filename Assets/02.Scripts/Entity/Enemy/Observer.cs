using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public AIBrain brain;

	private void Awake()
	{
		brain = GetComponent<AIBrain>();
	}
}
