using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ������ ��ǲ�� �޾� '��ǲ�� ���� ���ϴ� ������'�� ���Ž�Ű��, �� �������� public���� ���� �ٸ� ��ũ��Ʈ���� �� �������� ���� �÷��̾ �����̰� ����
public class InputHandler : MonoBehaviour
{
    #region ��ǲ�� ���� ���ϴ� ������
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

    #region '��ǲ�� ���� ���ϴ� ����'�� ��ȯ�����ִ� �Լ���
    private void Awake()
    {
        _cameraHandler = CameraHendler._instance;
    }

    private void OnEnable()
    {
        // Move�� Look ��ǲ�� ���������� ������ �ٲ��ֵ��� �̺�Ʈ�� �Լ� ���ε�
        if (_inputActions == null)
        {
            _inputActions = new PlayerInputAction();
            _inputActions.PlayerMovement.Move.performed += _inputActions => { movementInput = _inputActions.ReadValue<Vector2>(); };
            _inputActions.PlayerMovement.Look.performed += i => { cameraInput = i.ReadValue<Vector2>(); } ;
        }

        _inputActions.Enable();
    }

    private void OnDisable() { _inputActions.Disable(); }

    // Locomotion��ũ��Ʈ���� ����
    public void TickInput(float delta)
    {
        MoveInput(delta); 
        HandleRollInput(delta); 
    }

    // �̵� �� ���콺 ������ ���� (TickInput���� ����)
    private void MoveInput(float delta)
    {
        _horizontal = movementInput.x;
        _vertical = movementInput.y;
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
        _mouseX = cameraInput.x;
        _mouseY = cameraInput.y;
    }

    // ������, �޸��� ���� ���� (TickInput���� ����)
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