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
    public float WalkSpeed;
    public float RunSpeed;
    public float FallSpeed;
    public bool EnablePatroling;
    public LayerMask PlayerLayer;
    public LayerMask TerrainLayer;
    public float SightRange;
    public float AttackRange;
    public float PatrolRange;



    public enum State {
        Idle,
        Patroling,
        Attacking,
        Chasing
    }



    public AnimationClip IdleAnimation;    
    public AnimationClip WalkingAnimation;
    public AnimationClip BasicAttackAnimation;
    public AnimationClip RunningAnimation;



    protected Animator _animator;
    protected Player _player;
    protected AnimatorOverrideController _animatorOverrideController;
    protected NavMeshAgent _agent;
    protected Vector3 _patrolDestinationPoint;
    protected State _state;
    protected IEnumerator _patrolCoroutine;



    public bool IsAggressiveTowardsPlayer() {
        return _player.GetReputation(Alignment) < AggressivenessBorder;
    }

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
        _state = State.Idle;

        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;

        AnimationClipOverrides overrides = new AnimationClipOverrides(_animatorOverrideController.overridesCount);
        _animatorOverrideController.GetOverrides(overrides);

        overrides["Idle"] = IdleAnimation;
        overrides["Walking"] = WalkingAnimation;
        overrides["Basic Attack"] = BasicAttackAnimation;
        overrides["Running"] = RunningAnimation;

        _animatorOverrideController.ApplyOverrides(overrides);

        _patrolCoroutine = Patrol();

        if (EnablePatroling) {
            StartCoroutine(_patrolCoroutine);
        }

    }

    void Update() {
        bool playerInSightRange = Physics.CheckSphere(transform.position, SightRange, PlayerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerLayer);

        if (playerInAttackRange && playerInSightRange && IsAggressiveTowardsPlayer()) {
            StopCoroutine(_patrolCoroutine);
            _state = State.Attacking;
            AttackPlayer();
        } else if (!playerInAttackRange && playerInSightRange && IsAggressiveTowardsPlayer()) {
            StopCoroutine(_patrolCoroutine);
            _state = State.Chasing;
            ChasePlayer();
        }
    }

    private IEnumerator Patrol() {
        while (true) {
            yield return new WaitForSeconds(10f);

            _state = State.Patroling;
            bool destinationSet = false;
            Vector3 destination = new Vector3();
            while (!destinationSet) {
                float x = Random.Range(-PatrolRange, PatrolRange);
                float z = Random.Range(-PatrolRange, PatrolRange);

                destination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

                if (Physics.Raycast(destination, -transform.up, PatrolRange, TerrainLayer)) {
                    destinationSet = true;
                }
            }

            Vector3 distanceToDestination = transform.position - destination;

            while (distanceToDestination.magnitude > 1f) {
                _agent.SetDestination(destination);
                yield return null;
            }

            _state = State.Idle;
        }
        

    }

    private void AttackPlayer() {
        
    }

    private void ChasePlayer() {

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
