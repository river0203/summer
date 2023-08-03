using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(PlayerAnimatorManager _animatorHandler, PlayerStatsManager _playerStats, PlayerWeaponSlotManager _weaponSlotManager)
    {
        base.AttemptToCastSpell(_animatorHandler, _playerStats, _weaponSlotManager);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, _animatorHandler.transform);
        _animatorHandler.PlayTargetAnimation(spellAnimation, true);
        Debug.Log("Attempting to cast spell...");
    }

    public override void SucessfullyCastSpell(PlayerAnimatorManager _animatorHandler, PlayerStatsManager _playerStats, CameraHandler _cameraHandler, PlayerWeaponSlotManager _weaponSlotManager)
    {
        base.SucessfullyCastSpell(_animatorHandler, _playerStats, _cameraHandler, _weaponSlotManager);
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, _animatorHandler.transform);
        _playerStats.HealPlayer(healAmount);
    }
}
