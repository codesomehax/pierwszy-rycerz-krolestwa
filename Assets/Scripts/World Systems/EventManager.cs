using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<NPC> OnNpcDeath;
    public static event Action OnGoalCompleted;
    public static event Action<Quest> OnQuestCompleted;

    public static void StartEventOnNpcDeath(NPC npc)
    {
        OnNpcDeath?.Invoke(npc);
    }

    public static void StartEventOnGoalCompleted()
    {
        OnGoalCompleted?.Invoke();
    }

    public static void StartEventOnQuestCompleted(Quest quest)
    {
        OnQuestCompleted?.Invoke(quest);
    }
}
