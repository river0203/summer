using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    AnimatorHandler _animHandler;
    InputHandler _inputHandler;
    public string _lastAttack;

    private void Awake()
    {
        _animHandler = GetComponentInChildren<AnimatorHandler>();
        _inputHandler = GetComponent<InputHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (_inputHandler._comboFlag)
        {
            _animHandler._anim.SetBool("canDoCombo", false);
            if (_lastAttack == weapon.OH_Light_Attack_1)
            {
                _animHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                _lastAttack = weapon.OH_Light_Attack_2;
            }
            else if (_lastAttack == weapon.OH_Light_Attack_2)
            {
                _animHandler.PlayTargetAnimation(weapon.OH_Light_Attack_3, true);
                _lastAttack = weapon.OH_Light_Attack_3;
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        _animHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        _lastAttack = weapon.OH_Light_Attack_1;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        _animHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        _lastAttack = weapon.OH_Heavy_Attack_1;
    }
}
