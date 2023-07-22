using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(AnimatorHandler _animatorHandler, PlayerStats _playerStats)
    {
        base.AttemptToCastSpell(_animatorHandler, _playerStats);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, _animatorHandler.transform);
        _animatorHandler.PlayTargetAnimation(spellAnimation, true);
        Debug.Log("Attempting to cast spell...");
    }

    public override void SucessfullyCastSpell(AnimatorHandler _animatorHandler, PlayerStats _playerStats)
    {
        base.SucessfullyCastSpell(_animatorHandler, _playerStats);
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, _animatorHandler.transform);
        _playerStats.HealPlayer(healAmount);
        Debug.Log("Spell cast successful");

    }
}
