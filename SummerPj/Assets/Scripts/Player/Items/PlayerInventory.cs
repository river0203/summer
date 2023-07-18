using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager _weaponSlotManager;

    public WeaponItem _rightWeapon;
    public WeaponItem _leftWeapon;

    public WeaponItem _unarmedWeapon;

    public WeaponItem[] _weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] _weaponsInLeftHandSlots = new WeaponItem[1];

    public int _currentRightWeaponIndex;
    //public int _currentLeftWeaponIndex;

    public List<WeaponItem> _weaponInventory;

    private void Awake()
    {
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        _currentRightWeaponIndex = -1;
        // _currentLeftWeaponIndex = -1;

        _rightWeapon = _unarmedWeapon;
        // _leftWeapon = _unarmedWeapon;
    }

    public void ChangeRightWeapon()
    {
        _currentRightWeaponIndex = _currentRightWeaponIndex + 1;

        if (_currentRightWeaponIndex > _weaponsInRightHandSlots.Length - 1)
        {
            _currentRightWeaponIndex = -1;
            _rightWeapon = _unarmedWeapon;
            _weaponSlotManager.LoadWeaponOnSlot(_unarmedWeapon, false);
        }
        else if (_weaponsInRightHandSlots[_currentRightWeaponIndex] != null)
        {
            _rightWeapon = _weaponsInRightHandSlots[_currentRightWeaponIndex];
            _weaponSlotManager.LoadWeaponOnSlot(_weaponsInRightHandSlots[_currentRightWeaponIndex], false);
        }
        else
        {
            _currentRightWeaponIndex = _currentRightWeaponIndex + 1;
        }

        if(_currentRightWeaponIndex > _weaponsInRightHandSlots.Length - 1) 
        {
            _currentRightWeaponIndex = -1;
            _rightWeapon = _unarmedWeapon;
            _weaponSlotManager.LoadWeaponOnSlot(_unarmedWeapon, false);
        }
    }

    #region 왼손 무기 변경
        /*public void ChangeLeftWeapon()
        {
            _currentLeftWeaponIndex = _currentLeftWeaponIndex + 1;

            if (_currentLeftWeaponIndex > _weaponsInLeftHandSlots.Length - 1)
            {
                _currentLeftWeaponIndex = -1;
                _leftWeapon = _unarmedWeapon;
                _weaponSlotManager.LoadWeaponOnSlot(_unarmedWeapon, true);
            }
            else if (_weaponsInLeftHandSlots[_currentLeftWeaponIndex] != null)
            {
                _leftWeapon = _weaponsInLeftHandSlots[_currentLeftWeaponIndex];
                _weaponSlotManager.LoadWeaponOnSlot(_weaponsInLeftHandSlots[_currentLeftWeaponIndex], true);
            }
            else
            {
                _currentLeftWeaponIndex = _currentLeftWeaponIndex + 1;
            }

            if (_currentLeftWeaponIndex > _weaponsInLeftHandSlots.Length - 1)
            {
                _currentLeftWeaponIndex = -1;
                _leftWeapon = _unarmedWeapon;
                _weaponSlotManager.LoadWeaponOnSlot(_unarmedWeapon, true);
            }
        }*/
    #endregion
}
