using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int _healthLevel = 10;
    public int _maxHealth;
    public int _currentHealth;

    public HealthBar _healthBar;

    AnimatorHandler _animHandler;

    private void Awake()
    {
        _animHandler = GetComponentInChildren<AnimatorHandler>();
    }

    private void Start()
    {
        _currentHealth = SetMaxHealthFromHealthLevel();
        _healthBar.SetMaxHealth(_maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = _healthLevel * 10;
        return _maxHealth;
    }

    public void TakeDamege(int damege)
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
}
