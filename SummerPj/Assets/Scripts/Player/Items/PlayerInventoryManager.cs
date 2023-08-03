using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInventoryManager : MonoBehaviour
{
    public PlayerWeaponSlotManager _playerWeaponSlotManager;
    public AnimatorStateMachine _animStateMachine;
    public Animator _anim;

    public ConsumableItem currentConsumable;
    public SpellItem _currentSpell;
    public WeaponItem _currentWeapon;

    public int _maxInventorySlotCount = 4;
    public List<WeaponItem> _weaponSlots = new List<WeaponItem>();
    public List<SpellItem> _Spells = new List<SpellItem>();

    public int _currentWeaponIndex = -1;

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
                _playerWeaponSlotManager.LoadWeaponOnSlot(_weaponSlots[_currentWeaponIndex]);
            }
        }
        else
        {
            _currentWeaponIndex = 0;
            _currentWeapon = _weaponSlots[_currentWeaponIndex];
            _playerWeaponSlotManager.LoadWeaponOnSlot(_weaponSlots[_currentWeaponIndex]);
          /* _currentWeapon = _playerWeaponSlotManager._unarmedWeapon;
         _playerWeaponSlotManager.LoadWeaponOnSlot(_playerWeaponSlotManager._unarmedWeapon);*/
        }

        #region Change Spell & Anim
                if (_currentWeapon.itemName == "Sword") { _currentSpell = _Spells[0]; _anim.CrossFade("Locomotion_Sword", 0.2f); }
                else if (_currentWeapon.itemName == "Halberd") { _currentSpell = _Spells[1]; _anim.CrossFade("Locomotion_Halberd", 0.2f); }
                else if (_currentWeapon.itemName == "Rapier") { _currentSpell = _Spells[2]; _anim.CrossFade("Locomotion_Rapier", 0.2f); }
                else { _currentSpell = null; _anim.Play("Locomotion_Sword"); }
        #endregion
    }
}
