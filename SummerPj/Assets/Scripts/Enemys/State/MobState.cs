using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobState : CharacterStatsManager
{ 
    [SerializeField]
    Transform _target;
    NavMeshAgent _agent;

    float _maxHp = 100;
    float _currentHp;
    float _attackRange = 20;
    float _attackDamage;
    float _attackDelay;

    public UIEnemyHealthBar _enemyHealthBar;
    EnemyBossManager _enemyBossManager;
    EnemyAnimatorManager _enemyAnimatorManager;
    public bool _isBoss;
    public EnemyManager _enemyManager;
    CharacterStatsManager _characterState;
    public string _PhaseAnim;
    public IdleState _idleState;

    private void Awake()
    {
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _enemyBossManager = GetComponent<EnemyBossManager>();
        _currentHealth = _maxHealth;
        _maxHealth = SetMaxHealthFromHealthLevel();
        _enemyManager = GetComponent<EnemyManager>();
        _idleState = GetComponentInChildren<IdleState>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = true;
    }

    private void Start()
    {
        if (!_isBoss)
        {
            _enemyHealthBar.SetMaXHealth(_maxHealth);
        }
    }

    private void Update()
    {
        
    }

    private void IsAttack()
    {
        Vector3 _distnace = _target.position - this.transform.position;
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

    public override void TakeDamage(int _damege, string _damageAnimation = "Dead")
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

        _currentHealth -= _damege;
       

        if (_currentHealth <= 0)
        {
            _isDead = true;
            _enemyAnimatorManager.PlayTargetAnimation(_damageAnimation, true);
        }
    }

}
