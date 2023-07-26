using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    PlayerStats _playerStats;
    WeaponSlotManager _weaponSlotManager;
    public GameObject currentParticleFX;
    public int amountToBeHealed;
    public GameObject instantiatedFXModel;

    void Awake()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
        _weaponSlotManager = GetComponent<WeaponSlotManager>();
    }
    public void HealPlayerFromEffect()
    {
        _playerStats.HealPlayer(amountToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, _playerStats.transform);
        Destroy(instantiatedFXModel.gameObject);
        _weaponSlotManager.LoadBothWeaponsOnSlot();
    }
}
