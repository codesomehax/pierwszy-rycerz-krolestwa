using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class QuestManager : MonoBehaviour
{
    public List<Quest> ActiveQuests {get; private set;}
    public List<Quest> CompletedQuests {get; private set;} // completed, but the reward has not been received yet
    public List<Quest> FinishedQuests {get; private set;} // completed and reward received

    

    private void Awake()
    {
        ActiveQuests = new List<Quest>();
        CompletedQuests = new List<Quest>();
        FinishedQuests = new List<Quest>();

        EventManager.OnQuestCompleted += CompleteQuest;
    }

    public void AssignQuest(Quest quest)
    {
        ActiveQuests.Add(quest);
        quest.ActivateQuest();
    }

    public void CompleteQuest(Quest quest)
    {
        ActiveQuests.Remove(quest);
        CompletedQuests.Add(quest);
    }

    public void FinishQuest(Quest quest)
    {
        CompletedQuests.Remove(quest);
        FinishedQuests.Add(quest);
        quest.Finish();
    }
}
