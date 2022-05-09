using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest
{
    public string Name;
    public string Description;
    public int GoldReward;
    public List<Goal> Goals;



    private bool _completed;



    public void ActivateQuest()
    {
        EventManager.OnGoalCompleted += EvaluateIsCompleted;
        foreach(Goal goal in Goals)
        {
            goal.ActivateGoal();
        }
    }



    public void EvaluateIsCompleted()
    {
        if (Goals.All<Goal>(goal => goal.IsCompleted()))
        {
            Complete();
        }
    }

    public void Complete()
    {
        _completed = true;
        EventManager.StartEventOnQuestCompleted(this);
    }

    public bool IsCompleted()
    {
        return _completed;
    }
}
