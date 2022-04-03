using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* * There isn't a class called Monster or anything like that
* We can consider something a monster
* maybe if it wants to attack us, which we can detect using IsAggresiveTowardsPlayer() method
* which is based on the reputation of the player.
*
* * If a monster has special traits, like unique animations, create new class inheriting NPC
*/
public class NPC : Entity
{
    public Alliance Alignment;
    public float AggressivenessBorder; // aggressive if reputation is lower than this value
    public float AggressivenessDistance; // distance from where the NPC will start chasing the player (if aggressive)



    protected Animator _animator;
    protected Player _player;



    private Dictionary<NPCAnimationState, string> _animationStates;



    public bool IsAggressiveTowardsPlayer() {
        return _player.GetReputation(Alignment) < AggressivenessBorder;
    }

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();

        _animationStates.Add(NPCAnimationState.Idle, "Idle");
        _animationStates.Add(NPCAnimationState.Walking, "Walking");
        _animationStates.Add(NPCAnimationState.BasicAttack, "Basic Attack");
    }
}

