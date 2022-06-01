using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueEditor;
using SaveIsEasy;

/**
* Object supposed to hold player related properties and methods.
*/
public class Player : Entity
{
    public AnimationClip BasicAttack1;
    public AnimationClip BasicAttack2;
    public LayerMask EnemyLayer;
    public GameObject Sword;


    public static Player PlayerInstance;


    private int _reputationBonusesReceived;
    private int _currentAttackType;
    private int _nTrainings;
    private Dictionary<Alliance, int> _reputation;



    public int GetReputation(Alliance alliance)
    {
        return _reputation[alliance];
    }

    public void SetReputation(Alliance alliance, int reputation)
    {
        _reputation[alliance] = reputation;
    }

    public void IncreaseReputation(Alliance alliance, int reputation)
    {
        _reputation[alliance] += reputation;
    }

    public void DecreaseReputation(Alliance alliance, int reputation)
    {
        _reputation[alliance] -= reputation;
    }

    public void IncreaseBaseAttack(float attack)
    {
        BaseAttackDamage += attack;
    }

    public void IncreaseBaseDefense(float defense)
    {
        BaseDefense += defense;
    }

    public void IncreaseBaseMaxHP(float maxHP)
    {
        BaseMaxHP += maxHP;
    }

    public void IncreaseAttackMultiplier(float attackMultiplier)
    {
        AttackMultiplier += attackMultiplier;
    }

    public void IncreaseDefenseMultiplier(float defenseMultiplier)
    {
        DefenseMultiplier += defenseMultiplier;
    }

    public void IncreaseMaxHpMultiplier(float maxHpMultiplier)
    {
        MaxHpMultiplier += maxHpMultiplier;
    }

    protected override void Awake()
    {
        base.Awake();

        _reputation = new Dictionary<Alliance, int>();

        _reputation[Alliance.Good] = 100;
        _reputation[Alliance.Evil] = 100;

        _currentAttackType = 1;
        _animator.SetInteger("AttackType", 1);
        _nTrainings = 0;
        _reputationBonusesReceived = 0;

        EventManager.OnNpcDeath += GetLootFromEnemy;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !_alreadyAttacked && !ConversationManager.Instance.IsConversationActive)
        {
            Attack();
        }

        if (_attackingRightNow)
        {
            HitEnemy();
        }
    }

    private void LateUpdate()
    {
        if (SceneSwapper.DataLoadRequest)
        {
            if (SaveIsEasyAPI.FileExists(SceneManager.GetActiveScene().name + ".game"))
            {
                GetComponent<CharacterController>().enabled = false;
                SaveIsEasyAPI.LoadAll(SceneManager.GetActiveScene().name + ".game");
                GetComponent<CharacterController>().enabled = true;
            }
            SceneSwapper.DataLoadRequest = false;
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

    private void GetLootFromEnemy(NPC enemy)
    {
        _gold += enemy.GetGold();
        if (enemy.Alignment == Alliance.Good)
        {
            _reputation[Alliance.Good] = 0;
            _reputation[Alliance.Evil] += enemy.DeathReputationIncrease;
        }
        else
        {
            _reputation[Alliance.Evil] = 0;
            _reputation[Alliance.Good] += enemy.DeathReputationIncrease;
        }
    }

    public int GetNTrainings()
    {
        return _nTrainings;
    }

    public void IncreaseNTraining(int n)
    {
        _nTrainings += n;
    }

    public void ReceiveReputationBonus()
    {
        Alliance alliance = Alliance.Good;
        for (int i = 0; i < 2; i++)
        {
            int nBonusesDueToReputation = _reputation[alliance] / 100;

            if (nBonusesDueToReputation > 20) nBonusesDueToReputation = 20;

            _reputationBonusesReceived = nBonusesDueToReputation - _reputationBonusesReceived;

            AttackMultiplier += 0.05f * _reputationBonusesReceived;
            BaseDefense += 5f * _reputationBonusesReceived;

            alliance = Alliance.Evil;
        }
    }
}

