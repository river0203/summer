using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlot : MonoBehaviour
{
    PlayerInventoryManager _playerInventory;
    PlayerWeaponSlotManager _weaponSlotManager;
    UIManager _uiManager;

    public Image icon;
    WeaponItem item;

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerInventoryManager>();
        _weaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(WeaponItem newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void EquipThisItem()
    {
        if(_uiManager.rightHandSlot01Selected)
        {
            _playerInventory._weaponsInventory.Add(_playerInventory._weaponsInRightHandSlots[0]);
            _playerInventory._weaponsInRightHandSlots[0] = item;
            _playerInventory._currentRightWeaponIndex = 0;
            _playerInventory._weaponsInventory.Remove(item);
        }
        else if(_uiManager.rightHandSlot02Selected)
        {
            _playerInventory._weaponsInventory.Add(_playerInventory._weaponsInRightHandSlots[1]);
            _playerInventory._weaponsInRightHandSlots[1] = item;
            _playerInventory._currentRightWeaponIndex = 1;
            _playerInventory._weaponsInventory.Remove(item);
        }
        else if (_uiManager.leftHandSlot01Selected)
        {
            _playerInventory._weaponsInventory.Add(_playerInventory._weaponsInLeftHandSlots[0]);
            _playerInventory._weaponsInLeftHandSlots[0] = item;
            _playerInventory._currentLeftWeaponIndex = 0;
            _playerInventory._weaponsInventory.Remove(item);
        }
        else if (_uiManager.leftHandSlot02Selected)
        {
            _playerInventory._weaponsInventory.Add(_playerInventory._weaponsInLeftHandSlots[1]);
            _playerInventory._weaponsInLeftHandSlots[1] = item;
            _playerInventory._currentLeftWeaponIndex = 1;
            _playerInventory._weaponsInventory.Remove(item);
        }
        else
        {
            return;
        }

        if (_playerInventory._currentRightWeaponIndex != -1)
        {
            _playerInventory._rightWeapon = _playerInventory._weaponsInRightHandSlots[_playerInventory._currentRightWeaponIndex];
            _weaponSlotManager.LoadWeaponOnSlot(_playerInventory._rightWeapon, false);
        }
        if (_playerInventory._currentLeftWeaponIndex != -1)
        {
            _playerInventory._leftWeapon = _playerInventory._weaponsInLeftHandSlots[_playerInventory._currentLeftWeaponIndex];
            _weaponSlotManager.LoadWeaponOnSlot(_playerInventory._leftWeapon, true);
        }
        
        _uiManager._equipmentWindowUI.LoadWeaponsOnEquipmentScreen(_playerInventory);
        _uiManager.ResetAllSelectedSlots();
    }
}
