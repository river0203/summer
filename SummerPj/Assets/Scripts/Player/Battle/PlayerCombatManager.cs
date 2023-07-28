using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    PlayerAnimatorManager _playerAnimatorManager;
    PlayerEquipmentManager _playerEquipmentHandler;
    PlayerStatsManager _playerStatsManager;
    PlayerManager _playerManager;
    InputHandler _inputHandler;
    PlayerInventoryManager _playerInventoryManager;
    PlayerWeaponSlotManager _playerWeaponSlotManager;
    PlayerEffectsManager _playerEffectsManager;
    CameraHandler _cameraHandler;
    public string _lastAttack;

    LayerMask riposteLayer = 1 << 12;
    LayerMask backStabLayer = 1 << 11;

    private void Awake()
    {
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        _playerEquipmentHandler = GetComponent<PlayerEquipmentManager>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
        _playerInventoryManager = GetComponent<PlayerInventoryManager>();
        _playerManager = GetComponent<PlayerManager>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        _inputHandler = GetComponent<InputHandler>();
        _playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }
    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (_playerStatsManager._currentStamina <= 0) return;

        if (_inputHandler._comboFlag)
        {
            _playerAnimatorManager._anim.SetBool("canDoCombo", false);

            if (_lastAttack == weapon.OH_Light_Attack_1 && weapon.OH_Light_Attack_2 != "")
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                _lastAttack = weapon.OH_Light_Attack_2;
            }
            else if (_lastAttack == weapon.OH_Light_Attack_2 && weapon.OH_Light_Attack_3 != "")
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_3, true);
                _lastAttack = weapon.OH_Light_Attack_3;
            }
            else if(_lastAttack == weapon.TH_Light_Attack_01)
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Attack_02, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (_playerStatsManager._currentStamina <= 0) return;

        _playerWeaponSlotManager._attackingWeapon = weapon;

        if (_inputHandler._twoHandFlag) 
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Attack_01, true);
            _lastAttack = weapon.TH_Light_Attack_01; 
        }
        else
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            _lastAttack = weapon.OH_Light_Attack_1;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (_playerStatsManager._currentStamina <= 0) return;

        _playerWeaponSlotManager._attackingWeapon = weapon;

        if (_inputHandler._twoHandFlag)
        {

        }
        else
        {

        }
        _playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        _lastAttack = weapon.OH_Heavy_Attack_1;
    }

    #region Input Actions
    public void HandleRBAction()
    {
        if(_playerInventoryManager._rightWeapon.isMeleeWeapon)
        {
            PerformRBMeleeAction();
        }
        else if(_playerInventoryManager._rightWeapon.isSpellCaster || _playerInventoryManager._rightWeapon.isFaithCaster || _playerInventoryManager._rightWeapon.isPyroCaster) 
        {
            PerformRBMagicAction(_playerInventoryManager._rightWeapon);
        }
    }

    public void HandleLBAtcion()
    {
        PerformLBBlockingAction();
    }

    public void HandleLTAction()
    {
        if(_playerInventoryManager._rightWeapon.isMeleeWeapon)
        {
            PerformLTWeaponArt(_inputHandler._twoHandFlag);
        }
        /*        if(_playerInventory._leftWeapon.isShieldWeapon)
                {
                    PerformLTWeaponArt(_inputHandler._twoHandFlag);
                }*/
        else if(_playerInventoryManager._leftWeapon.isMeleeWeapon)
        {

        }
    }
    #endregion

    #region Attack Actions
    private void PerformRBMeleeAction()
    {
        if (_playerManager._canDoCombo)
        {
            _inputHandler._comboFlag = true;
            HandleWeaponCombo(_playerInventoryManager._rightWeapon);
            _inputHandler._comboFlag = false;
        }
        else
        {
            if (_playerManager._isInteracting)
                return;
            if (_playerManager._canDoCombo)
                return;

            _playerAnimatorManager._anim.SetBool("isUsingRightHand", true);
            HandleLightAttack(_playerInventoryManager._rightWeapon);
        }

        _playerEffectsManager.PlayWeaponFX(false); 
    }
    public void AttemptBackStabOrRiposte()
    {
        if (_playerStatsManager._currentStamina <= 0) return;

        RaycastHit hit;

        if (Physics.Raycast(_inputHandler.criticalAttackRayCastStartPoint.position, 
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager _enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = _playerWeaponSlotManager._rightHandDamageCollider;

/*            if(_enemyCharacterManager != null )
            {
                _playerManager.transform.position = _enemyCharacterManager._backStabCollider.criticalDamageStandPosition.position;

                _playerManager.transform.position = _enemyCharacterManager._backStabCollider.criticalDamageStandPosition.position;
                Vector3 rotationDirection = _playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - _playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(_playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                _playerManager.transform.rotation = targetRotation;

                int criticalDamage = _playerInventory._rightWeapon.criticalDamageMultiplier * rightWeapon._currentWeaponDamage;
                _enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                _animHandler.PlayTargetAnimation("Back Stab", true);
                _enemyCharacterManager.GetComponentInChildren<EnemyAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
            }*/
        }
        else if (Physics.Raycast(_inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, backStabLayer))
        {
            CharacterManager _enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = _playerWeaponSlotManager._rightHandDamageCollider;

            if(_enemyCharacterManager != null && _enemyCharacterManager.canBeRiposted)
            {
                _playerManager.transform.position = _enemyCharacterManager._riposteCollider.criticalDamageStandPosition.position;

                Vector3 rotationDirection = _playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - _playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(_playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                _playerManager.transform.rotation = targetRotation;

                int criticalDamage = _playerInventoryManager._rightWeapon.criticalDamageMultiplier * rightWeapon._currentWeaponDamage;
                _enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                _playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                _enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposte", true);
            }
        }
    }

    void PerformLBBlockingAction()
    {
        if(_playerManager._isInteracting) { return; }

        if(_playerManager.isBlocking) { return; }

        _playerAnimatorManager.PlayTargetAnimation("Block Start", false, true);
        _playerEquipmentHandler.OpenBlockingCollider();
        _playerManager.isBlocking = true;
    }
    private void PerformRBMagicAction(WeaponItem weapon)
    {
        if (_playerManager._isInteracting)
            return;

        if(weapon.isFaithCaster)
        {
            if(_playerInventoryManager._currentSpell != null && _playerInventoryManager._currentSpell.isFaithSpell)
            {
                if(_playerStatsManager._currentFocusPoints >= _playerInventoryManager._currentSpell.focusPointCost)
                {
                    _playerInventoryManager._currentSpell.AttemptToCastSpell(_playerAnimatorManager, _playerStatsManager, _playerWeaponSlotManager);
                }
                else
                {
                    _playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
        else if(weapon.isPyroCaster)
        {
            if (_playerInventoryManager._currentSpell != null && _playerInventoryManager._currentSpell.isPyroSpell)
            {
                if (_playerStatsManager._currentFocusPoints >= _playerInventoryManager._currentSpell.focusPointCost)
                {
                    _playerInventoryManager._currentSpell.AttemptToCastSpell(_playerAnimatorManager, _playerStatsManager, _playerWeaponSlotManager);
                }
                else
                {
                    _playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
    }

    private void PerformLTWeaponArt(bool isTwoHanding)
    {
        if (_playerManager._isInteracting) return;

        if (isTwoHanding)
        {
        }
        else 
        {
            _playerAnimatorManager.PlayTargetAnimation(_playerInventoryManager._rightWeapon.weapon_art, true);
        }

    }

    private void SuccessfullyCastSpell()
    {
        _playerInventoryManager._currentSpell.SucessfullyCastSpell(_playerAnimatorManager, _playerStatsManager, _cameraHandler, _playerWeaponSlotManager);
        _playerAnimatorManager._anim.SetBool("isFiringSpell", true);
    }
    #endregion
}
