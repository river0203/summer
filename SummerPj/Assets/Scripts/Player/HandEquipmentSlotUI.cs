using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandEquipmentSlotUI : MonoBehaviour
{
    UIManager _uiManager;
    public Image icon;
    WeaponItem weapon;

    public bool rightHandSlot01;
    public bool rightHandSlot02;
    public bool leftHandSlot01;
    public bool leftHandSlot02;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(WeaponItem newWeapon)
    {
        weapon = newWeapon;
        icon.sprite = weapon.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearItem()
    {
        weapon = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void SelectThisSlot()
    {
        if(rightHandSlot01)
        {
            _uiManager.rightHandSlot01Selected = true;
        }
        else if(rightHandSlot02) 
        { 
            _uiManager.rightHandSlot02Selected = true;
        }
        else if(leftHandSlot01)
        {
            _uiManager.leftHandSlot01Selected = true;
        }
        else
        {
            _uiManager.leftHandSlot02Selected = true;
        }
    }
}
