using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class MobState : CharacterStatsManager
{
    [SerializeField] Transform target;

    NavMeshAgent agent;
    Animation anim;

    public MobHealthBar mobHealthBar;

    int _damage = 25;
    public float _rotationSpeed = 3;

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
        mobHealthBar.SetMaXHealth(_maxHealth);
        state = State.Idle;
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
        
        if(_currentHealth > 0)
        {
            if (distance <= 2)
            {
                state = State.Attack;
                anim.Play("attack02");
            }
        }
        else
        {
            Death();
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

    public override void TakeDamage(int _damege, string _damageAnimation = "hit")
    {
        anim.Play("hit");

        _currentHealth -= _damage;
        mobHealthBar.SetHealth(_currentHealth);

        StartCoroutine(HitDelay());
        if (_currentHealth < 0)
        {
            Death();
        }
    }


    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(0.833f);
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