using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Save"))
		{
			(target as DataManager).SaveData();
		}
		if (GUILayout.Button("Load"))
		{
			(target as DataManager).LoadData();
		}
	}
}
