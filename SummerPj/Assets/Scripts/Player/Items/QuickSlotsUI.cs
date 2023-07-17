using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    public Image _leftWeaponIcon;
    public Image _rightWeaponIcon;

    public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
    {
        if (isLeft == false)
        {
            if (weapon.itemIcon != null)
            {
                _rightWeaponIcon.sprite = weapon.itemIcon;
                _rightWeaponIcon.enabled = true;
            }
            else
            {
                _rightWeaponIcon.sprite = null;
                _rightWeaponIcon.enabled = false;
            }
        }
        else
        {
            if (weapon.itemIcon != null)
            {
                _leftWeaponIcon.sprite = weapon.itemIcon;
                _leftWeaponIcon.enabled = true;
            }
            else
            {
                _leftWeaponIcon.sprite = null;
                _leftWeaponIcon.enabled = false;
            }
        }
    }
}
