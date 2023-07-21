using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{   
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
        _currentHealth -= damege;
        _anim.Play("Damaged");

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _anim.Play("Dead");
        }
    }
}
