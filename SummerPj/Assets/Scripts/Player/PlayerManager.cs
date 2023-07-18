using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    InputHandler _inputHandler;
    Animator _anim;
    CameraHendler _cameraHandler;
    PlayerLocomotion _playerLocomotion;

    public bool _isInteracting;

    [Header("PlayerFlag")]
    public bool _isSprinting;
    public bool _isInAir;
    public bool _isGrounded;
    public bool _canDoCombo;

    private void Awake()
    {
        _cameraHandler = FindObjectOfType<CameraHendler>();
    }

    void Start()
    {
        _inputHandler = GetComponent<InputHandler>();
        _anim = GetComponentInChildren<Animator>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        // 스크립트 꼬임 해결
        _isInteracting = _anim.GetBool("isInteracting");

        _canDoCombo = _anim.GetBool("canDoCombo");
        
        // 플레이어 이동
        _isSprinting = _inputHandler.b_input;
        _inputHandler.TickInput(delta);
        _playerLocomotion.HandleMovement(delta);
        _playerLocomotion.HandleRollingAndSprinting(delta);
        _playerLocomotion.HandleFalling(delta, _playerLocomotion._moveDirection);
        CheckForInteractableObject();
    }

    private void FixedUpdate()
    {
        // 카메라 이동
        float delta = Time.deltaTime;

        if (_cameraHandler != null)
        {
            _cameraHandler.FollowTarget(delta);
            _cameraHandler.HandlerCameraRotation(delta, _inputHandler._mouseX, _inputHandler._mouseY);
        }
    }

    private void LateUpdate()
    {
        _inputHandler._dodgeFlag = false;
        _inputHandler._sprintFlag = false;
        _inputHandler.la_input = false;
        _inputHandler.ha_input = false;
        _inputHandler.d_Pad_Up = false;
        _inputHandler.d_Pad_Down = false;
        _inputHandler.d_Pad_Right = false;
        _inputHandler.d_Pad_Left = false;
        _inputHandler.a_input = false;

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
                Debug.Log("is Interact2");
                Interactable interactableObj = hit.collider.GetComponent<Interactable>();

                if (interactableObj != null)
                {
                    string interactableText = interactableObj._interactableText;

                    if (_inputHandler.a_input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);   
                    }
                }
            }
        }
    }
}
