using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonDefault : MonoBehaviour
{
    public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void LoadScene(string sceneName)
	{
		LoadingSceneController.LoadScene(sceneName);
	}
}
