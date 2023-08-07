using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    WeaponHolderSlot HandSlot;
    WeaponHolderSlot leftHandSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    Animator _anim;

    private void Awake()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponslot in weaponHolderSlots)
        {
            if (weaponslot._isWeaponHandSlot)
            {
                HandSlot = weaponslot;
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
            leftHandSlot._currentWeapon = weapon;
            leftHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(true);
        }
        else
        {
            HandSlot._currentWeapon = weapon;
            HandSlot.LoadWeaponModel(weapon);
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
            leftHandDamageCollider._characterManager = GetComponentInParent<CharacterManager>();
        }
        else rightHandDamageCollider = HandSlot._currentWeaponModel.GetComponent<DamageCollider>();
        rightHandDamageCollider._characterManager = GetComponentInParent<CharacterManager>();
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
