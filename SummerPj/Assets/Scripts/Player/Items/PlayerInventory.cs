using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager _weaponSlotManager;

    public WeaponItem _rightWeapon;
    public WeaponItem _leftWeapon;

    private void Awake()
    {
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        _weaponSlotManager.LoadWeponOnSlot(_rightWeapon, false);
        _weaponSlotManager.LoadWeponOnSlot(_leftWeapon, true);
    }
}
