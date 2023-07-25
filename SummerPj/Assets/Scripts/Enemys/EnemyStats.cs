using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

   // public UIEnemyHealthBar _enemyHealthBar;

    EnemyAnimatorManager _enemyAnimatorManager;

    private void Awake()
    {
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
    }

    private void Start()
    {
        _maxHealth = SetMaxHealthFromHealthLevel();
        _currentHealth = _maxHealth;
        //_enemyHealthBar.SetMaxHealth(_maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = _healthLevel * 10;
        return _maxHealth;

    }

    public void TakeDamageAnimation(int _damage)
    {
        _currentHealth -= _damage;
        //_enemyHealthBar.SetHealth(_currentHealth);

        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            _isDead = true;
        }
    }

    public void TakeDamage(int _damege, string _damageAnimation = "Stage2")
    {
        if (_isDead)
            return;

        _currentHealth -= _damege;
        //_enemyHealthBar.SetHealth(_currentHealth);

        _enemyAnimatorManager.PlayTargetAnimation(_damageAnimation, true);

        if (_currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        _currentHealth = 0;
        _enemyAnimatorManager.PlayTargetAnimation("Stage2", true);
        _isDead = true; 
    }
}
