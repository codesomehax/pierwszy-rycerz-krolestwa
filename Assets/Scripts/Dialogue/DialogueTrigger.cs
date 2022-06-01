using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

// Attached to a talkable NPC
public class DialogueTrigger : MonoBehaviour
{
    public NPCConversation Conversation;
    public bool IsFirstConversation = false;

    private NPC _npc;
    private Player _player;
    private bool _playerStoppedDueToConversation;

    private void Awake()
    {
        _npc = GetComponent<NPC>();
        _player = FindObjectOfType<Player>();
        _playerStoppedDueToConversation = false;
    }

    private void Update()
    {
        bool isInRange = Physics.CheckSphere(transform.position, _npc.TalkRange, _npc.PlayerLayer);

        if ((Input.GetKeyDown(KeyCode.F) && isInRange && !ConversationManager.Instance.IsConversationActive && !_npc.IsAggressiveTowardsPlayer()) || IsFirstConversation)
        {
            _player.GetComponent<HumanMovement>().enabled = false;
            _playerStoppedDueToConversation = true;

            StartCoroutine(nameof(SmoothTurnToEachother));

            ConversationManager.Instance.StartConversation(Conversation);
        }
        else if (_playerStoppedDueToConversation && !ConversationManager.Instance.IsConversationActive)
        {
            _player.GetComponent<HumanMovement>().enabled = true;
            StopCoroutine(nameof(SmoothTurnToEachother));
            _playerStoppedDueToConversation = false;
        }

        if (IsFirstConversation) Destroy(gameObject);
    }

    private IEnumerator SmoothTurnToEachother()
    {
        Quaternion playerAtNPC = Quaternion.LookRotation(transform.position - _player.transform.position);
        Quaternion npcAtPlayer = Quaternion.LookRotation(_player.transform.position - transform.position);

        while (true)
        {
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, playerAtNPC, Time.deltaTime * 2f);
            transform.rotation = Quaternion.Slerp(transform.rotation, npcAtPlayer, Time.deltaTime * 2f);
            yield return null;
        }
    }
}
