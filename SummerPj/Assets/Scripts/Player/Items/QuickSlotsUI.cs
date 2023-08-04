using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    public Image _magicIcon;
    public Image _rightWeaponIcon;

    public void UpdateWeaponQuickSlotsUI(WeaponItem weapon, SpellItem spell)
    {
            if (weapon.itemIcon != null)
            {
                _rightWeaponIcon.sprite = weapon.itemIcon;
                _magicIcon.sprite = spell.itemIcon;
                _rightWeaponIcon.enabled = true;
                _magicIcon.enabled = true;
        }
            else
            {
                _rightWeaponIcon.sprite = null;
                _magicIcon.sprite = null;
                _rightWeaponIcon.enabled = false;
                _magicIcon.enabled = false;
            }

    }
}
