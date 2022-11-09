using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private GameObject[] map;
    public void FirstMap()
    {
        map[1].SetActive(false);
    }
    public void SecondMap()
    {
        map[2].SetActive(false);
        map[3].SetActive(false);
    }
    public void thirdMap()
    {
        map[6].SetActive(false);
    }
    public void fourMap()
    {
        map[4].SetActive(false);
        map[5].SetActive(false);

    }
    
    


}
