using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Object supposed to hold player related properties and methods.
*/
public class Player : Entity
{
    public AnimationClip BasicAttack1;
    public AnimationClip BasicAttack2;


    private int _currentAttackType;
    private Dictionary<Alliance, float> _reputation;

    public float GetReputation(Alliance alliance)
    {
        return _reputation[alliance];
    }

    protected override void Awake()
    {
        base.Awake();

        _reputation = new Dictionary<Alliance, float>();

        _reputation[Alliance.Good] = 0;
        _reputation[Alliance.Evil] = 0;

        _currentAttackType = 1;
        _animator.SetInteger("AttackType", 1);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !_alreadyAttacked)
        {
            Attack();
        }
    }

    public override void Die()
    {
        base.Die();
        GetComponent<HumanMovement>().enabled = false;
    }

    protected override void Attack()
    {
        _animator.SetFloat("VelocityX", 0f, 0.1f, Time.deltaTime);
        _animator.SetFloat("VelocityZ", 0f, 0.1f, Time.deltaTime);

        _attackingRightNow = true;
        _animator.SetBool("AttackingRightNow", true);
        Invoke(nameof(ResetAttackingRightNowState), (_currentAttackType == 1) ? BasicAttack1.length : BasicAttack2.length);
        
        _alreadyAttacked = true;
        GetComponent<HumanMovement>().enabled = false;
        Invoke(nameof(ResetAlreadyAttackedState), (_currentAttackType == 1) ? BasicAttack1.length : BasicAttack2.length);

    }

    protected override void ResetAlreadyAttackedState()
    {
        base.ResetAlreadyAttackedState();
        GetComponent<HumanMovement>().enabled = true;
    }

    protected override void ResetAttackingRightNowState()
    {
        _attackingRightNow = false;
        _animator.SetBool("AttackingRightNow", false);
        _currentAttackType = (_currentAttackType == 1) ? 2 : 1;
        _animator.SetInteger("AttackType", _currentAttackType);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        } 
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

