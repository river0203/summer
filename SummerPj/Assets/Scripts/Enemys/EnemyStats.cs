using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class EnemyStats : CharacterStats
{
    public int _healthLevel = 10;
    public int _maxHealth;
    public int _currentHealth;
    
    Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _currentHealth = SetMaxHealthFromHealthLevel();
    }

    private int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = _healthLevel * 10;
        return _maxHealth;
    }

    public void TakeDamage(int damege)
    {
        if (isDead) return;

        _currentHealth -= damege;
        _anim.Play("Damaged");

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _anim.Play("Dead");
            isDead = true;   
        }
    }
}
