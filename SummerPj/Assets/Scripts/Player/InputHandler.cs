using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float _horizontal;
    public float _vertical;
    public float _moveAmount;
    public float _mouseX;
    public float _mouseY;

    public bool b_input;
    public bool _dodgeFlag;
    public bool _sprintFlag;
    public float _dodgeInputTimer;
    public bool _isInteracting;

    PlayerInputAction _inputActions;
    CameraHendler _cameraHandler;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        _cameraHandler = CameraHendler._instance;
    }

    private void FixedUpdate()
    {
        float delta = Time.deltaTime;

        if (_cameraHandler != null )
        {
            _cameraHandler.FollowTarget(delta);
            _cameraHandler.HandlerCameraRotation(delta, _mouseX, _mouseY);
        }
    }

    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new PlayerInputAction();
            _inputActions.PlayerMovement.Move.performed += _inputActions => { movementInput = _inputActions.ReadValue<Vector2>(); };
            _inputActions.PlayerMovement.Look.performed += i => { cameraInput = i.ReadValue<Vector2>(); } ;
        }

        _inputActions.Enable();
    }

    private void OnDisable() { _inputActions.Disable(); }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta); 
    }

    private void MoveInput(float delta)
    {
        _horizontal = movementInput.x;
        _vertical = movementInput.y;
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
        _mouseX = cameraInput.x;
        _mouseY = cameraInput.y;
    }

    public void HandleRollInput(float delta)
    {
        b_input = _inputActions.PlayerActions.Dodge.phase == UnityEngine.InputSystem.InputActionPhase.Started;

        if (b_input)
        {
            _dodgeInputTimer += delta;
            _sprintFlag = true;
        }
        else
        {
            if (_dodgeInputTimer > 0 && _dodgeInputTimer < 0.5f)
            {
                _sprintFlag = false;
                _dodgeFlag = true;
            }

            _dodgeInputTimer = 0;
        }    
    }
}
