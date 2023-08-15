using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStatsManager
{
    EnemySoundManager _soundManager;
    EnemyBossManager _enemyBossManager;
    EnemyAnimatorManager _enemyAnimatorManager;
    EnemyManager _enemyManager;
    UIEnemyHealthBar _enemyHealthBar;
    CameraHandler _cameraHandler;
    CapsuleCollider _thisCollider;

    public IdleState _idleState;
    
    [SerializeField] bool _isBoss;
    
    [SerializeField] List<string> _PhaseAnim;
    [SerializeField] float detectionRadius = 100;
    [SerializeField] LayerMask _MobLayer;

    private void Awake()
    {
        _enemyBossManager = GetComponent<EnemyBossManager>();
        _enemyManager = GetComponent<EnemyManager>();
        _thisCollider = FindObjectOfType<CapsuleCollider>();

        _soundManager = GetComponentInChildren<EnemySoundManager>();
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _idleState = GetComponentInChildren<IdleState>();

        _cameraHandler = FindAnyObjectByType<CameraHandler>();

        _currentHealth = _maxHealth;
        _maxHealth = SetMaxHealthFromHealthLevel();
    }

    private void Start()
    {
        if (!_isBoss)
        {
            _enemyHealthBar.SetMaXHealth(_maxHealth);
        }
    }

    private int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = _healthLevel * 10;
        return _maxHealth;
    }

    public override void TakeDamage(int _damege ,string _damageAnimation)
    {
        if (_isDead)
            return;

        // HP가 줄어들고 UI에 반영
        _currentHealth -= _damege;

        if (!_isBoss)
        {
            _enemyHealthBar.SetHealth(_currentHealth);
        }
        else if (_isBoss && _enemyBossManager != null)
        {
            _enemyBossManager.UpdateBossHealthBar(_currentHealth);
        }

        _soundManager.PlayBossHitSound();

        if (_currentHealth <= 0)
        {
            HandleDeath();
            _soundManager.PlayBossDeadSound();
        }
    }

    public void HandleDeath()
    {
        GameObject[] Mobs = GameObject.FindGameObjectsWithTag("Mobs");

        if (Mobs != null)
        {
            _isDead = false;

            _currentHealth += 40;
            _enemyBossManager._bossHealthBar.SetBossMaxHealth(_currentHealth);

            // 부활 애니메이션을 랜덤으로 출력
            _enemyManager._isPreformingAction = true;
            int randomValue = Random.Range(0, _PhaseAnim.Count);
            _enemyAnimatorManager.PlayTargetAnimation(_PhaseAnim[randomValue], true);
        }
        else
        {
            _currentHealth = 0;
            _enemyManager._isPreformingAction = true;
            _enemyAnimatorManager.PlayTargetAnimation("Dead", true);
            _isDead = true;
            _thisCollider.enabled = false;
        }
    }
}