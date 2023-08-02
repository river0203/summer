using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterSpellCasting : MonoBehaviour
{
    CharacterManager _characterCastingSpell;

    private void Awake()
    {
        _characterCastingSpell = GetComponentInParent<CharacterManager>();
    }

    private void Update()
    {
        if(_characterCastingSpell.isFiringSpell)
        {
            Destroy(gameObject);
        }
    }
}
