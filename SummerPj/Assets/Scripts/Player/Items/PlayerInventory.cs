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

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;
    private void Awake()
    {
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        currentRightWeaponIndex = -1;
        _rightWeapon = unarmedWeapon;
        //_leftWeapon = unarmedWeapon;
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
        {
            _rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            _weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
        }
        else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }
        else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
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
}
