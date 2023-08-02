using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInventoryManager : MonoBehaviour
{
    PlayerWeaponSlotManager _playerWeaponSlotManager;

    Animator _animator;
    public ConsumableItem currentConsumable;
    public SpellItem _currentSpell;
    public WeaponItem _rightWeapon;
    [HideInInspector] public WeaponItem _leftWeapon;

    public int _maxInventorySlotCount = 4;
    public List<WeaponItem> _weaponsInRightHandSlots = new List<WeaponItem>();
    public List<WeaponItem> _weaponsInLeftHandSlots = new List<WeaponItem>();
    public List<SpellItem> _Spells = new List<SpellItem>();

    public int _currentRightWeaponIndex = -1;
    [HideInInspector] public int _currentLeftWeaponIndex = -1;

    public List<WeaponItem> _weaponsInventory;

    private void Awake()
    {
        _playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _rightWeapon = _weaponsInRightHandSlots[0];
        _leftWeapon = _weaponsInLeftHandSlots[0];
        _currentRightWeaponIndex = 0;
        _currentLeftWeaponIndex = 0;
        _playerWeaponSlotManager.LoadWeaponOnSlot(_rightWeapon, false);
        _playerWeaponSlotManager.LoadWeaponOnSlot(_leftWeapon, true);
    }

    public void ChangeRightWeapon()
    {
        if (_currentRightWeaponIndex < _maxInventorySlotCount - 1 && _currentRightWeaponIndex < _weaponsInRightHandSlots.Count - 1)
        {
            _currentRightWeaponIndex++;

            if (_weaponsInRightHandSlots[_currentRightWeaponIndex] != null)
            {
                _rightWeapon = _weaponsInRightHandSlots[_currentRightWeaponIndex];
                _playerWeaponSlotManager.LoadWeaponOnSlot(_weaponsInRightHandSlots[_currentRightWeaponIndex], false);
            }
        }
        else
        {
            _currentRightWeaponIndex = -1;
            _rightWeapon = _playerWeaponSlotManager._unarmedWeapon;
            _playerWeaponSlotManager.LoadWeaponOnSlot(_playerWeaponSlotManager._unarmedWeapon, false);
        }

        #region Change Spell
        if (_rightWeapon.itemName == "Sword") { _currentSpell = _Spells[0]; }
        else if (_rightWeapon.itemName == "Halberd") { _currentSpell = _Spells[1]; }
        else if (_rightWeapon.itemName == "Rapier") { _currentSpell = _Spells[2]; }
        #endregion
    }

    public void ChangeLeftWeapon()
    {
        if (_currentLeftWeaponIndex < _maxInventorySlotCount - 1 && _currentLeftWeaponIndex < _weaponsInLeftHandSlots.Count - 1)
        {
            _currentLeftWeaponIndex++;

            if (_weaponsInLeftHandSlots[_currentLeftWeaponIndex] != null)
            {
                _leftWeapon = _weaponsInLeftHandSlots[_currentLeftWeaponIndex];
                _playerWeaponSlotManager.LoadWeaponOnSlot(_weaponsInLeftHandSlots[_currentLeftWeaponIndex], true);
            }
        }
        else
        {
            _currentLeftWeaponIndex = -1;
            _leftWeapon = _playerWeaponSlotManager._unarmedWeapon;
            _playerWeaponSlotManager.LoadWeaponOnSlot(_playerWeaponSlotManager._unarmedWeapon, true);
        }
    }
}
