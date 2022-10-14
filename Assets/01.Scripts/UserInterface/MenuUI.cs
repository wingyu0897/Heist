using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
	public void QuitGame()
	{
		Application.Quit();
	}

	public void StartGame()
	{
		GameManager.instance.StartGame();
	}
}
