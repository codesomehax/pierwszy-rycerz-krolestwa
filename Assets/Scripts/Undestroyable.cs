using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undestroyable : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
