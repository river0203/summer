using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorHandler _animHandler;
    PlayerStats _playerStats;
    PlayerManager _playerManager;
    InputHandler _inputHandler;
    PlayerInventory _playerInventory;
    WeaponSlotManager _weaponSlotManager;
    public string _lastAttack;

    LayerMask backStabLayer = 1 << 11;

    private void Awake()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
        _playerInventory = GetComponentInParent<PlayerInventory>();
        _playerManager = GetComponentInParent<PlayerManager>();
        _animHandler = GetComponent<AnimatorHandler>();
        _inputHandler = GetComponentInParent<InputHandler>();
        _weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
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

    private void AttemptBackStabOrRiposte()
    {
        RaycastHit hit;

        if (Physics.Raycast(_inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager _characterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        }
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

    private void SuccessfullyCastSpell()
    {
        _playerInventory._currentSpell.SucessfullyCastSpell(_animHandler, _playerStats);
    }
    #endregion
}
