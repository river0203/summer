using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterManager _characterManager;
    Collider _damageCollider;
    public bool enabledDamageColliderOnStartUp = false;   

    public int _currentWeaponDamage = 25;

    private void Awake()
    {
        _damageCollider = GetComponent<Collider>();
        _damageCollider.gameObject.SetActive(true);
        _damageCollider.isTrigger = true;
        _damageCollider.enabled = enabledDamageColliderOnStartUp;
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
            PlayerStatsManager playerStats = collision.GetComponent<PlayerStatsManager>();
            CharacterManager _enemycharacterManager = collision.GetComponent<CharacterManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

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
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
            CharacterManager _enemycharacterManager = collision.GetComponent<CharacterManager>();

            if (enemyStats != null)
            {
                enemyStats.TakeDamage(_currentWeaponDamage);
            }
            else if (shield != null && _enemycharacterManager.isBlocking)
            {
                float physicalDamageAfterBlock = _currentWeaponDamage - (_currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                    return;
                }
            }
        }

/*        if(collision.tag == "Illusionary Wall")
        {
            IllusionaryWall _illusionaryWall = collision.GetComponent<IllusionaryWall>();

            _illusionaryWall.wallHasBeenHit = true;
        }*/
    }
}
