using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager _characterManager;

    public int healthLevel = 10;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;

    public int staminaLevel = 10;
    [HideInInspector] public float _maxStamina;
    [HideInInspector] public float _currentStamina;

    public int focusLevel = 10;
    [HideInInspector] public float _maxFocusPoints;
    [HideInInspector] public float _currentFocusPoints;

    [HideInInspector] public bool _isDead;



    public virtual void TakeDamage(int damege, string damageAnimation)
    {
        _characterManager._characterSoundFXManager.PlayRandomDamageSoundFX();    
    }

    public virtual void TakeDamageNoAnimation(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            _isDead = true;
        }
    }
}
