using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class MobState : MonoBehaviour
{
    public float _rotationSpeed = 3;
    public Transform target;
    public CharacterManager _characterManager;

    NavMeshAgent agent;
    Animation anim;
    
    int _damage = 25;
    int _maxHealth;
    int _currentHealth;
    bool _isDie;

    enum State
    {
        Idle,
        Run,
        Attack
    }

    State state;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _maxHealth = 50;
        _currentHealth = _maxHealth;
        state = State.Idle;
        _isDie = false;
    }

    void Update()
    {
        if (state == State.Idle)
        {
            UpdateIdle();
        }
        else if (state == State.Run)
        {
            UpdateRun();
        }
        else if (state == State.Attack)
        {
            UpdateAttack();
        }

    }

    private void UpdateAttack()
    {
        agent.speed = 0;
        //anim.Play("attack02");
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > 2)
        {
            state = State.Run;
            anim.Play("walk");
        }
    }

    private void UpdateRun()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= 2)
        {
            state = State.Attack;
            anim.Play("attack02");
        }

        agent.speed = 3.5f;
        agent.destination = target.transform.position;
    }

    private void UpdateIdle()
    {
        agent.speed = 0;
        target = GameObject.Find("Player").transform;
        if (target != null)
        {
            state = State.Run;
            anim.Play("walk");
        }
    }

    #region ÇÇ°Ý


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStatsManager playerStats = collision.GetComponent<PlayerStatsManager>();
            CharacterEffectsManager _playerEffectsManager = collision.GetComponent<CharacterEffectsManager>();
            CharacterManager _playercharacterManager = collision.GetComponent<CharacterManager>();

            if (_playercharacterManager != null)
            {
                if (_playercharacterManager.isParrying)
                {
                    _characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
            }

            if (playerStats != null)
            {
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                // _playerEffectsManager.PlayBloodSplatterFX(contactPoint);

                playerStats.TakeDamage(_damage);
            }
        }
    }

    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(2);
        state = State.Idle;
    }

    public void Death()
    {
        _currentHealth = 0;
        anim.Play("die01");
        StartCoroutine(DestroyMobs());
    }

    IEnumerator DestroyMobs()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    #endregion

}