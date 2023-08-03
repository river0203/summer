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
            _playerInventory._weaponsInventory.Add(_playerInventory._weaponSlots[0]);
            _playerInventory._weaponSlots[0] = item;
            _playerInventory._currentWeaponIndex = 0;
            _playerInventory._weaponsInventory.Remove(item);
        }
        else if(_uiManager.rightHandSlot02Selected)
        {
            _playerInventory._weaponsInventory.Add(_playerInventory._weaponSlots[1]);
            _playerInventory._weaponSlots[1] = item;
            _playerInventory._currentWeaponIndex = 1;
            _playerInventory._weaponsInventory.Remove(item);
        }
        else
        {
            return;
        }

        if (_playerInventory._currentWeaponIndex != -1)
        {
            _playerInventory._currentWeapon = _playerInventory._weaponSlots[_playerInventory._currentWeaponIndex];
            _weaponSlotManager.LoadWeaponOnSlot(_playerInventory._currentWeapon);
        }
        
        _uiManager._equipmentWindowUI.LoadWeaponsOnEquipmentScreen(_playerInventory);
        _uiManager.ResetAllSelectedSlots();
    }
}
