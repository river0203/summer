using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerInventory playerInventory;
    EquipmentWindowUI _equipmentWindowUI;

    [Header("UI Windows")]
    public GameObject hudWindow;
    public GameObject selectWindow;
    public GameObject weaponInventoryWindow;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;

    private void Awake()
    {
        _equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
    }

    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        _equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
    }

    public void UpdateUI()
    {
        #region Weapon Inventory Slots
        for(int i = 0; i < weaponInventorySlots.Length; i++) 
        { 
            if( i< playerInventory._weaponInventory.Count )
            {
                if(weaponInventorySlots.Length < playerInventory._weaponInventory.Count )
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }
                weaponInventorySlots[i].AddItem(playerInventory._weaponInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventorySlot();
            }
        }
        #endregion 
    }
    public void OpenSelectWindow()
    {
        selectWindow.SetActive(true);
    }

    public void CloseSelectWindow()
    {
        selectWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
        weaponInventoryWindow.SetActive(false);
    }
}
