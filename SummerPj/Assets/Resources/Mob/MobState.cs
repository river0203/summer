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
    bool _startIdle = false;
    bool _canAttack = false;
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
        anim.Play("getup");
        _maxHealth = 50;
        _currentHealth = _maxHealth;
        mobHealthBar.SetMaXHealth(_maxHealth);
        if(_startIdle )
        {
           // state = State.Idle;
        }
    }

    public void Playgetup()
    {
        _startIdle = false;
    }

    public void EndGetup()
    {
        _startIdle = true;
    }
    void Update()
    {
        if(_startIdle)
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
    }

    private void UpdateAttack()
    {
        agent.speed = 0;
        if(_canAttack)
        {
            anim.Play("attack02");
        }
        StartCoroutine(HitDelay());
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > 2)
        {
            state = State.Run;
        }
    }

    public void PlayAttack()
    {
        _canAttack = false;
    }

    public void EndAttack()
    {
        _canAttack = true;
    }

    private void UpdateRun()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        anim.Play("walk");
        if (_currentHealth > 0)
        {
            if (distance <= 2)
            {
                state = State.Attack;
                _canAttack = true;
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