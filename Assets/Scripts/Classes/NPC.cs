using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
* * There isn't a class called Monster or anything like that
* We can consider something a monster
* maybe if it wants to attack us, which we can detect using IsAggresiveTowardsPlayer() method
* which is based on the reputation of the player.
*
* * Adding new animations for NPCs
* If you want to add a new animation, for example talking, you have to do as follows:
*   In the NPC script:
*   - add a new public AnimationClip and select this animation in the editor
*   - if your animation is for example Talking, write additional line in Awake() function:
*     overrides["Talking"] = TalkingAnimation;
*   In the editor:
*   - search for the NPC animator and add a new AnimationState (wherever) with a new, EMPTY (no animation) AnimationClip called for example Talking
*   NOTE: the name of overrides["Talking"] and the AnimationClip name HAVE TO match
*/
public class NPC : Entity
{
    public Alliance Alignment;
    public float AggressivenessBorder; // aggressive if reputation is lower than this value
    public bool EnablePatroling;
    public LayerMask PlayerLayer;
    public LayerMask TerrainLayer;
    public float SightRange;
    public float AttackRange;
    public float PatrolRange;
    public float AttackSpeed;



    public enum State
    {
        Idle,
        Patroling,
        PatrolingWalking,
        Attacking,
        Chasing
    }

    public enum MovementBlendTreeAnimatorState
    {
        Idle = 0,
        Walking,
        Running
    }



    public AnimationClip IdleAnimation;    
    public AnimationClip WalkingAnimation;
    public AnimationClip BasicAttackAnimation;
    public AnimationClip RunningAnimation;



    protected Animator _animator;
    protected Player _player;
    protected AnimatorOverrideController _animatorOverrideController;
    protected NavMeshAgent _agent;
    protected State _state;
    protected bool _alreadyAttacked;
    protected bool _attackingRightNow;
    protected bool _destinationSet;
    protected Vector3 _destination;
    protected float _timeBetweenAttacks;



    public bool IsAggressiveTowardsPlayer()
    {
        return _player.GetReputation(Alignment) < AggressivenessBorder;
    }

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
        _agent = GetComponent<NavMeshAgent>();
        _state = State.Idle;
        _alreadyAttacked = false;
        _attackingRightNow = false;
        _destinationSet = false;
        _destination = new Vector3();
        _timeBetweenAttacks = 1f / AttackSpeed;

        // animatorOverrider

        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;

        AnimationClipOverrides overrides = new AnimationClipOverrides(_animatorOverrideController.overridesCount);
        _animatorOverrideController.GetOverrides(overrides);

        overrides["Base Idle"] = IdleAnimation;
        overrides["Base Walking"] = WalkingAnimation;
        overrides["Base Basic Attack"] = BasicAttackAnimation;
        overrides["Base Running"] = RunningAnimation;

        _animatorOverrideController.ApplyOverrides(overrides);

    }

    void Update()
    {
        bool playerInSightRange = Physics.CheckSphere(transform.position, SightRange, PlayerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerLayer);

        if (playerInAttackRange && /* playerInSightRange && */ IsAggressiveTowardsPlayer())
        {
            _state = State.Attacking;
            CancelInvoke(nameof(Patrol));
            AttackPlayer();
        }
        else if (!playerInAttackRange && playerInSightRange && IsAggressiveTowardsPlayer())
        {
            _state = State.Chasing;
            CancelInvoke(nameof(Patrol));
            if (_attackingRightNow) return;
            ChasePlayer();
        }
        else if (EnablePatroling && _state != State.Patroling && !_destinationSet)
        {
            _agent.SetDestination(transform.position);
            _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Idle);
            _state = State.Patroling;
            Invoke(nameof(Patrol), 10f);
        }
        else if (EnablePatroling && _destinationSet)
        {
            _state = State.PatrolingWalking;
            PatrolWalk();
        }
        else if (!EnablePatroling)
        {
            _state = State.Idle;
        }
    }

    private void Patrol()
    {
        while (!_destinationSet)
        {
            float x = Random.Range(-PatrolRange, PatrolRange);
            float z = Random.Range(-PatrolRange, PatrolRange);

            _destination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

            if (Physics.Raycast(_destination, -transform.up, PatrolRange, TerrainLayer))
                _destinationSet = true;
        }

        _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Walking);
    }

    private void PatrolWalk()
    {
        _agent.SetDestination(_destination);

        Vector3 distanceToDestination = transform.position - _destination;

        // destination reached
        if (distanceToDestination.magnitude < 1f)
        {
            _destinationSet = false;
            _state = State.Idle;
        }
    }

    private void AttackPlayer()
    {
        _agent.SetDestination(transform.position); // make sure enemy doesn't move

        transform.LookAt(_player.transform);
        _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Idle);

        if (!_alreadyAttacked)
        {
            _animator.Play("Base Basic Attack");
            _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Idle);
            _attackingRightNow = true;
            Invoke(nameof(ResetAttackingRightNowState), BasicAttackAnimation.length);
            _alreadyAttacked = true;
            Invoke(nameof(ResetAlreadyAttackedState), _timeBetweenAttacks);
        }    
    }

    private void ResetAlreadyAttackedState()
    {
        _alreadyAttacked = false;
    }

    private void ResetAttackingRightNowState()
    {
        _attackingRightNow = false;
        _animator.Play("Movement");
    }

    private void ChasePlayer()
    {
        _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Running);
        _agent.SetDestination(_player.transform.position);
    }




}


// Help class
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
