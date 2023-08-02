using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    public List<HandEquipmentSlotUI> _rightHandEquipmentSlotUI = new List<HandEquipmentSlotUI>();
    public List<HandEquipmentSlotUI> _leftHandEquipmentSlotUI = new List<HandEquipmentSlotUI>();

    public void LoadWeaponsOnEquipmentScreen(PlayerInventoryManager playerInventory) 
    { 
        for (int i = 0; i < _rightHandEquipmentSlotUI.Count; i++)
        {
            if (playerInventory._weaponSlots.Count > i)
                _rightHandEquipmentSlotUI[i].AddItem(playerInventory._weaponSlots[i]);
        }
    }
}
