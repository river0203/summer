using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    [Header("Damage FX")]
    public GameObject _bloodSplatterFX;

    [Header("Weapon FX")]
    public WeaponFX _rightWeaponFX;
    public WeaponFX _leftWeaponFX;
    public virtual void PlayWeaponFX(bool isLeft)
    {
        if(!isLeft)
        {
            if(_rightWeaponFX != null)
            {
                _rightWeaponFX.PlayWeaponFX();
            }
        }
        else
        {
            if (_rightWeaponFX != null)
            {
                _leftWeaponFX.PlayWeaponFX();
            }
        }
    }

    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        GameObject blood = Instantiate(_bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }
}
