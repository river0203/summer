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

    private void Awake()
    {
        _cameraHandler = CameraHendler._instance;
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
        
        // 플레이어 이동
        _isSprinting = _inputHandler.b_input;
        _inputHandler.TickInput(delta);
        _playerLocomotion.HandleMovement(delta);
        _playerLocomotion.HandleRollingAndSprinting(delta);
        _playerLocomotion.HandleFalling(delta, _playerLocomotion._moveDirection);
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

        if (_isInAir)
        {
            _playerLocomotion._inAirTimer += Time.deltaTime;
        }
    }
}
