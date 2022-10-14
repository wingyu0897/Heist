using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class obtion : MonoBehaviour
{
    public GameObject penel;
    public bool menu_On_Off;

    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu_On_Off == true)
            {
                menu_On_Off = false;
            }
            if (menu_On_Off == false)
            {
                menu_On_Off = true;
            }
        }

        if (menu_On_Off)
        {
            penel.SetActive(false);
        }
        if (!menu_On_Off)
        {
            penel.SetActive(true);
        }
    }
}
