using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : CharacterEffectsManager
{
    [Header("Weapon FX")]
    public ParticleSystem _normalWeaponTrail;

    public override void PlayWeaponFX()
    {
        _normalWeaponTrail.Play();
    }
    public override void StopWeaponFX()
    {
        _normalWeaponTrail.Stop();
    }
}
