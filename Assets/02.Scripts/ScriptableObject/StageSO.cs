using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageLevel
{
    ��,
    ����,
    ��,
    �߻�,
    ��
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
