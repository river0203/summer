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

    public void TakeDamageNoAnimation(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _isDead = true;
        }
    }

    public override void TakeDamage(int damege, string damageAnimation = "Damage_01")
    {
        if (_isDead) return;

        _currentHealth -= damege;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _anim.Play("Dead");
            _isDead = true;
        }
    }
}
