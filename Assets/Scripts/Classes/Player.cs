using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueEditor;

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


    private int _currentAttackType;
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

    protected override void Awake()
    {
        base.Awake();

        _reputation = new Dictionary<Alliance, int>();

        _reputation[Alliance.Good] = 0;
        _reputation[Alliance.Evil] = 0;

        _currentAttackType = 1;
        _animator.SetInteger("AttackType", 1);

        SceneManager.sceneLoaded += OnSceneLoaded;
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

    void LateUpdate()
    {
        if (PlayerPrefs.GetInt("Last scene") == -1) PlayerPrefs.SetInt("Last scene", SceneManager.GetSceneByName("Woods").buildIndex);
        
        if (PlayerPrefs.GetInt("Last scene") != SceneManager.GetActiveScene().buildIndex)
        {

            SceneStartTransforms transforms = new SceneStartTransforms();
            Scene from = SceneManager.GetSceneByBuildIndex(PlayerPrefs.GetInt("Last scene"));
            Scene to = SceneManager.GetActiveScene();


            transform.position = transforms.FindByScenes(from, to);

            Debug.Log(transforms.FindByScenes(from, to));
            Debug.Log(transform.position);

            PlayerPrefs.SetInt("Last scene", SceneManager.GetActiveScene().buildIndex);
        }
        GetComponent<CharacterController>().enabled = true;

        // TODO
        if (Input.GetKey(KeyCode.F2))
        {
            transform.position = new Vector3(179.570007f, 301.850006f, 179.979996f);
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        try
        {
            GetComponent<CharacterController>().enabled = false;
        }
        catch (System.Exception) {}
    }
}

