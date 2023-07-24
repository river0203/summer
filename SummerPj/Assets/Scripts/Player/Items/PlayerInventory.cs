using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager _weaponSlotManager;

    public SpellItem _currentSpell;
    public WeaponItem _rightWeapon;
    public WeaponItem _leftWeapon;

    public WeaponItem _unarmedWeapon;

    public int _maxInventorySlotCount = 4;
    public List<WeaponItem> _weaponsInRightHandSlots = new List<WeaponItem>();
    public List<WeaponItem> _weaponsInLeftHandSlots = new List<WeaponItem>();

    public int _currentRightWeaponIndex = -1;
    public int _currentLeftWeaponIndex = -1;

    public List<WeaponItem> _weaponsInventory;

    private void Awake()
    {
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        while (_weaponsInRightHandSlots.Count < _maxInventorySlotCount)
        {
            _weaponsInRightHandSlots.Add(new WeaponItem());
        }
        while (_weaponsInLeftHandSlots.Count < _maxInventorySlotCount)
        {
            _weaponsInLeftHandSlots.Add(new WeaponItem());
        }

        _rightWeapon = _weaponsInRightHandSlots[0];
        _leftWeapon = _weaponsInLeftHandSlots[0];
        _weaponSlotManager.LoadWeaponOnSlot(_rightWeapon, false);
        _weaponSlotManager.LoadWeaponOnSlot(_leftWeapon, true);
    }

    public void ChangeRightWeapon()
    {
        if (_currentRightWeaponIndex < _maxInventorySlotCount - 1 && _currentRightWeaponIndex < _weaponsInRightHandSlots.Count - 1)
        {
            _currentRightWeaponIndex++;

            if (_weaponsInRightHandSlots[_currentRightWeaponIndex] != null)
            {
                _rightWeapon = _weaponsInRightHandSlots[_currentRightWeaponIndex];
                _weaponSlotManager.LoadWeaponOnSlot(_weaponsInRightHandSlots[_currentRightWeaponIndex], false);
            }
        }
        else
        {
            _currentRightWeaponIndex = -1;
            _rightWeapon = _unarmedWeapon;
            _weaponSlotManager.LoadWeaponOnSlot(_unarmedWeapon, false);
        }
    }

    public void ChangeLeftWeapon()
    {
        if (_currentLeftWeaponIndex < _maxInventorySlotCount - 1 && _currentLeftWeaponIndex < _weaponsInLeftHandSlots.Count - 1)
        {
            _currentLeftWeaponIndex++;

            if (_weaponsInLeftHandSlots[_currentLeftWeaponIndex] != null)
            {
                _leftWeapon = _weaponsInLeftHandSlots[_currentLeftWeaponIndex];
                _weaponSlotManager.LoadWeaponOnSlot(_weaponsInLeftHandSlots[_currentLeftWeaponIndex], true);
            }
        }
        else
        {
            _currentLeftWeaponIndex = -1;
            _leftWeapon = _unarmedWeapon;
            _weaponSlotManager.LoadWeaponOnSlot(_unarmedWeapon, true);
        }
    }
}
