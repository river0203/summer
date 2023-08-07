using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStatsManager
{

    public UIEnemyHealthBar _enemyHealthBar;
    EnemyBossManager _enemyBossManager;
    EnemyAnimatorManager _enemyAnimatorManager;
    public bool _isBoss;
    EnemyManager _enemyManager;
    CharacterStatsManager _characterState;

    private void Awake()
    {
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _enemyBossManager = GetComponent<EnemyBossManager>();
        _currentHealth = _maxHealth;
        _maxHealth = SetMaxHealthFromHealthLevel();
    }

    private void Start()
    {
        if(!_isBoss)
        {
            _enemyHealthBar.SetMaXHealth(_maxHealth);
        }
    }

    private int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = _healthLevel * 10;
        return _maxHealth;

    }

    public void TakeDamageAnimation(int _damage)
    {
        _currentHealth -= _damage;
        _enemyHealthBar.SetHealth(_currentHealth);

        if (_currentHealth <= 0 && _enemyManager.isPhase == false)
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

        if(!_isBoss)
        {
            _enemyHealthBar.SetHealth(_currentHealth);
        }
        else if(_isBoss && _enemyBossManager != null)
        {
            _enemyBossManager.UpdateBossHealthBar(_currentHealth);
        }

        _currentHealth -= _damege;
        _enemyAnimatorManager.PlayTargetAnimation(_damageAnimation, true);

        if (_currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        _currentHealth = 0;
        _enemyAnimatorManager.PlayTargetAnimation("Dead", true);
        _isDead = true;
    }

    private void Phase2()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _enemyManager.detectionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            MobState _mobState = colliders[i].transform.GetComponent<MobState>();

            if(_mobState != null && _characterState._currentHealth < 0)
            {
                _enemyManager.isPhase = true;
                Debug.Log("Phase 2");
                _characterState._currentHealth += 40;

                _enemyManager.HandleStateMachine();
            }
            else
            {
                _enemyManager.isPhase = false;
            }
        }
    }
}
