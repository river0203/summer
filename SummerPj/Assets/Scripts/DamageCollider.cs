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
    protected string currentDamageAnimation;

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
            CharacterEffectsManager _playerEffectsManager = collision.GetComponent<CharacterEffectsManager>();
            CharacterManager _playercharacterManager = collision.GetComponent<CharacterManager>();
            float directionHitFrom = (Vector3.SignedAngle(_playercharacterManager.transform.forward, _playercharacterManager.transform.forward, Vector3.up)); 
            ChooseWhichDirectionDamageCameFrom(directionHitFrom);

            if(_playercharacterManager != null)
            {
                if(_playercharacterManager.isParrying)
                {
                    _characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
            }


            if (playerStats != null)
            {
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                // _playerEffectsManager.PlayBloodSplatterFX(contactPoint);

                playerStats.TakeDamage(_currentWeaponDamage, currentDamageAnimation);
            }
        }
        
        if (collision.tag == "Enemy")
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
            CharacterEffectsManager _enemyEffectsManager = collision.GetComponent<CharacterEffectsManager>();
            CharacterManager _enemycharacterManager = collision.GetComponent<CharacterManager>();

            Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            //_enemyEffectsManager.PlayBloodSplatterFX(contactPoint);

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

    public void ChooseWhichDirectionDamageCameFrom(float direction)
    {
        if(direction >= 145 && direction <= 180)
        {
            currentDamageAnimation = "Damage_Foward_01";
        }
        else if (direction <= -145 && direction >= 180)
        {
            currentDamageAnimation = "Damage_Foward_01";
        }
        else if (direction >= -45 && direction <= 180)
        {
            currentDamageAnimation = "Damage_Back_01";
        }
        else if (direction >= -144 && direction <= 180)
        {
            currentDamageAnimation = "Damage_Right_01";
        }
        else if (direction >= 45 && direction <= 180)
        {
            currentDamageAnimation = "Damage_Left_01";
        }
    }
}
