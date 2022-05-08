using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class DialogueManager : MonoBehaviour
{
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
}
