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

    public IdleState _idleState;
    
    [SerializeField] bool _isBoss;
    
    [SerializeField] List<string> _PhaseAnim;
    [SerializeField] float detectionRadius = 20;
    [SerializeField] LayerMask _MobLayer;

    private void Awake()
    {
        _enemyBossManager = GetComponent<EnemyBossManager>();
        _enemyManager = GetComponent<EnemyManager>();
        _soundManager = GetComponentInChildren<EnemySoundManager>();
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _idleState = GetComponentInChildren<IdleState>();

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

/*    public void TakeDamageAnimation(int _damage)
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
    }*/

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
        Collider[] Mobcollider = Physics.OverlapSphere(transform.position, detectionRadius, _MobLayer);

        if (Mobcollider != null)
        {
            _isDead = false;
            _currentHealth += 40;
            Debug.Log("페이즈 2");
            
            // 부활 애니메이션을 랜덤으로 출력
            int randomValue = Random.Range(0, _PhaseAnim.Count);
            _enemyManager.isPreformingAction = true;
            _enemyAnimatorManager.PlayTargetAnimation(_PhaseAnim[randomValue], true);
            //_enemyManager.currentState = _idleState;
        }
        else
        {
            _currentHealth = 0;
            _enemyManager.isPreformingAction = true;
            _enemyAnimatorManager.PlayTargetAnimation("Dead", true);
            StartCoroutine(DestroyChar());
            _isDead = true;
        }
    }

    private IEnumerator DestroyChar()
    {
        yield return new WaitForSeconds(15);
        Destroy(gameObject);
    }
}