using JetBrains.Annotations;
using SG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    PlayerAnimatorManager _animHandler;
    PlayerEquipmentManager _playerEquipmentHandler;
    PlayerStats _playerStats;
    PlayerManager _playerManager;
    InputHandler _inputHandler;
    PlayerInventory _playerInventory;
    WeaponSlotManager _weaponSlotManager;
    public string _lastAttack;

    LayerMask riposteLayer = 1 << 12;
    LayerMask backStabLayer = 1 << 11;

    private void Awake()
    {
        _playerEquipmentHandler = GetComponent<PlayerEquipmentManager>();
        _playerStats = GetComponentInParent<PlayerStats>();
        _playerInventory = GetComponentInParent<PlayerInventory>();
        _playerManager = GetComponentInParent<PlayerManager>();
        _animHandler = GetComponent<PlayerAnimatorManager>();
        _inputHandler = GetComponentInParent<InputHandler>();
        _weaponSlotManager = GetComponent<WeaponSlotManager>();
    }
    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (_playerStats._currentStamina <= 0) return;

        if (_inputHandler._comboFlag)
        {
            _animHandler._anim.SetBool("canDoCombo", false);

            if (_lastAttack == weapon.OH_Light_Attack_1 && weapon.OH_Light_Attack_2 != "")
            {
                _animHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                _lastAttack = weapon.OH_Light_Attack_2;
            }
            else if (_lastAttack == weapon.OH_Light_Attack_2 && weapon.OH_Light_Attack_3 != "")
            {
                _animHandler.PlayTargetAnimation(weapon.OH_Light_Attack_3, true);
                _lastAttack = weapon.OH_Light_Attack_3;
            }
            else if(_lastAttack == weapon.TH_Light_Attack_01)
            {
                _animHandler.PlayTargetAnimation(weapon.TH_Light_Attack_02, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (_playerStats._currentStamina <= 0) return;

        _weaponSlotManager._attackingWeapon = weapon;

        if (_inputHandler._twoHandFlag) 
        {
            _animHandler.PlayTargetAnimation(weapon.TH_Light_Attack_01, true);
            _lastAttack = weapon.TH_Light_Attack_01; 
        }
        else
        {
            _animHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            _lastAttack = weapon.OH_Light_Attack_1;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (_playerStats._currentStamina <= 0) return;

        _weaponSlotManager._attackingWeapon = weapon;

        if (_inputHandler._twoHandFlag)
        {

        }
        else
        {

        }
        _animHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        _lastAttack = weapon.OH_Heavy_Attack_1;
    }

    #region Input Actions
    public void HandleRBAction()
    {
        if(_playerInventory._rightWeapon.isMeleeWeapon)
        {
            PerformRBMeleeAction();
        }
        else if(_playerInventory._rightWeapon.isSpellCaster || _playerInventory._rightWeapon.isFaithCaster || _playerInventory._rightWeapon.isPyroCaster) 
        {
            PerformRBMagicAction(_playerInventory._rightWeapon);
        }
    }

    public void HandleLBAtcion()
    {
        PerformLBBlockingAction();
    }

    public void HandleLTAction()
    {
        if(_playerInventory._rightWeapon.isMeleeWeapon)
        {
            PerformLTWeaponArt(_inputHandler._twoHandFlag);
        }
        /*        if(_playerInventory._leftWeapon.isShieldWeapon)
                {
                    PerformLTWeaponArt(_inputHandler._twoHandFlag);
                }*/
        else if(_playerInventory._leftWeapon.isMeleeWeapon)
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
            HandleWeaponCombo(_playerInventory._rightWeapon);
            _inputHandler._comboFlag = false;
        }
        else
        {
            if (_playerManager._isInteracting)
                return;
            if (_playerManager._canDoCombo)
                return;

            _animHandler._anim.SetBool("isUsingRightHand", true);
            HandleLightAttack(_playerInventory._rightWeapon);
        }
    }
    public void AttemptBackStabOrRiposte()
    {
        if (_playerStats._currentStamina <= 0) return;

        RaycastHit hit;

        if (Physics.Raycast(_inputHandler.criticalAttackRayCastStartPoint.position, 
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager _enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = _weaponSlotManager._rightHandDamageCollider;

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
            DamageCollider rightWeapon = _weaponSlotManager._rightHandDamageCollider;

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

                int criticalDamage = _playerInventory._rightWeapon.criticalDamageMultiplier * rightWeapon._currentWeaponDamage;
                _enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                _animHandler.PlayTargetAnimation("Riposte", true);
                _enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposte", true);
            }
        }
    }

    void PerformLBBlockingAction()
    {
        if(_playerManager._isInteracting) { return; }

        if(_playerManager.isBlocking) { return; }

        _animHandler.PlayTargetAnimation("Block Start", false, true);
        _playerEquipmentHandler.OpenBlockingCollider();
        _playerManager.isBlocking = true;
    }
    private void PerformRBMagicAction(WeaponItem weapon)
    {
        if (_playerManager._isInteracting)
            return;

        if(weapon.isFaithCaster)
        {
            if(_playerInventory._currentSpell != null && _playerInventory._currentSpell.isFaithSpell)
            {
                if(_playerStats._currentFocusPoints >= _playerInventory._currentSpell.focusPointCost)
                {
                    _playerInventory._currentSpell.AttemptToCastSpell(_animHandler, _playerStats);
                }
                else
                {
                    _animHandler.PlayTargetAnimation("Shrug", true);
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
            _animHandler.PlayTargetAnimation(_playerInventory._rightWeapon.weapon_art, true);
        }

    }

    private void SuccessfullyCastSpell()
    {
        _playerInventory._currentSpell.SucessfullyCastSpell(_animHandler, _playerStats);
    }
    #endregion
}
