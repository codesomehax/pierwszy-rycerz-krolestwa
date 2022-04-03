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
    public AnimationClip IdleAnimation;    
    public AnimationClip WalkingAnimation;
    public AnimationClip BasicAttackAnimation;



    protected Animator _animator;
    protected Player _player;
    protected AnimatorOverrideController _animatorOverrideController;



    public bool IsAggressiveTowardsPlayer() {
        return _player.GetReputation(Alignment) < AggressivenessBorder;
    }

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();

        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;

        AnimationClipOverrides overrides = new AnimationClipOverrides(_animatorOverrideController.overridesCount);
        _animatorOverrideController.GetOverrides(overrides);

        overrides["Idle"] = IdleAnimation;
        overrides["Walking"] = WalkingAnimation;
        overrides["Basic Attack"] = BasicAttackAnimation;

        _animatorOverrideController.ApplyOverrides(overrides);



    }
}

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) {}

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}
