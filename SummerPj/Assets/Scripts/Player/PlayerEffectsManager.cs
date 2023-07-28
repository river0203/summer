using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    PlayerStatsManager _playerStatsManager;
    PlayerWeaponSlotManager _playerWeaponSlotManager;
    public GameObject currentParticleFX;
    public int amountToBeHealed;
    public GameObject instantiatedFXModel;

    void Awake()
    {
        _playerStatsManager = GetComponentInParent<PlayerStatsManager>();
        _playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }
    public void HealPlayerFromEffect()
    {
        _playerStatsManager.HealPlayer(amountToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, _playerStatsManager.transform);
        Destroy(instantiatedFXModel.gameObject);
        _playerWeaponSlotManager.LoadBothWeaponsOnSlot();
    }
}
