using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : CharacterEffectsManager
{
    [Header("Weapon FX")]
    public ParticleSystem _normalWeaponTrail;

    public void PlayWeaponFX()
    {
        _normalWeaponTrail.Play();
    }
    public void StopWeaponFX()
    {
        _normalWeaponTrail.Stop();
    }
}
