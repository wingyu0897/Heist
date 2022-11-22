using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MissionManager : MonoBehaviour
{
    [Header("--Reference--")]
    [SerializeField]
    private Image checkBox;
    [SerializeField]
    private Sprite checkBoxNull;
    [SerializeField]
    private Sprite checkBoxCheck;
    [SerializeField]
    private TextMeshProUGUI missionText;

    [Header("--Mission--")]
    [SerializeField]
    private List<Missions> missions = new List<Missions>();
    private List<Missions> closeMissions = new List<Missions>();
    private Missions currentMission;

    private bool canTransition = true;

	private void Start()
	{
        if (missions.Count > 0)
		{
    		currentMission = missions[0];
            missionText.text = currentMission.Text;
		}
	}

	private void Update()
	{
		CheckMission();
        if (StageManager.Instance.isEnd || currentMission == null)
		{
            gameObject.SetActive(false);
		}
	}

    private void CheckMission()
	{
        if (currentMission)
		{
            if (currentMission.Condition() && canTransition)
		    {
                closeMissions.Add(currentMission);
                missions.Remove(currentMission);
                canTransition = false;
                OnClear();
		    }
		}
	}

    private void OnClear()
	{
        checkBox.sprite = checkBoxCheck;

        Sequence seq = DOTween.Sequence();
        seq.Append(missionText.DOColor(Color.yellow, 2f));
        seq.AppendCallback(() => currentMission = missions.Count > 0 ? missions[0] : null);
        seq.AppendCallback(() => missionText.text = currentMission?.Text);
        seq.AppendCallback(() => checkBox.sprite = checkBoxNull);
        seq.AppendCallback(() => 
            { 
                if (StageManager.Instance.isEnd || currentMission == null) gameObject.SetActive(false); 
            });
    }
}
