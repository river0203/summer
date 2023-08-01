using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : CharacterEffectsManager
{
    [Header("Weapon FX")]
    public ParticleSystem _normalWeaponTrail;

    public void PlayWeaponFX()
    {
        _normalWeaponTrail.Stop();

        if(_normalWeaponTrail.isStopped)
        {
            _normalWeaponTrail.Play();
        }
    }
}
