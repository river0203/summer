using System.Collections;
using System.Collections.Generic;
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

        // ��ũ��Ʈ ���� �ذ�
        _isInteracting = _anim.GetBool("isInteracting");

        _canDoCombo = _anim.GetBool("canDoCombo");
        
        // �÷��̾� �̵�
        _isSprinting = _inputHandler.b_input;
        _inputHandler.TickInput(delta);
        _playerLocomotion.HandleMovement(delta);
        _playerLocomotion.HandleRollingAndSprinting(delta);
        _playerLocomotion.HandleFalling(delta, _playerLocomotion._moveDirection);
    }

    private void FixedUpdate()
    {
        // ī�޶� �̵�
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

        if (_isInAir)
        {
            _playerLocomotion._inAirTimer += Time.deltaTime;
        }
    }
}
