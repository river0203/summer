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

    [Header("Interaction")]
    public bool _isInteracting;

    [Header("Combat Flags")]
    public bool canBeRiposted;
    public bool canBeParried;
    public bool isParrying;
    public bool isBlocking;
    public bool _canDoCombo;
    public bool isInvulerable; 
    public bool isUsingRightHand;
    public bool isUsingLeftHand;

    [Header("Movement Flags")]
    public bool _isSprinting;
    public bool _isInAir;
    public bool _isGrounded;

    [Header("Spells")]
    public bool isFiringSpell;

    public int pendingCriticalDamage;
}
