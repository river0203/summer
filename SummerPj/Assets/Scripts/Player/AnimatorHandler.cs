using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animator의 파라미터 변수를 변경시켜줌 
public class AnimatorHandler : MonoBehaviour
{
    PlayerManager _playerManager;
    public Animator _anim;
    InputHandler _inputHandler;
    PlayerLocomotion _playerLocomotion;
    int _vertical;
    int _horizontal;
    public bool canRotate;

    public void Init()
    {
        _playerManager = GetComponentInParent<PlayerManager>();
        _anim = GetComponent<Animator>();
        _inputHandler = GetComponentInParent<InputHandler>();
        _playerLocomotion = GetComponentInParent<PlayerLocomotion>();
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

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        _anim.applyRootMotion = isInteracting;
        _anim.SetBool("isInteracting", isInteracting);
        _anim.CrossFade(targetAnim, 0.2f);
    }

    public void CanRotate() { canRotate = true; }
    public void StopRotate() { canRotate = false; }

    public void EnableCombo()
    {
        _anim.SetBool("canDoCombo", true);
    }

    public void Disablecombo()
    {
        _anim.SetBool("canDoCombo", false);
    }

    private void OnAnimatorMove()
    {
        if (_playerManager._isInteracting == false)
            return;

        float delta = Time.deltaTime;
        _playerLocomotion._rigid.drag = 0;
        Vector3 deltaPosition = _anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        _playerLocomotion._rigid.velocity = velocity;
    }
}
