using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    private static string nextScene;

	[SerializeField] private Transform progressObj;
	[SerializeField] private Slider slider;

    public static void LoadScene(string sceneName)
	{
		nextScene = sceneName;
		SceneManager.LoadScene("LoadingScene");
	}

	private void Start()
	{
		StartCoroutine(LoadingSceneProcess());
	}

	IEnumerator LoadingSceneProcess()
	{
		AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
		op.allowSceneActivation = false;

		float timer = 0;

		while (!op.isDone)
		{
			yield return null;

			progressObj.localEulerAngles += new Vector3(0, 0, 90) * Time.unscaledDeltaTime;
			slider.value = op.progress;

			if (op.progress >= 0.9f)
			{
				timer += Time.unscaledDeltaTime;
				slider.value = Mathf.Lerp(0.9f, 1f, timer);
				if (timer > 1f)
				{
					op.allowSceneActivation = true;
					yield break;
				}
			}
		}
	}
}
