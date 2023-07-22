using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Wepon Item")]
public class WeaponItem : Item
{
    public GameObject _modelPrefabe;
    public bool _isUnarmed;

    [Header("Idle Animations")]
    public string right_hand_idle;
    public string left_hand_idle;
    public string th_idle;

    [Header("One Handed Attack Animations")]
    public string OH_Light_Attack_1;
    public string OH_Light_Attack_2;
    public string OH_Light_Attack_3;
    public string OH_Heavy_Attack_1;

    [Header("Two Handed Attack Animations")]
    public string TH_Light_Attack_01;
    public string TH_Light_Attack_02;

    [Header("Stamina Costs")]
    public int baseStaminar;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Weapon Type")]
    public bool isSpellCaster;
    public bool isFaithCaster;
    public bool isPyroCaster;
    public bool isMeleeWeapon;
}