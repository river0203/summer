using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot _leftHandSlot;
    WeaponHolderSlot _rightHandSlot;

    private void Awake()
    {
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

    public void LoadWeponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            _leftHandSlot.LoadWeaponModel(weaponItem);
        }
        else
        {
            _rightHandSlot.LoadWeaponModel(weaponItem);
        }
    }
}
