using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayAttack : MonoBehaviour
{
    private Player _player;
    private TextMeshProUGUI _attackText;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _attackText = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        _attackText.text = "ATTACK:\n" + _player.AttackDamage.ToString();
    }
}
