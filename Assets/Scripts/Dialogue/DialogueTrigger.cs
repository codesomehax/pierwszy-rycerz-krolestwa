using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

// Attached to a talkable NPC
public class DialogueTrigger : MonoBehaviour
{
    public NPCConversation Conversation;

    private NPC _npc;
    private Player _player;

    private void Awake()
    {
        _npc = GetComponent<NPC>();
        _player = FindObjectOfType<Player>();
        EventManager.OnReputationChange += UpdatePlayerReputation;
    }

    private void Update()
    {
        bool isInRange = Physics.CheckSphere(transform.position, _npc.TalkRange, _npc.PlayerLayer);

        if (Input.GetKeyDown(KeyCode.F) && isInRange && !ConversationManager.Instance.IsConversationActive)
        {
            ConversationManager.Instance.StartConversation(Conversation);
        }
    }

    private void UpdatePlayerReputation()
    {
        ConversationManager.Instance.SetInt("ReputationGood", _player.GetReputation(Alliance.Good));
        ConversationManager.Instance.SetInt("ReputationEvil", _player.GetReputation(Alliance.Evil));
    }
}
