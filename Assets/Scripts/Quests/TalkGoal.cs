using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkGoal : Goal
{
    public override void ActivateGoal()
    {
        _currentProgress = 0;
    }

    public void TryUpdateProgress()
    {
        _currentProgress++;
        EvaluateIsCompleted();
    }
}
