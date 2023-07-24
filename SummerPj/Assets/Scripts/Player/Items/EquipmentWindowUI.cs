using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    public List<HandEquipmentSlotUI> _rightHandEquipmentSlotUI = new List<HandEquipmentSlotUI>();
    public List<HandEquipmentSlotUI> _leftHandEquipmentSlotUI = new List<HandEquipmentSlotUI>();

    public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory) 
    { 
        for (int i = 0; i < _rightHandEquipmentSlotUI.Count; i++)
        {
            if (playerInventory._weaponsInRightHandSlots.Count > i)
                _rightHandEquipmentSlotUI[i].AddItem(playerInventory._weaponsInRightHandSlots[i]);
        }

        for (int i = 0; i < _leftHandEquipmentSlotUI.Count; ++i)
        {
            if (playerInventory._weaponsInLeftHandSlots.Count > i)
                _leftHandEquipmentSlotUI[i].AddItem(playerInventory._weaponsInLeftHandSlots[i]);
        }
    }
}
