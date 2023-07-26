using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterManager _characterManager;
    Collider _damageCollider;

    public int _currentWeaponDamage = 25;

    private void Awake()
    {
        _damageCollider = GetComponent<Collider>();
        _damageCollider.gameObject.SetActive(true);
        _damageCollider.isTrigger = true;
        _damageCollider.enabled = false;
    }

    public void EnableDamagecollider()
    {
        _damageCollider.enabled = true;
    }

    public void DisableDamagecollider()
    {
        _damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
             PlayerStats playerStats = collision.GetComponent<PlayerStats>();
             CharacterManager _enemycharacterManager = collision.GetComponent<CharacterManager>();

            if(_enemycharacterManager != null)
            {
                if(_enemycharacterManager.isParrying)
                {
                    _characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
            }
            if (playerStats != null)
            {
                playerStats.TakeDamage(_currentWeaponDamage);
            }
        }
        
        if (collision.tag == "Enemy")
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            CharacterManager _enemycharacterManager = collision.GetComponent<CharacterManager>();

            if (enemyStats != null)
            {
                enemyStats.TakeDamage(_currentWeaponDamage);
            }
        }
    }
}
