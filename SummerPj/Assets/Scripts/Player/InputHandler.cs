using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public bool la_input;
    public bool ha_input;
    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;

    public bool _dodgeFlag;
    public bool _sprintFlag;
    public bool _comboFlag;
    public float _dodgeInputTimer;
    #endregion

    PlayerInputAction _inputActions;
    PlayerAttack _playerAttack;
    PlayerInventory _playerInventory;
    PlayerManager _playerManager;

    Vector2 movementInput;
    Vector2 cameraInput;

    #region '��ǲ�� ���� ���ϴ� ����'�� ��ȯ�����ִ� �Լ���
    private void Awake()
    {
        _playerAttack = GetComponent<PlayerAttack>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerManager = GetComponent<PlayerManager>();
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
        HandleAttackInput(delta);
        HandleQuickSlotsInput();
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

    private void HandleAttackInput(float delta)
    {
        _inputActions.PlayerActions.LightAttack.performed += i => { la_input = true; };
        _inputActions.PlayerActions.HeavyAttack.performed += i => { ha_input = true; };

        if (la_input)
        {
            if (_playerManager._canDoCombo)
            {
                _comboFlag = true;
                _playerAttack.HandleWeaponCombo(_playerInventory._rightWeapon);
                _comboFlag = false;
            }
            else
            {
                if (_playerManager._isInteracting)
                    return;
                if (_playerManager._canDoCombo)
                    return;

                _playerAttack.HandleLightAttack(_playerInventory._rightWeapon);
            }
        }
        if (ha_input)
        {
            _playerAttack.HandleHeavyAttack(_playerInventory._rightWeapon);
        }
    }

    private void HandleQuickSlotsInput()
    {
        _inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
        _inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;

        if (d_Pad_Right)
        {
            _playerInventory.ChangeRightWeapon();
        }
        else if (d_Pad_Left)
        {
            //_playerInventory.ChangeLeftWeapon();
        }
    }
    #endregion
}