using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class DialogueManager : MonoBehaviour
{
    private Player _player;



    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }


    private void Update()
    {
        if (ConversationManager.Instance.IsConversationActive)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                ConversationManager.Instance.SelectNextOption();
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                ConversationManager.Instance.SelectPreviousOption();
            }
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
            {
                ConversationManager.Instance.PressSelectedOption();
            }
        }
    }

    // called only during conversation!!!
    public void UpdateConversationAccordingToQuestState(Quest quest)
    {
        if (quest.IsActive())
        {
            ConversationManager.Instance.SetBool(quest.name + "Active", true);
        }
        if (quest.IsCompleted())
        {
            ConversationManager.Instance.SetBool(quest.name + "Completed", true);
        }
        if (quest.IsFinished())
        {
            ConversationManager.Instance.SetBool(quest.name + "Finished", true);
        }
    }

    public void UpdateConversationReputation()
    {
        ConversationManager.Instance.SetInt("ReputationGood", _player.GetReputation(Alliance.Good));
        ConversationManager.Instance.SetInt("ReputationEvil", _player.GetReputation(Alliance.Evil));
    }
}
