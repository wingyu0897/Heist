using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public GameObject optionPanel;
    public bool isActive;
    public bool isPause;

    private void Awake()
    {
        isActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
            Active(isActive);
        }

        Time.timeScale = isPause ? 0 : 1f;
    }

    public void ChangeSceneWithLoad(string sceneName)
	{
        Active(false);
        StageManager.Instance.EndTheGame(false);
        LoadingSceneController.LoadScene(sceneName);
	}

    public void MainMenu()
    {
        Active(false);
        SceneManager.LoadScene("Menu");
    }

    public void Active(bool isActive)
	{
        isPause = isActive;
        optionPanel.SetActive(isActive);
        this.isActive = isActive;
        GameManager.Instance.isOption = isActive;
	}
}
