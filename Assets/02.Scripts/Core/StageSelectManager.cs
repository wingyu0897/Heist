using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> buttons = new List<GameObject>();
	[SerializeField] 
	private GameObject stageInfo;
	[SerializeField] 
	private Image mapImage;
	[SerializeField] 
	private TextMeshProUGUI mapNameText;
	[SerializeField]
	private TextMeshProUGUI mapAddressText;
	[SerializeField]
	private TextMeshProUGUI stageLevelText;

	private StageSO currentStage;

	private void Start()
	{
		for (int i = 0; i <= PlayerData.Instance.clearStageCount; i++)
		{
			if (buttons.Count > i)
			{
				buttons[i]?.SetActive(true);
			}
		}
		stageInfo?.SetActive(false);
		currentStage = null;
	}

	public void SelectStage(StageSO data)
	{
		if (data.stageNumber > PlayerData.Instance.clearStageCount)
			return;

		stageInfo?.SetActive(true);
		currentStage = data;
		mapImage.sprite = data.stageImage;
		mapNameText.text = data.stageSceneName;
		mapAddressText.text = $"위치  {data.address}";
		stageLevelText.text = $"난이도  {data.stageLevel}";
	}

	public void StartGame()
	{
		GameManager.Instance?.LoadGame(currentStage.stageSceneName);
	}
}
