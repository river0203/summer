using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStatsManager
{
    AudioSource _audioSource;
    Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentHealth = SetMaxHealthFromHealthLevel();
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            _isDead = true;
        }
    }

    public override void TakeDamage(int damege, string damageAnimation /*= "Damage_01"*/)
    {
        if (_isDead) return;

        currentHealth -= damege;


        if (currentHealth <= 0)
        {
            currentHealth = 0;
            _anim.Play("Dead");
            _isDead = true;
        }
    }
}
