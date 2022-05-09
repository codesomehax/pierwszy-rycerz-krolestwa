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
    


    protected Animator _animator;
    protected bool _alreadyAttacked;
    protected bool _attackingRightNow;
    protected float _currentHP;
    protected bool _isAlive;
    protected int _gold;



    // There is no default Attack() method
    protected virtual void Attack() {}


    protected virtual void ResetAlreadyAttackedState()
    {
        _alreadyAttacked = false;
    }

    protected virtual void ResetAttackingRightNowState()
    {
        _attackingRightNow = false;
    }

    protected virtual void Awake()
    {
        _currentHP = MaxHP;
        _animator = GetComponent<Animator>();
        _alreadyAttacked = false;
        _attackingRightNow = false;
        _isAlive = true;
        _gold = 0;
        _animator.SetBool("IsAlive", true);
    }

    public bool IsAttackingRightNow()
    {
        return _attackingRightNow;
    }

    public bool IsAlive()
    {
        return _isAlive;
    }

    public float GetCurrentHP()
    {
        return _currentHP;
    }

    public void SetCurrentHP(float hp)
    {
        _currentHP = (hp < 0f) ? 0f : hp;
    }

    public void AddGold(int gold)
    {
        _gold += gold;
    }

    public void SubstractGold(int gold)
    {
        _gold -= gold;
    }

    public int GetGold()
    {
        return _gold;
    }

    public virtual void TakeDamage(float attackDamage)
    {
        float dmg = attackDamage - Defense;
        if (dmg > 0)
        {
            _currentHP -= dmg;
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

    public void Heal(float hp)
    {
        if (_currentHP + hp > MaxHP)
        {
            _currentHP = MaxHP;
        }
        else
        {
            _currentHP += hp;
        }
    }

    public virtual void Die()
    {
        _isAlive = false;
        _animator.SetBool("IsAlive", false);
        foreach(Collider collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }
        this.enabled = false;
    }
}
