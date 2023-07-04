using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : MonoBehaviour
{
    #region  Action
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool dodge;
    public bool attack;
    public bool parry;
    public bool LockOn;
    #endregion 

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
    public void OnAttack(InputValue value)
    {
        AttackInput(value.isPressed);
    }
    public void OnParry(InputValue value)
    {
        ParryInput(value.isPressed);
    }
    public void OnLockOn(InputValue value)
    {
        LockOnInput(value.isPressed);
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
    public void AttackInput(bool newAttackState)
    {
        attack = newAttackState;
    }
    public void ParryInput(bool newParryState)
    {
        parry = newParryState;
    }
    public void LockOnInput(bool newLockOnState)
    {
        LockOn = newLockOnState;
    }
}
