using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Entity class provides typical properties for alive creatures like HP or Attack, but it
* can obviously be used as well for the undead.
*/
public class Entity : InteractableObject
{
    public float MaxHP;
    public float AttackDamage;
    public float Defense;
    public float WalkSpeed;
    public float RunSpeed;
    public float FallSpeed;
    public float TimeBetweenAttacks;

    public Object Inventory; // TODO unless we decide to not implement any kind of inventory
    


    protected Animator _animator;
    protected bool _alreadyAttacked;
    protected bool _attackingRightNow;
    protected float _currentHP;



    // There is no default Attack() method
    protected virtual void Attack() {}


    protected void ResetAlreadyAttackedState()
    {
        _alreadyAttacked = false;
    }

    protected void ResetAttackingRightNowState()
    {
        _attackingRightNow = false;
        _animator.Play("Movement");
    }

    protected virtual void Awake()
    {
        _currentHP = MaxHP;
        _animator = GetComponent<Animator>();
        _alreadyAttacked = false;
        _attackingRightNow = false;
    }

    public float GetCurrentHP()
    {
        return _currentHP;
    }

    public void SetCurrentHP(float hp)
    {
        _currentHP = (hp < 0f) ? 0f : hp;
    }
}
