using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    WeaponHolderSlot rightHandSlot;
    WeaponHolderSlot leftHandSlot;
    EnemyEffactManager _enemyEffactManager;

    DamageCollider leftHandDamageCollider;
    public DamageCollider rightHandDamageCollider;

    Animator _anim;

    private void Awake()
    {
        _enemyEffactManager = GetComponent<EnemyEffactManager>();
        rightHandDamageCollider = GetComponentInChildren<DamageCollider>(true);
    }

    private void Start()
    {   
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
        if (isleft)
        {
            leftHandDamageCollider = leftHandSlot._currentWeaponModel.GetComponentInChildren<DamageCollider>(true);
            leftHandDamageCollider._characterManager = GetComponentInParent<CharacterManager>();
            _enemyEffactManager._leftWeaponFX = leftHandSlot._currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        else
        {
            rightHandDamageCollider = rightHandSlot._currentWeaponModel.GetComponentInChildren<DamageCollider>(true);
            rightHandDamageCollider._characterManager = GetComponentInParent<CharacterManager>();
            _enemyEffactManager._rightWeaponFX = rightHandSlot._currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
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
