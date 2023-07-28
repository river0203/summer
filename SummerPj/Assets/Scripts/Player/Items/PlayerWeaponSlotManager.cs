using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    PlayerManager _playerManager;
    PlayerInventoryManager _playerInventoryManager;
    Animator animator;
    QuickSlotsUI _quickSlotsUI;
    PlayerStatsManager _playerStats;
    InputHandler _inputHandler;
    PlayerEffectsManager _playerEffectsManager;

    public WeaponItem _attackingWeapon;


    private void Awake()
    {
        _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        _playerManager = GetComponent<PlayerManager>();
        _playerInventoryManager = GetComponent<PlayerInventoryManager>();
        animator = GetComponentInChildren<Animator>();
        _quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        _playerStats = GetComponent<PlayerStatsManager>();
        _inputHandler = GetComponent<InputHandler>();
        LoadWeaponHolderSlots();
    }

    void LoadWeaponHolderSlots()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponslot in weaponHolderSlots)
        {
            if (weaponslot._isLeftHandSlot)
            {
                _leftHandSlot = weaponslot;
            }
            else if (weaponslot._isRightHandSlot)
            {
                _rightHandSlot = weaponslot;
            }
            else if (weaponslot._isBackSlot)
            {
                _backSlot = weaponslot;
            }
        }
    }

    public void LoadBothWeaponsOnSlot()
    {
        LoadWeaponOnSlot(_playerInventoryManager._rightWeapon, false);
        LoadWeaponOnSlot(_playerInventoryManager._leftWeapon, true);
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if(weaponItem != null)
        {
            if (isLeft)
            {
                _leftHandSlot.currentWeapon = weaponItem;
                _leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                _quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
            }
            else
            {
                if (_inputHandler._twoHandFlag)
                {
                    _backSlot.LoadWeaponModel(_leftHandSlot.currentWeapon);
                    _leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.th_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Both Arms Empty", 0.2f);
                    animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                    _backSlot.UnloadWeaponAndDestroy();
                }
                _rightHandSlot.currentWeapon = weaponItem;
                _rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                _quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
        }
        else
        {
            weaponItem = _unarmedWeapon;
            if(isLeft)
            {
                animator.CrossFade("Left Arm Empty", 0.2f);
                _playerInventoryManager._leftWeapon = _unarmedWeapon;
                _leftHandSlot.currentWeapon = _unarmedWeapon;
                _leftHandSlot.LoadWeaponModel(_unarmedWeapon);
                LoadLeftWeaponDamageCollider();
                _quickSlotsUI.UpdateWeaponQuickSlotsUI(true, _unarmedWeapon);
            }
            else
            {
                animator.CrossFade("Right Arm Empty", 0.2f);
                _playerInventoryManager._rightWeapon = _unarmedWeapon;
                _rightHandSlot.currentWeapon = _unarmedWeapon;
                _rightHandSlot.LoadWeaponModel(_unarmedWeapon);
                LoadRightWeaponDamageCollider();
                _quickSlotsUI.UpdateWeaponQuickSlotsUI(false, _unarmedWeapon);
            }
        }
        // 무기 로드


        // 슬롯 UI 로드
        _quickSlotsUI.UpdateWeaponQuickSlotsUI(isLeft, weaponItem);
    }

    #region 데미지 콜라이더
    private void LoadLeftWeaponDamageCollider()
    {
        if (_leftHandSlot._currentWeaponModel == null)
            return;

        _leftHandDamageCollider = _leftHandSlot._currentWeaponModel.GetComponentInChildren<DamageCollider>();
        _leftHandDamageCollider._currentWeaponDamage = _playerInventoryManager._leftWeapon.baseDamage;
        _playerEffectsManager._leftWeaponFX = _leftHandSlot._currentWeaponModel.GetComponentInChildren<WeaponFX>();   
    }
    private void LoadRightWeaponDamageCollider()
    {
        if (_rightHandSlot._currentWeaponModel == null)
            return;

        _rightHandDamageCollider = _rightHandSlot._currentWeaponModel.GetComponentInChildren<DamageCollider>();
        _rightHandDamageCollider._currentWeaponDamage = _playerInventoryManager._rightWeapon.baseDamage;
        _playerEffectsManager._rightWeaponFX = _rightHandSlot._currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }
    public void OpenLeftDamageCollier()
    {
        _leftHandDamageCollider.EnableDamagecollider();
    }
    public void CloseLeftDamageCollier()
    {
        _leftHandDamageCollider.DisableDamagecollider();
    }
    public void OpenRightDamageCollier()
    {
        _rightHandDamageCollider.EnableDamagecollider();
    }
    public void CloseRightDamageCollier()
    {
        _rightHandDamageCollider.DisableDamagecollider();
    }
    #endregion

    #region 스테미나
    public void DrainStaminaLightAttack()
    {
        _playerStats.TakeStaminaDamage(Mathf.RoundToInt(_attackingWeapon.baseStaminar * _attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        _playerStats.TakeStaminaDamage(Mathf.RoundToInt(_attackingWeapon.baseStaminar * _attackingWeapon.heavyAttackMultiplier));
    }
    #endregion
}
