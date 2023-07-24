using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    PlayerManager _playerManager;
    PlayerInventory _playerInventory;
    public WeaponItem _attackingWeapon;

    WeaponHolderSlot _leftHandSlot;
    WeaponHolderSlot _rightHandSlot;
    WeaponHolderSlot _backSlot;

    public DamageCollider _leftHandDamageCollider;
    public DamageCollider _rightHandDamageCollider;

    Animator animator;

    QuickSlotsUI _quickSlotsUI;

    PlayerStats _playerStats;
    InputHandler _inputHandler;

    private void Awake()
    {
        _playerManager = GetComponentInParent<PlayerManager>();
        _playerInventory = GetComponentInParent<PlayerInventory>();
        animator = GetComponent<Animator>();
        _quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        _playerStats = GetComponentInParent<PlayerStats>();
        _inputHandler = GetComponentInParent<InputHandler>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach(WeaponHolderSlot weaponslot in weaponHolderSlots)
        {
            if(weaponslot._isLeftHandSlot)
            {
                _leftHandSlot = weaponslot;
            }
            else if(weaponslot._isRightHandSlot)
            {
                _rightHandSlot = weaponslot;
            }
            else if(weaponslot._isBackSlot)
            {
                _backSlot = weaponslot;
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        // 무기 로드
        if (isLeft)
        {
            _leftHandSlot.currentWeapon = weaponItem;
            _leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            _quickSlotsUI.UpdateWeaponQuickSlotsUI(true,weaponItem);

            // 현재 무기에 따라 왼손 Idle 애니메이션 변경
            #region Handle Left Weapon Idle Animations
            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
            }
            else animator.CrossFade("Left Arm Empty", 0.2f);
            #endregion
        }
        else
        {
            if(_inputHandler._twoHandFlag)
            {
                _backSlot.LoadWeaponModel(_leftHandSlot.currentWeapon);
                _leftHandSlot.UnloadWeaponAndDestroy();
                animator.CrossFade(weaponItem.th_idle, 0.2f);
            }
            else
            {
                #region Handle Right Weapon Idle Animations

                animator.CrossFade("Both Arms Empty", 0.2f);
                // 현재 무기에 따라 오른손 Idle 애니메이션 변경
                if (_backSlot != null)
                {
                    _backSlot.UnloadWeaponAndDestroy();
                }

                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                }
                else animator.CrossFade("Right Arm Empty", 0.2f);
                #endregion
            }
            _rightHandSlot.currentWeapon = weaponItem;
            _rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            _quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
        }

        // 슬롯 UI 로드
        _quickSlotsUI.UpdateWeaponQuickSlotsUI(isLeft, weaponItem);
    }

    #region 데미지 콜라이더
    private void LoadLeftWeaponDamageCollider()
    {
        if (_leftHandSlot._currentWeaponModel == null)
            return;

        _leftHandDamageCollider = _leftHandSlot._currentWeaponModel.GetComponentInChildren<DamageCollider>();
        _leftHandDamageCollider._currentWeaponDamage = _playerInventory._leftWeapon.baseDamage;

    }

    private void LoadRightWeaponDamageCollider()
    {
        if (_rightHandSlot._currentWeaponModel == null)
            return;

        _rightHandDamageCollider = _rightHandSlot._currentWeaponModel.GetComponentInChildren<DamageCollider>();
        _rightHandDamageCollider._currentWeaponDamage = _playerInventory._rightWeapon.baseDamage;
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
