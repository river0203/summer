using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ConsumableItem/Flask")]
public class FlaskItem : ConsumableItem 
{
    [Header("Flask Type")]
    public bool estusFlask;
    public bool ashenFlask;

    [Header("Recovery Amount")]
    public int healthRecoverAmount;
    public int focusPointsRecoverAmount;

    [Header("Recovery FX")]
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerAnimatorManager _playerAnimatorManager, PlayerWeaponSlotManager _weaponSlotManager, PlayerEffectsManager _playerEffectsManager)
    {
        base.AttemptToConsumeItem(_playerAnimatorManager, _weaponSlotManager, _playerEffectsManager);
        
        GameObject flask = Instantiate(itemModel, _weaponSlotManager._WeaponSlot.transform);
        _playerEffectsManager.currentParticleFX = recoveryFX;
        _playerEffectsManager.amountToBeHealed = healthRecoverAmount;
        _playerEffectsManager.instantiatedFXModel = flask;
        _weaponSlotManager._WeaponSlot.UnloadWeapon();
    }
}
