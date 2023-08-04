using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerInventoryManager _playerInventory;
    public EquipmentWindowUI _equipmentWindowUI;

    [Header("Pop Ups")]
    public BonfireLitPopUpUI _bonfireLitPopUpUI;

    [Header("UI Windows")]
    public GameObject _hudWindow;
    public GameObject _selectWindow;
    public GameObject _equipmentScreentWindow;
    public GameObject _weaponInventoryWindow;

    [Header("Equipment Window Slot Selected")]
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] _weaponInventorySlots;

    private void Start()
    {
        _bonfireLitPopUpUI = GetComponentInChildren<BonfireLitPopUpUI>();
        _weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        _equipmentWindowUI.LoadWeaponsOnEquipmentScreen(_playerInventory);
    }

    public void UpdateUI()
    {
        #region Weapon Inventory Slots
        for(int i = 0; i < _weaponInventorySlots.Length; i++) 
        { 
            if (i < _playerInventory._weaponsInventory.Count)
            {
                if(_weaponInventorySlots.Length < _playerInventory._weaponsInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    _weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }
                _weaponInventorySlots[i].AddItem(_playerInventory._weaponsInventory[i]);
            }
            else
            {
                _weaponInventorySlots[i].ClearInventorySlot();
            }
        }
        #endregion 
    }

    public void OpenSelectWindow()
    {
        _selectWindow.SetActive(true);
        
    }

    public void CloseSelectWindow()
    {
        _selectWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
        ResetAllSelectedSlots();
        _weaponInventoryWindow.SetActive(false);
        _equipmentScreentWindow.SetActive(false);
    }

    public void ResetAllSelectedSlots()
    {
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
    }

    public void ActivateBonfirePopUp()
    {
        _bonfireLitPopUpUI.DisplayBonfireLitPopUp();
    }
}
