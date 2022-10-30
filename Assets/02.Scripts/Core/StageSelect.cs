using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
	[SerializeField] private GameObject stageInfo;
	[SerializeField] private Image infoImage;
	[SerializeField] private TextMeshProUGUI infoText;

	private StageSO currentStage;

	private void Start()
	{
		stageInfo?.SetActive(false);
		currentStage = null;
	}

	public void SelectStage(StageSO data)
	{
		stageInfo?.SetActive(true);
		currentStage = data;
		infoImage.sprite = data.stageImage;
		infoText.text = data.stageName;
	}

	public void StartGame()
	{
		GameManager.Instance?.LoadGame(currentStage.stageName);
	}
}
