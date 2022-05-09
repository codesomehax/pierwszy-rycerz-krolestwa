using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal
{
    public int EnemyID;

    private void TryUpdateProgress(NPC enemy)
    {
        if (enemy.ID == EnemyID)
        {
            _currentProgress++;
            EvaluateIsCompleted();
        }
    }

    // only called when the quest containing this goal gets activated too
    public override void ActivateGoal()
    {
        EventManager.OnNpcDeath += TryUpdateProgress;
    }
}
