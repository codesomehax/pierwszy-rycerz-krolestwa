using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayNPCHealth : MonoBehaviour
{
    private Camera _player;
    private NPC _npc;
    public GameObject healthbarUI;
    public Slider slider;
    // Start is called before the first frame update
    private void Awake()
    {
        _npc = GetComponent<NPC>();
        _player = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        Quaternion npcAtPlayer = Quaternion.LookRotation(_player.transform.position - slider.transform.position);

        slider.transform.rotation = npcAtPlayer;

        slider.value = CalcualteHealth();
        if(slider.value <= 0f)
        {
            Object.Destroy(healthbarUI);
        }
    }

    float CalcualteHealth()
    {
        float hp = _npc.GetCurrentHP() / _npc.MaxHP;
        if (hp > 1f) return 1f;
        else if (hp < 0f) return 0f;
        else return hp;
    }
}
