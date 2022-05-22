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
    public float PatrolRange;
    public float AttackRange;
    public float TalkRange;
    public float PatrolTimeInterval;
    public int MaxGold;



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
    public AnimationClip HurtAnimation;    
    public AnimationClip DeathAnimation;


    protected Player _player;
    protected AnimatorOverrideController _animatorOverrideController;
    protected NavMeshAgent _agent;
    protected State _state;
    protected bool _destinationSet;
    protected Vector3 _destination;
    protected bool _patrolTimeIntervalPassed;
    protected bool _alreadyTookDamage;



    public bool IsAggressiveTowardsPlayer()
    {
        return _player.GetReputation(Alignment) < AggressivenessBorder;
    }

    public override void TakeDamage(float attackDamage)
    {
        if (_alreadyTookDamage) return;

        float dmg = attackDamage - Defense;
        if (dmg > 0)
        {
            _currentHP -= dmg;
            _alreadyTookDamage = true;
            if (!_attackingRightNow)
            {
                _animator.SetTrigger("Hurt");
            }
            if (_currentHP <= 0f)
            {
                _currentHP = 0f;
                Die();
            }
        }
    }

    public override void Die()
    {
        base.Die();
        DialogueTrigger t;
        if (TryGetComponent<DialogueTrigger>(out t))
        {
            t.enabled = false;
        }
        EventManager.StartEventOnNpcDeath(this);
    }

    protected override void Awake()
    {
        base.Awake();
        _player = FindObjectOfType<Player>();
        _agent = GetComponent<NavMeshAgent>();
        _state = State.Idle;
        _destinationSet = false;
        _destination = new Vector3();
        _patrolTimeIntervalPassed = false;
        _alreadyTookDamage = false;
        _gold = Random.Range(0, MaxGold + 1);
        _agent.speed = WalkSpeed;

        // animatorOverrider

        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;

        AnimationClipOverrides overrides = new AnimationClipOverrides(_animatorOverrideController.overridesCount);
        _animatorOverrideController.GetOverrides(overrides);

        overrides["Base Idle"] = IdleAnimation;
        overrides["Base Walking"] = WalkingAnimation;
        overrides["Base Basic Attack"] = BasicAttackAnimation;
        overrides["Base Running"] = RunningAnimation;
        overrides["Base Hurt"] = HurtAnimation;
        overrides["Base Death"] = DeathAnimation;

        _animatorOverrideController.ApplyOverrides(overrides);

    }

    private void Start()
    {
        if (!_isAlive)
        {
            _animator.Play("Base Death", _animator.GetLayerIndex("Base Layer"), 1f);
            _agent.SetDestination(transform.position);
        }
    }

    void Update()
    {
        if (_alreadyTookDamage && !_player.IsAttackingRightNow())
        {
            _alreadyTookDamage = false;
        }

        bool playerInSightRange = Physics.CheckSphere(transform.position, SightRange, PlayerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerLayer);

        if (!_destinationSet && _state == State.Patroling)
        {
            SetDestination();
        }

        if (playerInAttackRange && /* playerInSightRange && */ IsAggressiveTowardsPlayer() && _player.IsAlive()) // continously executed, Attacking
        {
            _state = State.Attacking;
            CancelInvoke(nameof(Patrol));
            Attack();
        }
        else if (!playerInAttackRange && playerInSightRange && IsAggressiveTowardsPlayer() && _player.IsAlive()) // continously executed, Chasing
        {
            _state = State.Chasing;
            CancelInvoke(nameof(Patrol));
            if (_attackingRightNow) return;
            ChasePlayer();
        }
        else if (EnablePatroling && _state != State.Patroling && !_destinationSet) // executed only once, Patroling
        {
            _agent.SetDestination(transform.position);
            _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Idle);
            _state = State.Patroling;
            Invoke(nameof(Patrol), PatrolTimeInterval);
        }
        else if (EnablePatroling && _destinationSet && _patrolTimeIntervalPassed) // continously executed, Patroling Walking
        {
            _state = State.PatrolingWalking;
            PatrolWalk();
        }
        else if (!EnablePatroling) // idle
        {
            _agent.SetDestination(transform.position);
            _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Idle, 0.1f, Time.deltaTime);
            _state = State.Idle;
        }
    }


    private void SetDestination()
    {
        float x = Random.Range(-PatrolRange, PatrolRange);
        float z = Random.Range(-PatrolRange, PatrolRange);

        _destination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(_destination, -transform.up, 2f, TerrainLayer))
        {
            _destinationSet = true;
        }
    }
    private void Patrol()
    {
        _patrolTimeIntervalPassed = true;
    }

    private void PatrolWalk()
    {
        _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Walking, 0.1f, Time.deltaTime);
        _agent.speed = WalkSpeed;
        _agent.SetDestination(_destination);

        Vector3 distanceToDestination = transform.position - _destination;

        // destination reached
        if (distanceToDestination.magnitude < 1f)
        {
            _destinationSet = false;
            _patrolTimeIntervalPassed = false;
            _state = State.Idle;
        }
    }

    protected override void Attack()
    {
        _agent.SetDestination(transform.position); // make sure enemy doesn't move

        transform.LookAt(_player.transform);
        _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Idle, 0.1f, Time.deltaTime);

        if (!_alreadyAttacked)
        {
            _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Idle, 0.1f, Time.deltaTime);
            _attackingRightNow = true;
            _animator.SetBool("AttackingRightNow", true);
            Invoke(nameof(ResetAttackingRightNowState), BasicAttackAnimation.length);
            _alreadyAttacked = true;
            Invoke(nameof(ResetAlreadyAttackedState), TimeBetweenAttacks);

            _player.TakeDamage(AttackDamage);
        }    
    }

    protected override void ResetAttackingRightNowState()
    {
        _attackingRightNow = false;
        _animator.SetBool("AttackingRightNow", false);
    }

    private void ChasePlayer()
    {
        _destinationSet = false;
        _animator.SetFloat("State", (float) MovementBlendTreeAnimatorState.Running, 0.1f, Time.deltaTime);
        _agent.speed = RunSpeed;
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
