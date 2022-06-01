using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DBR : MonoBehaviour
{
    private Player _player;
    private TextMeshProUGUI _dbrText;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _dbrText = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        _dbrText.text = "BAD REP:\n" + _player.GetReputation(Alliance.Evil).ToString();
    }
}
