using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : MonoBehaviour
{
    InputAction _inputAction;

    #region  Action
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool dodge;
    public bool weakAttack;
    public bool strongAttack;
    public bool parry;
    public bool ultimate;
    public bool heal;
    public bool lockOn;
    #endregion 

    private void Start()
    {
        _inputAction = GetComponent<InputAction>();
    }
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }
    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
    }
    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }
    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }
    public void OnDodge(InputValue value)
    {
        DodgeInput(value.isPressed);
    }
    public void OnParry(InputValue value)
    {
        ParryInput(value.isPressed);
    }
    public void OnLockOn(InputValue value)
    {
        LockOnInput(value.isPressed);
    }
    public void OnWeakAttack(InputValue value)
    {
        WeakAttackInput(value.isPressed);
    }
    public void OnStrongAttack(InputValue value)
    {
        StrongAttackInput(value.isPressed);
    }
    public void OnUltimate(InputValue value)
    {
        UltimateInput(value.isPressed);
    }
    public void OnHeal(InputValue value)
    {
        HealInput(value.isPressed);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }
    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }
    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }
    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }
    public void DodgeInput(bool newDodgeState)
    {
        dodge = newDodgeState;
    }
    public void ParryInput(bool newParryState)
    {
        parry = newParryState;
    }
    public void LockOnInput(bool newLockOnState)
    {
        lockOn = newLockOnState;
    }
    public void WeakAttackInput(bool newWeakAttackState)
    {
        weakAttack = newWeakAttackState;
    }
    public void StrongAttackInput(bool newStrongAttackState)
    {
        strongAttack = newStrongAttackState;
    }
    public void UltimateInput(bool newUltimateState)
    {
        ultimate = newUltimateState;
    }
    public void HealInput(bool newHealState)
    {
        heal = newHealState;
    }
}
