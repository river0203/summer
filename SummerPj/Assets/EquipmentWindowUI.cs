using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;

    HandEquipmentSlotUI[] handEquipmentSlotUI;

    private void Start()
    {
        handEquipmentSlotUI = GetComponentsInChildren<HandEquipmentSlotUI>();
    }

    public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
    {
        for(int i = 0; i< handEquipmentSlotUI.Length; i++)
        {
            Debug.Log("asdf");
            if (handEquipmentSlotUI[i].rightHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory._weaponsInRightHandSlots[0]);
            }
            else if (handEquipmentSlotUI[i].rightHandSlot02)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory._weaponsInRightHandSlots[1]);
            }
            if (handEquipmentSlotUI[i].leftHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory._weaponsInLeftHandSlots[0]);
            }
            if (handEquipmentSlotUI[i].leftHandSlot02)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory._weaponsInLeftHandSlots[1]);
            }
        }
    }

    public void SelectRightHandSlot01()
    {
        rightHandSlot01Selected = true;
    }

    public void SelectRightHandSlot02()
    {
        rightHandSlot02Selected = true;
    }

    public void SelectLeftHandSlot01()
    {
        leftHandSlot01Selected = true;
    }

    public void SelectLeftHandSlot02()
    {
        leftHandSlot02Selected = true;
    }
}
