using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterEffectsManager : MonoBehaviour
{
    [Header("Damage FX")]
    public VisualEffectAsset _bloodSplatterFX;

    [Header("Weapon FX")]
    public WeaponFX _weaponFX;
    public virtual void PlayWeaponFX()
    {
        if (_weaponFX != null)
        {
            _weaponFX.PlayWeaponFX();
        }
    }

    public virtual void StopWeaponFX()
    {
        if( _weaponFX != null )
        {
            _weaponFX.StopWeaponFX();
        }
    }

    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        VisualEffectAsset blood = Instantiate(_bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }
}
