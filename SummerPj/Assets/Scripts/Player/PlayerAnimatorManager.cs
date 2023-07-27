using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animator의 파라미터 변수를 변경시켜줌 
public class PlayerAnimatorManager : AnimatorManager
{
    PlayerManager _playerManager;
    PlayerStatsManager _playerStatsManager;
    InputHandler _inputHandler;
    PlayerLocomotionManager _playerLocomotionManager;
    int _vertical;
    int _horizontal;

    public void Init()
    {
        _playerStatsManager = GetComponentInParent<PlayerStatsManager>();
        _playerManager = GetComponentInParent<PlayerManager>();
        _anim = GetComponentInChildren<Animator>();
        _inputHandler = GetComponentInParent<InputHandler>();
        _playerLocomotionManager = GetComponentInParent<PlayerLocomotionManager>();
        _vertical = Animator.StringToHash("Vertical");
        _horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        float v = 0;

        if (verticalMovement > 0 && verticalMovement < 0.55f)
            v = 0.5f;
        else if (verticalMovement > 0.55f)
            v = 1;
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
            v = -0.5f;
        else if (verticalMovement < -0.55f)
            v = -1;
        else
            v = 0;
        #endregion

        #region Horizontal
        float h = 0;

        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            h = 0.5f;
        else if (horizontalMovement > 0.55f)
            h = 1;
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            h = -0.5f;
        else if (horizontalMovement < -0.55f)
            h = -1;
        else
            h = 0;
        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        _anim.SetFloat(_vertical, v, 0.1f, Time.deltaTime);
        _anim.SetFloat(_horizontal, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate() 
    {
        _anim.SetBool("canRotate", true);
    }
    public void StopRotate() 
    {
        _anim.SetBool("canRotate", false);
    }

    public void EnableCombo()
    {
        _anim.SetBool("canDoCombo", true);
    }
    public void Disablecombo()
    {
        _anim.SetBool("canDoCombo", false);
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        _playerStatsManager.TakeDamageNoAnimation(_playerManager.pendingCriticalDamage);
        _playerManager.pendingCriticalDamage = 0;
    }
    public void EnableParrying()
    {
        _playerManager.isParrying = true;
    }
    public void DisableParrying()
    {
        _playerManager.isParrying = false;
    }
    public void EnableCanBeRiposted()
    {
        _playerManager.canBeRiposted = true;
    }
    public void DisableCanBeRiposted()
    {
        _playerManager.canBeRiposted = false;
    }
    public void EnableIsInvulerable()
    {
        _anim.SetBool("isInvulnerable", true);
    }
    public void DisableIsvulerable()
    {
        _anim.SetBool("isInvulnerable", false);
    }

    private void OnAnimatorMove()
    {
        if (_playerManager._isInteracting == false)
            return;

        float delta = Time.deltaTime;
        _playerLocomotionManager._rigid.drag = 0;
        Vector3 deltaPosition = _anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        _playerLocomotionManager._rigid.velocity = velocity;
    }
}
