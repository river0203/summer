using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class MobState : CharacterStatsManager
{
    public Transform target;
    NavMeshAgent agent;
    Animation anim;
   
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
        state = State.Idle;
        _currentHealth = _maxHealth;
        _maxHealth = SetMaxHealthFromHealthLevel();
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
        StartCoroutine(AttackDelay());
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
            StartCoroutine(AttackDelay());
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

    IEnumerator AttackDelay()
    {
        anim.Play("attack02");
        yield return new WaitForSeconds(5);
    }

    #region ÇÇ°Ý

    private int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = 20;
        return _maxHealth;

    }

    public void TakeDamageAnimation(int _damage)
    {
        _currentHealth -= _damage;

        if (_currentHealth <= 0)
        {
            _isDead = true;
        }
        else
        {
            _isDead = false;
        }
    }

    public override void TakeDamage(int _damege, string _damageAnimation = "Stage2")
    {
        if (_isDead)
            return;

        _currentHealth -= _damege;
        anim.Play("hit");

        if (_currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        _currentHealth = 0;
        anim.Play("die01");
        _isDead = true;
    }

    #endregion

}