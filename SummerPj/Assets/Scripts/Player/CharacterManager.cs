using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Lock On Transform")]
    public Transform _lockOnTransform;
    
    [Header("Combat Colliders")]
    public BoxCollider _backStabBoxCollider;
    public CriticalDamageCollider _backStabCollider;
    public CriticalDamageCollider _riposteCollider;

    [Header("Combat Flags")]
    public bool canBeRiposted;
    public bool canBeParried;
    public bool isParrying;
    public bool isBlocking;

    [Header("Spells")]
    public bool isFiringSpell;

    public int pendingCriticalDamage;
}
