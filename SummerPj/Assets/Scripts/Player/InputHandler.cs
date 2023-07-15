using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 매 프레임 인풋을 받아 '인풋에 따라 변하는 변수들'을 갱신시키고, 그 변수들을 public으로 열어 다른 스크립트에서 그 변수들을 통해 플레이어를 움직이게 해줌
public class InputHandler : MonoBehaviour
{
    #region 인풋에 따라 변하는 변수들
    public float _horizontal;
    public float _vertical;
    public float _moveAmount;
    public float _mouseX;
    public float _mouseY;

    public bool b_input;
    public bool _dodgeFlag;
    public bool _sprintFlag;
    public float _dodgeInputTimer;
    #endregion

    PlayerInputAction _inputActions;
    CameraHendler _cameraHandler;

    Vector2 movementInput;
    Vector2 cameraInput;

    #region '인풋에 따라 변하는 변수'를 변환시켜주는 함수들
    private void Awake()
    {
        _cameraHandler = CameraHendler._instance;
    }

    private void OnEnable()
    {
        // Move와 Look 인풋이 있을때마다 변수를 바꿔주도록 이벤트에 함수 바인딩
        if (_inputActions == null)
        {
            _inputActions = new PlayerInputAction();
            _inputActions.PlayerMovement.Move.performed += _inputActions => { movementInput = _inputActions.ReadValue<Vector2>(); };
            _inputActions.PlayerMovement.Look.performed += i => { cameraInput = i.ReadValue<Vector2>(); } ;
        }

        _inputActions.Enable();
    }

    private void OnDisable() { _inputActions.Disable(); }

    // Locomotion스크립트에서 실행
    public void TickInput(float delta)
    {
        MoveInput(delta); 
        HandleRollInput(delta); 
    }

    // 이동 및 마우스 포지션 갱신 (TickInput에서 실행)
    private void MoveInput(float delta)
    {
        _horizontal = movementInput.x;
        _vertical = movementInput.y;
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
        _mouseX = cameraInput.x;
        _mouseY = cameraInput.y;
    }

    // 구르기, 달리기 상태 갱신 (TickInput에서 실행)
    public void HandleRollInput(float delta)
    {
        b_input = _inputActions.PlayerActions.Dodge.IsPressed();

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
    #endregion
}