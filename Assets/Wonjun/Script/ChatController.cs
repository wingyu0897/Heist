using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChatController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _chatText;
    [SerializeField] private TextMeshProUGUI _charactorText;
    [SerializeField] private float _chatCoolTime = 5f;

    [SerializeField] private string[] _texts;
    [SerializeField] private string[] _Charactortexts;
    private string _nowText;

    private void Start()
    {
        StartCoroutine(SetText());
    }

    private IEnumerator SetText()
    {
        for(int i = 0; i < _texts.Length; i++)
        {
            _chatText.text = _texts[i];
            _charactorText.text = _Charactortexts[i];
            yield return new WaitForSeconds(_chatCoolTime);
        }
        _chatText.text = null;
        _Charactortexts = null;
    }
}
