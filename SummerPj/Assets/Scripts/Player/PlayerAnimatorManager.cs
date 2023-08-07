using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animator의 파라미터 변수를 변경시켜줌 
public class PlayerAnimatorManager : AnimatorManager
{
    InputHandler _inputHandler;
    PlayerLocomotionManager _playerLocomotionManager;
    int _vertical;
    int _horizontal;

    protected override void Awake()
    {
        base.Awake();
        _anim = GetComponent<Animator>();
        _inputHandler = GetComponent<InputHandler>();
        _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
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


    private void OnAnimatorMove()
    {
        if (_characterManager._isInteracting == false)
            return;

        float delta = Time.deltaTime;
        _playerLocomotionManager._rigid.drag = 0;
        Vector3 deltaPosition = _anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        _playerLocomotionManager._rigid.velocity = velocity;
    }
}
