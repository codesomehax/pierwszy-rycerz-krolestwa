using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayHealth : MonoBehaviour
{
    private Player _player;
    private TextMeshProUGUI _healthText;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _healthText = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        _healthText.text ="HEALTH:\n" + _player.GetCurrentHP().ToString();
    }
}
