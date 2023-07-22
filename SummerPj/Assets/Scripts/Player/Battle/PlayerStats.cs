using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public int _healthLevel = 10;
    public int _maxHealth;
    public int _currentHealth;

    public int _staminaLevel = 10;
    public float _maxStamina = 100;
    public float _currentStamina;
    public float staminaRegenerationAmount = 1;
    public float staminaRegenTimer = 0;
    public float staminaRegenTime = 0.5f;

    public HealthBar _healthBar;
    public StaminaBar _staminaBar;
    AnimatorHandler _animHandler;
    PlayerManager _playerManager;

    private void Awake()
    {
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

    public void TakeDamage(int damege)
    {
        if (_playerManager.isInvulerable) return;

        if (isDead) return;

        _currentHealth -= damege;

        _healthBar.SetCurrentHealth(_currentHealth);

        _animHandler.PlayTargetAnimation("Damaged", true);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _animHandler.PlayTargetAnimation("Dead", true);
            isDead = true;
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
        currentHealth = currentHealth + healAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        _healthBar.SetCurrentHealth(currentHealth);
    }
}
