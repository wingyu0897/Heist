using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public List<GetPaint> paints;

	private void Start()
	{
		int ranNum = Random.Range(0, paints.Count);
		paints[ranNum].isReal = true;
	}
}
