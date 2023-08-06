using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInventoryManager : MonoBehaviour
{
    PlayerWeaponSlotManager _playerWeaponSlotManager;
    Animator _anim;

    public ConsumableItem currentConsumable;
    public SpellItem _currentSpell;
    public WeaponItem _currentWeapon;

    public int _maxInventorySlotCount = 3;
    public List<WeaponItem> _weaponSlots = new List<WeaponItem>();

    public int _currentWeaponIndex = 0; 

    AnimatorState _animState;
    public List<WeaponItem> _weaponsInventory;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }

    private void Start()
    {
        _currentWeapon = _weaponSlots[0];
        _currentSpell = _currentWeapon._skill;
        _currentWeaponIndex = 0;
        _playerWeaponSlotManager.LoadWeaponOnSlot(_currentWeapon);
    }
    public void ChangeWeapon()
    {
        if (_currentWeaponIndex < _maxInventorySlotCount - 1 && _currentWeaponIndex < _weaponSlots.Count - 1)
        {
            _currentWeaponIndex++;

            if (_weaponSlots[_currentWeaponIndex] != null)
            {
                _currentWeapon = _weaponSlots[_currentWeaponIndex];
                _currentSpell = _currentWeapon._skill;
                _playerWeaponSlotManager.LoadWeaponOnSlot(_weaponSlots[_currentWeaponIndex]);
                
                _anim.CrossFade(_currentWeapon._Locomotion, 0.2f);
            }
        }
        else
        {
            _currentWeaponIndex = 0;

            _currentWeapon = _weaponSlots[_currentWeaponIndex];
            _currentSpell = _currentWeapon._skill;
            _playerWeaponSlotManager.LoadWeaponOnSlot(_weaponSlots[_currentWeaponIndex]);
            _anim.CrossFade(_currentWeapon._Locomotion, 0.2f);
        }
    }
}
