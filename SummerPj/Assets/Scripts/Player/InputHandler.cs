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

    public void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new PlayerInputAction();
            _inputActions.Player.Move.performed += _inputActions => { movementInput = _inputActions.ReadValue<Vector2>(); };
            _inputActions.Player.Look.performed += i => { cameraInput = i.ReadValue<Vector2>(); } ;
        }

        _inputActions.Enable();
    }

    public void OnDisable() { _inputActions.Disable(); }

    public void TickInput(float delta)
    {
        MoveInput(delta);
    }

    public void MoveInput(float delta)
    {
        _horizontal = movementInput.x;
        _vertical = movementInput.y;
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
        _mouseX = cameraInput.x;
        _mouseY = cameraInput.y;
    }
}
