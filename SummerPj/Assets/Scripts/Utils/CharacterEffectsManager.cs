using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    [Header("Damage FX")]
    public GameObject _bloodSplatterFX;

    [Header("Weapon FX")]
    public WeaponFX _weaponFX;
    public virtual void PlayWeaponFX()
    {
            if (_weaponFX != null)
            {
                _weaponFX.PlayWeaponFX();
            }
    }

    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        GameObject blood = Instantiate(_bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }
}
