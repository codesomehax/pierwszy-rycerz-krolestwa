using System.Collections.Generic;
using UnityEngine;

// Attached to every single talkable NPC
public class DialogueTrigger : MonoBehaviour, IComparer<DialogueTrigger>
{
    public DialogueMessage[] Messages;
    public int DialogueOrder = 1;



    private DialogueManager _dialagueManager;
    private Player _player;
    private bool _hasAlreadySpoken;



    void Awake()
    {
        _dialagueManager = FindObjectOfType<DialogueManager>();
        _player = FindObjectOfType<Player>();
        _hasAlreadySpoken = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F) && CanTalkRightNow())
        {
            transform.LookAt(_player.transform);
            _dialagueManager.StartDialogue(Messages);
        }
    }

    public bool CanTalkRightNow()
    {
        if (!_hasAlreadySpoken)
        {
            DialogueTrigger[] triggers = GetComponents<DialogueTrigger>();
        }
        return false;
    }

    public int Compare(DialogueTrigger x, DialogueTrigger y)
    {
        throw new System.NotImplementedException();
    }
}
