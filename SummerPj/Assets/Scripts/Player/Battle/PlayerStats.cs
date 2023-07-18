using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int _healthLevel = 10;
    public int _maxHealth;
    public int _currentHealth;

    public int _staminaLevel = 10;
    public int _maxStamina;
    public int _currentStamina;

    public HealthBar _healthBar;
    public StaminaBar _staminaBar;
    AnimatorHandler _animHandler;

    private void Awake()
    {
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

    private int SetMaxStaminaFromStaminaLevel()
    {
        _maxStamina = _staminaLevel * 10;
        return _maxStamina;
    }

    public void TakeDamage(int damege)
    {
        _currentHealth -= damege;

        _healthBar.SetCurrentHealth(_currentHealth);

        _animHandler.PlayTargetAnimation("Damaged", true);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _animHandler.PlayTargetAnimation("Dead", true);
        }
    }

    public void TakeStaminaDamage(int damege)
    {
        _currentStamina -= damege;

        _staminaBar.SetCurrentStamina(_currentStamina);
    }
}
