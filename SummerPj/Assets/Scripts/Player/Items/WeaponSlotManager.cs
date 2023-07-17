using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot _leftHandSlot;
    WeaponHolderSlot _rightHandSlot;

    DamageCollider _leftHandDamageCollider;
    DamageCollider _rightHandDamageCollider;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>(true);
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot._isLeftHandSlot)
            {
                _leftHandSlot = weaponSlot;
            }
            else if (weaponSlot._isRightHandSlot)
            {
                _rightHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            _leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();

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
            _rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();

            // ���� ���⿡ ���� ������ Idle �ִϸ��̼� ����
            #region Handle Right Weapon Idle Animations
            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
            }
            else animator.CrossFade("Right Arm Empty", 0.2f);
            #endregion
        }
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

    public void OpenRightDamageCollier()
    {
        _rightHandDamageCollider.EnableDamagecollider();
    }

    public void CloseLeftDamageCollier()
    {
        _leftHandDamageCollider.DisableDamagecollider();
    }

    public void CloseRightDamageCollier()
    {
        _rightHandDamageCollider.DisableDamagecollider();
    }

    #endregion
}
