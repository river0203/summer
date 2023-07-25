using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public float _staminaRegenerationAmount = 100;
    public float _staminaRegenTimer = 0;
    public float _staminaRegenTime = 0.5f;

    public HealthBar _healthBar;
    public StaminaBar _staminaBar;
    PlayerAnimatorManager _animHandler;
    InputHandler _inputHandler;
    PlayerManager _playerManager;
    public FocusPointBar _focusPointBar;
    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _focusPointBar = FindObjectOfType<FocusPointBar>();
        _playerManager = GetComponent<PlayerManager>();
        _healthBar = FindObjectOfType<HealthBar>();
        _staminaBar = FindObjectOfType<StaminaBar>();
        _animHandler = GetComponentInChildren<PlayerAnimatorManager>();
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

    public void TakeDamage(int damege)
    {
        if (_playerManager.isInvulerable) return;

        if (_isDead) return;

        _currentHealth -= damege;

        _healthBar.SetCurrentHealth(_currentHealth);

        _animHandler.PlayTargetAnimation("Damaged", true);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _animHandler.PlayTargetAnimation("Dead", true);
            _isDead = true;
        }
    }

    public void TakeDamageNoAnimation(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _isDead = true;
        }
    }

    public void TakeStaminaDamage(int damege)
    {
        _currentStamina -= damege;

        _staminaBar.SetCurrentStamina(_currentStamina);
    }

    public void RegenerateStamina()
    {
        if(_playerManager._isInteracting)
        {
            _staminaRegenTimer = 0;
        }
        else 
        {
            _staminaRegenTimer += Time.deltaTime;
            if (_currentStamina < _maxStamina && _staminaRegenTimer > _staminaRegenTime)
            {
                if (_inputHandler._sprintFlag) return;

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
