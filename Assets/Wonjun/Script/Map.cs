using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField] private Image[] map;
    [SerializeField] private SpriteRenderer sr;
    private void Start()
    {
        for(int i = 0; i < map.Length; i++)
        {
            

        sr = map[i].GetComponent<SpriteRenderer>();
            map[i].color = Color.gray;
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("gkgk");
            map[0].color = Color.cyan;
            map[1].color = Color.cyan;
            map[2].color = Color.cyan;
            map[3].color = Color.cyan;
            map[7].color = Color.cyan;

        }
    }





}
