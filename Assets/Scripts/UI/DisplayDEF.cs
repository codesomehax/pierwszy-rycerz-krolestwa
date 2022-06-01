using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayDEF : MonoBehaviour
{
    private Player _player;
    private TextMeshProUGUI _defText;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _defText = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        _defText.text = "DEF:\n" + _player.Defense.ToString();
    }
}
