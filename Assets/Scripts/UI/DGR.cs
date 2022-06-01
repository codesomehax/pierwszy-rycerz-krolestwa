using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DGR : MonoBehaviour
{
    private Player _player;
    private TextMeshProUGUI _dgrText;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _dgrText = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        _dgrText.text = "GOOD REP:\n" + _player.GetReputation(Alliance.Good).ToString();
    }
}
