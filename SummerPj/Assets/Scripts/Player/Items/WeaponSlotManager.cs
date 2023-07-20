using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    PlayerManager _playerManager;
    public WeaponItem _attackingWeapon;

    WeaponHolderSlot _leftHandSlot;
    WeaponHolderSlot _rightHandSlot;
    WeaponHolderSlot _backSlot;

    DamageCollider _leftHandDamageCollider;
    DamageCollider _rightHandDamageCollider;

    Animator animator;

    QuickSlotsUI _quickSlotsUI;

    PlayerStats _playerStats;
    InputHandler _inputHandler;

    private void Awake()
    {
        _playerManager = GetComponentInParent<PlayerManager>(); 
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
        // ���� �ε�
        if (isLeft)
        {
            _leftHandSlot.currentWeapon = weaponItem;
            _leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            _quickSlotsUI.UpdateWeaponQuickSlotsUI(true,weaponItem);

            // ���� ���⿡ ���� �޼� Idle �ִϸ��̼� ����
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
                // ���� ���⿡ ���� ������ Idle �ִϸ��̼� ����
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

        // ���� UI �ε�
        _quickSlotsUI.UpdateWeaponQuickSlotsUI(isLeft, weaponItem);
    }

    #region ������ �ݶ��̴�

    private void LoadLeftWeaponDamageCollider()
    {
        if (_leftHandSlot._currentWeaponModel == null)
            return;

        _leftHandDamageCollider = _leftHandSlot._currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    private void LoadRightWeaponDamageCollider()
    {
        if (_rightHandSlot._currentWeaponModel == null)
            return;

        _rightHandDamageCollider = _rightHandSlot._currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenLeftDamageCollier()
    {
        _leftHandDamageCollider.EnableDamagecollider();
    }
    #endregion

    #region ���׹̳�

    #endregion
}