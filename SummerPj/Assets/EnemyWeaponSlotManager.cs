using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    WeaponHolderSlot rightHandSlot;
    WeaponHolderSlot leftHandSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    Animator _anim;

    private void Awake()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponslot in weaponHolderSlots)
        {
            if (weaponslot._isLeftHandSlot)
            {
                leftHandSlot = weaponslot;
            }
            else if (weaponslot._isRightHandSlot)
            {
                rightHandSlot = weaponslot;
            }
        }
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        LoadWeaponsOnBothHands();
    }

    public void LoadWeaponOnSlot(WeaponItem weapon, bool isleft)
    {
        if(isleft) 
        { 
            leftHandSlot.currentWeapon = weapon;
            leftHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(true);
        }
        else
        {
            rightHandSlot.currentWeapon = weapon;
            rightHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(false);
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        if (rightHandWeapon != null)
        {
            LoadWeaponOnSlot(rightHandWeapon, false);
        }
        if (leftHandWeapon != null)
        {
            LoadWeaponOnSlot(leftHandWeapon, true);
        }
    }

    public void LoadWeaponsDamageCollider(bool isleft)
    {
        if(isleft)
        {
            leftHandDamageCollider = leftHandSlot._currentWeaponModel.GetComponent<DamageCollider>();
        }
        else rightHandDamageCollider = rightHandSlot._currentWeaponModel.GetComponent<DamageCollider>();
    }

    public void OpenDamageCollider()
    {
        rightHandDamageCollider.EnableDamagecollider();
    }
    public void CloseDamageCollider()
    {
        rightHandDamageCollider.DisableDamagecollider();
    }

    public void DrainStaminaLightAttack()
    {

    }
    public void DrainStaminaHeavyAttack()
    {

    }

    public void EnableCombo()
    {
        //_anim.SetBool("canDoCombo", true);
    }
    public void DisableCombo()
    {
        //_anim.SetBool("canDoCombo", false);
    }
}
