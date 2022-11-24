using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageLevel
{
    하,
    중하,
    중,
    중상,
    상
}

[CreateAssetMenu(menuName = "SO/DATA/Stage")]
public class StageSO : ScriptableObject
{
    public int stageNumber;
    public string stageSceneName;
    public string mapName;
    public string address;
    public StageLevel stageLevel;
    public Sprite stageImage;
}
