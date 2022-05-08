using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<DialogueMessage> _dialogueMessages;

    void Awake()
    {
        _dialogueMessages = new Queue<DialogueMessage>();
    }

    public void StartDialogue(DialogueMessage[] dialogueMessages)
    {
        foreach(DialogueMessage m in dialogueMessages)
        {
            _dialogueMessages.Enqueue(m);
        }

        // TODO show UI
    }

    public void DisplayNextDialogueMessage()
    {
        if (_dialogueMessages.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueMessage message = _dialogueMessages.Dequeue();

        // TODO display
    }

    public void EndDialogue()
    {
        // TODO hide UI
    }
}
