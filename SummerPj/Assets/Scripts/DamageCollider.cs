using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterEffectsManager _characterEffectsManager;

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
        _characterEffectsManager = GetComponentInChildren<CharacterEffectsManager>();
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
                if (_playercharacterManager.isInvulerable) return;
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                _playerEffectsManager.PlayBloodSplatterFX(contactPoint);

                playerStats.TakeDamage(_currentWeaponDamage, currentDamageAnimation); 
            }
        }
        
        if (collision.tag == "Enemy")
        {
            PlayerStatsManager playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStatsManager>();
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
            CharacterEffectsManager _enemyEffectsManager = collision.GetComponent<CharacterEffectsManager>();
            CharacterManager _enemycharacterManager = collision.GetComponent<CharacterManager>();

            Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (enemyStats != null)
            {
                #region 적 타격시 마나 회복
                if (playerStats._currentFocusPoints < playerStats._maxFocusPoints)
                {
                    playerStats._currentFocusPoints += playerStats._hitFocusPlus;
                    if (playerStats._currentFocusPoints > playerStats._maxFocusPoints)
                    {
                        playerStats._currentFocusPoints = playerStats._maxFocusPoints;
                    }
                    playerStats._focusPointBar.SetCurrentFocusPoints(playerStats._currentFocusPoints);
                }
                #endregion

                enemyStats.TakeDamage(_currentWeaponDamage, currentDamageAnimation);
                _enemyEffectsManager.PlayBloodSplatterFX(contactPoint);
            }
        }
    }
    public void EnableDamagecollider()
    {
        if (_characterEffectsManager != null)
        {
            _characterEffectsManager.PlayWeaponFX();
        }
        _damageCollider.enabled = true;
    }

    public void DisableDamagecollider()
    {
        if (_characterEffectsManager != null)
        {
            _characterEffectsManager.StopWeaponFX();
        }
        _damageCollider.enabled = false;
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
