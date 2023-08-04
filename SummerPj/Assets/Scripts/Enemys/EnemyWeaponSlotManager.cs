using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    public WeaponItem currentHandWeapon;

    WeaponHolderSlot currentWeaponSlot;
    EnemyEffactManager _enemyEffactManager;

    public DamageCollider bossWeaponCollider;

    Animator _anim;

    private void Awake()
    {
        _enemyEffactManager = GetComponent<EnemyEffactManager>();
        bossWeaponCollider = GetComponentInChildren<DamageCollider>(true);
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        LoadWeaponsOnBothHands();
    }

    public void LoadWeaponOnSlot(WeaponItem weapon, bool _isWeapon)
    {
        if(_isWeapon) 
        {
            currentWeaponSlot._currentWeapon = weapon;
            currentWeaponSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(false);
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        if (currentHandWeapon != null)
        {
            LoadWeaponOnSlot(currentHandWeapon, false);
        }
    }

    public void LoadWeaponsDamageCollider(bool _isWeapon)
    {
        if (_isWeapon)
        {
            bossWeaponCollider = currentWeaponSlot._currentWeaponModel.GetComponentInChildren<DamageCollider>(true);
            bossWeaponCollider._characterManager = GetComponentInParent<CharacterManager>();
            //_enemyEffactManager._rightWeaponFX = currentWeaponSlot._currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
    }   

    public void OpenDamageCollider()
    {
        bossWeaponCollider.EnableDamagecollider();
    }
    public void CloseDamageCollider()
    {
        bossWeaponCollider.DisableDamagecollider();
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
