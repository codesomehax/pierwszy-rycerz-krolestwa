using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillGoal : Goal
{
    public NPC Enemy;

    private void TryUpdateProgress(NPC enemy)
    {
        if (enemy.ID == Enemy.ID)
        {
            _currentProgress++;
            EvaluateIsCompleted();
        }
    }

    // only called when the quest containing this goal gets activated too
    public override void ActivateGoal()
    {
        _currentProgress = 0;
        EventManager.OnNpcDeath += TryUpdateProgress;
    }

    public override void Complete()
    {
        base.Complete();
        EventManager.OnNpcDeath -= TryUpdateProgress;
    }
}
