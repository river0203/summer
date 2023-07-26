using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile Spell")]
public class projectileSpell : SpellItem
{
    public float baseDamage;
    public float projectileVelocity;
    Rigidbody rigid;

    public override void AttemptToCastSpell(PlayerAnimatorManager _animatorHandler, PlayerStats _playerStats, WeaponSlotManager _weaponSlotManager)
    {
        base.AttemptToCastSpell(_animatorHandler, _playerStats, _weaponSlotManager);
        GameObject instatiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, _weaponSlotManager._rightHandSlot.transform);
        instatiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
        _animatorHandler.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SucessfullyCastSpell(PlayerAnimatorManager _animatorHandler, PlayerStats _playerStats)
    {
        base.SucessfullyCastSpell(_animatorHandler, _playerStats);
    }
}
