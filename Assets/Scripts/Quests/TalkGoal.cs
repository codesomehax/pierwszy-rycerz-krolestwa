using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkGoal : Goal
{
    public override void ActivateGoal()
    {
        
    }

    public void TryUpdateProgress()
    {
        _currentProgress++;
        EvaluateIsCompleted();
    }
}
