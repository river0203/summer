using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public int _healthLevel = 10;
    public int _maxHealth;
    public int _currentHealth;

    public int _focusLevel = 10;
    public float _maxFocusPoints = 100;
    public float _currentFocusPoints;

    public int _staminaLevel = 10;
    public float _maxStamina = 100;
    public float _currentStamina;

    public float staminaRegenerationAmount = 100;
    public float staminaRegenTimer = 0;
    public float staminaRegenTime = 0.5f;

    public HealthBar _healthBar;
    public StaminaBar _staminaBar;
    AnimatorHandler _animHandler;
    PlayerManager _playerManager;
    public FocusPointBar _focusPointBar;
    private void Awake()
    {
        _focusPointBar = FindObjectOfType<FocusPointBar>();
        _playerManager = GetComponent<PlayerManager>();
        _healthBar = FindObjectOfType<HealthBar>();
        _staminaBar = FindObjectOfType<StaminaBar>();
        _animHandler = GetComponentInChildren<AnimatorHandler>();
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

    public void TakeStaminaDamage(int damege)
    {
        _currentStamina -= damege;

        _staminaBar.SetCurrentStamina(_currentStamina);
    }

    public void RegenerateStamina()
    {
        if(_playerManager._isInteracting)
        {
            staminaRegenTimer = 0;
        }
        else 
        {
            staminaRegenTimer += Time.deltaTime;
            if (_currentStamina < _maxStamina && staminaRegenTimer > staminaRegenTime)
            {
                _currentStamina += staminaRegenerationAmount * Time.deltaTime;
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
