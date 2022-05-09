using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour
{
    public string Name;
    public string Description;
    public int GoldReward;
    public List<Goal> Goals {get; private set;}



    private bool _completed;
    private bool _active;
    private bool _finished;

    

    private void Awake()
    {
        Goals = new List<Goal>();
        _completed = _active = _finished;
    }



    public void ActivateQuest()
    {
        EventManager.OnGoalCompleted += EvaluateIsCompleted;
        _active = true;
        foreach(Goal goal in GetComponents<Goal>())
        {
            Goals.Add(goal);
            goal.ActivateGoal();
        }
    }

    public void Complete()
    {
        _completed = true;
        _active = false;
        EventManager.OnGoalCompleted -= EvaluateIsCompleted;
        EventManager.StartEventOnQuestCompleted(this);
    }

    public void Finish()
    {
        _completed = false;
        _finished = true;
    }


    public void EvaluateIsCompleted()
    {
        if (Goals.All<Goal>(goal => goal.IsCompleted()))
        {
            Complete();
        }
    }




    public bool IsCompleted()
    {
        return _completed;
    }

    public bool IsActive()
    {
        return _active;
    }

    public bool IsFinished()
    {
        return _finished;
    }
}
