using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    public float _staminaRegenerationAmount = 100;
    public float _staminaRegenTimer = 0;
    public float _staminaRegenTime = 0.5f;

    public HealthBar _healthBar;
    public StaminaBar _staminaBar;
    public FocusPointBar _focusPointBar;

    PlayerAnimatorManager _playerAnimationManager;
    InputHandler _inputHandler;
    PlayerManager _playerManager;

    int a;
    private void Awake()
    {
        _focusPointBar = FindObjectOfType<FocusPointBar>();
        _staminaBar = FindObjectOfType<StaminaBar>();
        _healthBar = FindObjectOfType<HealthBar>();
        
        _inputHandler = GetComponent<InputHandler>();
        _playerAnimationManager = GetComponent<PlayerAnimatorManager>();
        _playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        _currentHealth = SetMaxHealthFromHealthLevel();
        _healthBar.Init(_maxHealth);
            
        _currentStamina = SetMaxStaminaFromStaminaLevel();
        _staminaBar.Init(_maxStamina);

        _currentFocusPoints = SetMaxFocusPointsFromFocusLevel();
        _focusPointBar.Init(_maxFocusPoints);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = _healthLevel * 10;
        return _maxHealth;
    }

    private float SetMaxStaminaFromStaminaLevel()
    {
        _maxStamina = _staminaLevel * 10;
        return _maxStamina;
    }

    private float SetMaxFocusPointsFromFocusLevel()
    {
        _maxFocusPoints = _focusLevel * 10;
        return _maxFocusPoints;
    }

    public override void TakeDamage(int damege, string damageAnimation)
    {
        if (_playerManager.isInvulerable) 
            return;

        if (_isDead) 
            return;

        _currentHealth -= damege;

        _healthBar.SetCurrentHealth(_currentHealth);

        _playerAnimationManager.PlayTargetAnimation(damageAnimation, true);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _playerAnimationManager.PlayTargetAnimation("Dead", true);
            _isDead = true;
        }
    }

    public override void TakeDamageNoAnimation(int damage)
    {
        base.TakeDamageNoAnimation(damage);
        _healthBar.SetCurrentHealth(_currentHealth);
    }

    public void TakeStaminaDamage(int damege)
    {
        _currentStamina -= damege;

        _staminaBar.SetCurrentStamina(_currentStamina);
    }

    public void RegenerateStamina()
    {
        if(_playerManager._isInteracting || _inputHandler._sprintFlag)
        {
            _staminaRegenTimer = 0;
        }
        else 
        {
            _staminaRegenTimer += Time.deltaTime;
            if (_currentStamina < _maxStamina && _staminaRegenTimer > _staminaRegenTime)
            {
                _currentStamina += _staminaRegenerationAmount * Time.deltaTime;
                _staminaBar.SetCurrentStamina(Mathf.RoundToInt(_currentStamina));
            }
        }
    }

    public void HealPlayer(int healAmount)
    {
        _currentHealth = _currentHealth + healAmount;

        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        _healthBar.SetCurrentHealth(_currentHealth);
    }

    public void DeductFocusPoints(float focusPoints) 
    { 
        _currentFocusPoints = _currentFocusPoints - focusPoints;

        if(_currentFocusPoints < 0)
        {
            _currentFocusPoints = 0;
        }
        _focusPointBar.SetCurrentFocusPoints(_currentFocusPoints);
    }
}
