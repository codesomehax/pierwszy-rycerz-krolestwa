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
    public LayerMask EnemyLayer;
    public GameObject Sword;


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

        if (_attackingRightNow)
        {
            HitEnemy();
        }
    }

    public override void Die()
    {
        base.Die();
        GetComponent<HumanMovement>().enabled = false;
    }

    private void HitEnemy()
    {
        // I have got these vectors in the editor and they are based on the sword's position
        Vector3 center = new Vector3(0f, 0.9f, 0f);
        Vector3 halfExtents = new Vector3(0.1f, 0.81f, 0.04f);

        center += Sword.transform.position;

        Collider[] enemiesHit = Physics.OverlapBox(center, halfExtents, Sword.transform.rotation, EnemyLayer);

        foreach(Collider enemy in enemiesHit)
        {
            enemy.GetComponent<NPC>().TakeDamage(AttackDamage);
        }
    }

    protected override void Attack()
    {
        _animator.SetFloat("VelocityX", 0f);
        _animator.SetFloat("VelocityZ", 0f);

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

