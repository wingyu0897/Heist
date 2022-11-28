using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthOnly : MonoBehaviour
{
    public List<AIBrain> entities;

	private void Update()
	{
		foreach (AIBrain ab in entities)
		{
			if (ab.IsDead)
			{
				StageManager.Instance.Louded();
			}
		}
	}
}
