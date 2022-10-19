using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class obtion : MonoBehaviour
{
    public GameObject panel;
    public bool optionBool;

    private void Awake()
    {
        optionBool = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionBool)
            {
                optionBool = false;
                panel.SetActive(false);
            }
            else
            {
                optionBool  =true;
                panel.SetActive(true);
            }
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
