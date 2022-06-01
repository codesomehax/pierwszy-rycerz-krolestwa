using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayGold : MonoBehaviour
{
    private Player _player;
    private TextMeshProUGUI _goldText;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _goldText = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        _goldText.text = "GOLD:\n" + _player.GetGold().ToString();
    }
}

