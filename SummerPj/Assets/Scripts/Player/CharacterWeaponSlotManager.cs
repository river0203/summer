using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSlotManager : MonoBehaviour
{
    public WeaponItem _unarmedWeapon;

    public WeaponHolderSlot _leftHandSlot;
    public WeaponHolderSlot _rightHandSlot;
    public WeaponHolderSlot _backSlot;

    public DamageCollider _leftHandDamageCollider;
    public DamageCollider _rightHandDamageCollider;
}
