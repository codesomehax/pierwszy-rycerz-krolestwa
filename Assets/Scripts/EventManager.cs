using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action OnReputationChange;

    public static void StartEventOnReputationChange()
    {
        OnReputationChange?.Invoke();
    }
}
