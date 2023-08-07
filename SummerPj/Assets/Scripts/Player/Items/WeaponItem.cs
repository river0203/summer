using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Wepon Item")]
public class WeaponItem : Item
{
    public GameObject _modelPrefabe;
    public bool _isUnarmed;

    [Header("Damage")]
    public int baseDamage = 25;
    public int criticalDamageMultiplier = 4;

    [Header("Idle Animations")]
    public string _Locomotion;

    [Header("Absorption")]
    public float physicalDamageAbsorption;

    [Header("One Handed Attack Animations")]
    public string OH_Light_Attack_1;
    public string OH_Light_Attack_2;
    public string OH_Light_Attack_3;
    public string OH_Light_Attack_4;
    public string OH_Heavy_Attack_1;

    [Header("Two Handed Attack Animations")]
    public string TH_Light_Attack_01;
    public string TH_Light_Attack_02;

    [Header("Running Attack Animations")]
    public string Running_Attack_01;

    [Header("Weapon Art")]
    public string weapon_art;

    [Header("Stamina Costs")]
    public int baseStaminar;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Weapon Type")]
    public bool isMeleeWeapon;
    public bool isShieldWeapon;

    [Header("Sound FX")]
    public AudioClip[] weaponWhooshes;

    [Header("Skill")]
    public SpellItem _skill;
}