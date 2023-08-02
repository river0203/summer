using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [HideInInspector] public int _healthLevel = 10;
    [HideInInspector] public int _maxHealth;
    [HideInInspector] public int _currentHealth;

    [HideInInspector] public int _staminaLevel = 10;
    [HideInInspector] public float _maxStamina;
    [HideInInspector] public float _currentStamina;

    [HideInInspector] public int _focusLevel = 10;
    [HideInInspector] public float _maxFocusPoints;
    [HideInInspector] public float _currentFocusPoints;

    [HideInInspector] public bool _isDead;

    public virtual void TakeDamage(int damege, string damageAnimation)
    {

    }

    public virtual void TakeDamageNoAnimation(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _isDead = true;
        }
    }

    public void DestroyObj()
    {
        if(_isDead == true)
        {
            StartCoroutine(DestroyChar());
        }
    }

    private IEnumerator DestroyChar()
    {
        yield return new WaitForSeconds(15);
        Destroy(gameObject);
        
    }
}
