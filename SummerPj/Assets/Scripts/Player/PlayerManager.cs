using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : CharacterManager
{
    PlayerStatsManager _playerStatsManager;
    InputHandler _inputHandler;
    Animator _anim;
    CameraHandler _cameraHandler;
    PlayerLocomotionManager _playerLocomotion;
    PlayerAnimatorManager _playerAnimatorManager;
    interactableUI _interactableUI;
    public GameObject interactableUIGameObject;

    private void Awake()
    {
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        _inputHandler = GetComponent<InputHandler>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        _anim = GetComponent<Animator>();
        _playerLocomotion = GetComponent<PlayerLocomotionManager>();
        _interactableUI = FindObjectOfType<interactableUI>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        // 스크립트 꼬임 해결
        _isInteracting = _anim.GetBool("isInteracting");
        _canDoCombo = _anim.GetBool("canDoCombo");
        isUsingRightHand = _anim.GetBool("isUsingRightHand");
        isUsingLeftHand = _anim.GetBool("isUsingLeftHand");
        isInvulerable = _anim.GetBool("isInvulnerable");
        isFiringSpell = _anim.GetBool("isFiringSpell");
        _anim.SetBool("isBlocking", isBlocking);
        _anim.SetBool("isInAir", _isInAir);
        _anim.SetBool("isDead", _playerStatsManager._isDead);

        _inputHandler.TickInput(delta);
        _playerAnimatorManager.canRotate = _anim.GetBool("canRotate");
        _playerLocomotion.HandleJumping();
        _playerLocomotion.HandleRollingAndSprinting(delta);
        _playerStatsManager.RegenerateStamina();

        // 플레이어 이동
        _isSprinting = _inputHandler.b_input;

        CheckForInteractableObject();
        
    }

    private void FixedUpdate()
    {
        // 카메라 이동
        float delta = Time.deltaTime;
        _playerLocomotion.HandleMovement(delta);
        _playerLocomotion.HandleFalling(delta, _playerLocomotion._moveDirection);
        _playerLocomotion.HandleRotation(delta);
    }

    private void LateUpdate()
    {
        _inputHandler._dodgeFlag = false;
        _inputHandler.la_input = false;
        _inputHandler.ha_input = false;
        _inputHandler.lt_Input = false;
        _inputHandler.d_Pad_Up = false;
        _inputHandler.d_Pad_Down = false;
        _inputHandler.d_Pad_Right = false;
        _inputHandler.d_Pad_Left = false;
        _inputHandler.a_input = false;
        _inputHandler.jump_Input = false;
        _inputHandler.inventory_Input = false;

        float delta = Time.deltaTime;
        if (_cameraHandler != null)
        {
            _cameraHandler.FollowTarget(delta);
            _cameraHandler.HandlerCameraRotation(delta, _inputHandler._mouseX, _inputHandler._mouseY);
        }

        if (_isInAir)
        {
            _playerLocomotion._inAirTimer += Time.deltaTime;
        }
    }

    public void CheckForInteractableObject()
    { 
        RaycastHit hit;
        
        Debug.DrawRay(transform.position, transform.forward, Color.yellow);
        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, _cameraHandler._ignoreLayers))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable interactableObj = hit.collider.GetComponent<Interactable>();

                if (interactableObj != null)
                {
                    string interactableText = interactableObj._interactableText;
                    _interactableUI._interactableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);

                    if (_inputHandler.a_input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        else
        {
            if(interactableUIGameObject != null)
            {
                interactableUIGameObject.SetActive(false);
            }
        }
    }
}
