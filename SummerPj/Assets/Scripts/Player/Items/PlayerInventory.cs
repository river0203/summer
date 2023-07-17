using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager _weaponSlotManager;

    public WeaponItem _rightWeapon;
    public WeaponItem _leftWeapon;

    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    public int currentRightWeaponIndex;
    // public int currentLeftWeaponIndex;
    private void Awake()
    {
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        currentRightWeaponIndex = -1;
        // currentLeftWeaponIndex = -1;

        _rightWeapon = unarmedWeapon;
        // _leftWeapon = unarmedWeapon;
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = -1;
            _rightWeapon = unarmedWeapon;
            _weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
        }
        else if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
        {
            _rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            _weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
        }
        else
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        if(currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1) 
        {
            currentRightWeaponIndex = -1;
            _rightWeapon = unarmedWeapon;
            _weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
        }
    }

    #region 왼손 무기 변경
        /*public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                _leftWeapon = unarmedWeapon;
                _weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
            }
            else if (weaponsInLeftHandSlots[currentLeftWeaponIndex] != null)
            {
                _leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                _weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                _leftWeapon = unarmedWeapon;
                _weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
            }
        }*/
    #endregion
}
