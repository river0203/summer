using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStatsManager
{

    public UIEnemyHealthBar _UIenemyHealthBar;

    EnemyAnimatorManager _enemyAnimatorManager;

    private void Awake()
    {
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _UIenemyHealthBar = GetComponentInChildren<UIEnemyHealthBar>();
    }

    private void Start()
    {
        _maxHealth = SetMaxHealthFromHealthLevel();
        _currentHealth = _maxHealth;
        _UIenemyHealthBar.SetMaXHealth(_maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = _healthLevel * 10;
        return _maxHealth;

    }

    public void TakeDamageAnimation(int _damage)
    {
        _currentHealth -= _damage;
        _UIenemyHealthBar.SetHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _isDead = true;
        }
    }

    public override void TakeDamage(int _damege, string _damageAnimation = "Stage2")
    {
        if (_isDead)
            return;

        _currentHealth -= _damege;
        _UIenemyHealthBar.SetHealth(_currentHealth);

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
}
