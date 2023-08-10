using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStatsManager
{
    EnemySoundManager _soundManager;
    public UIEnemyHealthBar _enemyHealthBar;
    EnemyBossManager _enemyBossManager;
    EnemyAnimatorManager _enemyAnimatorManager;
    public bool _isBoss;
    public EnemyManager _enemyManager;
    CharacterStatsManager _characterState;
    public IdleState _idleState;
    List<string> _PhaseAnim;

    private void Awake()
    {
        _soundManager = GetComponentInChildren<EnemySoundManager>();
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _enemyBossManager = GetComponent<EnemyBossManager>();
        _currentHealth = _maxHealth;
        _maxHealth = SetMaxHealthFromHealthLevel();
        _enemyManager = GetComponent<EnemyManager>();
        _idleState = GetComponentInChildren<IdleState>();
    }

    private void Start()
    {
        if (!_isBoss)
        {
            _enemyHealthBar.SetMaXHealth(_maxHealth);
        }

        List<string> _PhaseAnim = new List<string>();
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

    public override void TakeDamage(int _damege ,string _damageAnimation)
    {
        if (_isDead)
            return;

        if (!_isBoss)
        {
            _enemyHealthBar.SetHealth(_currentHealth);
        }
        else if (_isBoss && _enemyBossManager != null)
        {
            _enemyBossManager.UpdateBossHealthBar(_currentHealth);
        }

        _soundManager.PlayBossHitSound();
        _currentHealth -= _damege;

        if (_currentHealth <= 0)
        {
            HandleDeath();
            _soundManager.PlayBossDeadSound();
        }
    }

    public void HandleDeath()
    {
        if (_enemyManager.isPhase)
        {
            _isDead = false;
            _currentHealth += 40;
            Debug.Log("ÆäÀÌÁî 2");
            
            int randomValue = Random.Range(0, _PhaseAnim.Count);
            _enemyAnimatorManager.PlayTargetAnimation(_PhaseAnim[randomValue], true);
            //_enemyManager.currentState = _idleState;
        }
        else
        {
            _currentHealth = 0;
            _enemyAnimatorManager.PlayTargetAnimation("Dead", true);
            _isDead = true;
        }

    }
}