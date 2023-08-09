using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class MobDamageCollider : MonoBehaviour
{
    public CharacterManager _characterManager;
    Collider _damageCollider;
    public bool enabledDamageColliderOnStartUp = true;

    public int _currentWeaponDamage = 25;

    private void Awake()
    {
        _damageCollider = GetComponent<Collider>();

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

            if (_playercharacterManager != null)
            {
                if (_playercharacterManager.isParrying)
                {
                    _characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
            }

            if (playerStats != null)
            {
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                // _playerEffectsManager.PlayBloodSplatterFX(contactPoint);

                playerStats.TakeDamage(_currentWeaponDamage);
            }
        }
    }
}
