using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Goal : MonoBehaviour
{
    public string Description;
    public int RequiredProgress;



    protected bool _completed;
    protected int _currentProgress;



    public void EvaluateIsCompleted()
    {
        if (_currentProgress >= RequiredProgress) Complete();
    }

    public void Complete()
    {
        _completed = true;
        EventManager.StartEventOnGoalCompleted(); // inform the containing quest that it should evaluate whether it's completed or not now
    }

    public bool IsCompleted()
    {
        return _completed;
    }


    // In this function event subscriptions take place
    public abstract void ActivateGoal();

    // * This function has to be implemented already in the inheritting class
    // private void TryUpdateProgress(params dynamic[] p);
}
