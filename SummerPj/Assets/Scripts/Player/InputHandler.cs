using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public bool a_input;
    public bool y_Input;
    public bool la_input;
    public bool ha_input;
    public bool jump_Input;
    public bool inventory_Input;
    public bool lockOnInput;
    public bool right_Stick_Right_Input;
    public bool right_Stick_Left_Input;
    public bool _canLockOnMove = false;
    float _lockOnInputMoveTimer;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;

    public bool _twoHandFlag;
    public bool _dodgeFlag;
    public bool _sprintFlag;
    public bool _comboFlag;
    public bool _lockOnFlag;
    public bool _inventoryFlag;
    public float _dodgeInputTimer;
    #endregion

    PlayerInputAction _inputActions;
    PlayerAttack _playerAttack;
    PlayerInventory _playerInventory;
    PlayerManager _playerManager;
    WeaponSlotManager _weaponSlotManager;
    CameraHandler _cameraHandler;
    UIManager _uiManager;
    AnimatorHandler _animatorHandler;

    Vector2 _movementInput;
    Vector2 _cameraInput;

    public void Update()
    {
        if (_canLockOnMove == false)
        {
            _lockOnInputMoveTimer += Time.deltaTime;
            if (_lockOnInputMoveTimer > 0.5)
            {
                _canLockOnMove = true;
            }
                
        }
    }

    private void Awake()
    {
        _playerAttack = GetComponent<PlayerAttack>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerManager = GetComponent<PlayerManager>();
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _uiManager = FindObjectOfType<UIManager>();
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        _animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    #region '��ǲ�� ���� ���ϴ� ����'�� ��ȯ�����ִ� �Լ���
    private void OnEnable()
    {
        // Move�� Look ��ǲ�� ���������� ������ �ٲ��ֵ��� �̺�Ʈ�� �Լ� ���ε�
        if (_inputActions == null)
        {
            _inputActions = new PlayerInputAction();
            _inputActions.PlayerMovement.Move.performed += _inputActions => { _movementInput = _inputActions.ReadValue<Vector2>(); };
            _inputActions.PlayerMovement.Look.performed += i => { _cameraInput = i.ReadValue<Vector2>(); } ;
            _inputActions.PlayerActions.LightAttack.performed += i => { la_input = true; };
            _inputActions.PlayerActions.HeavyAttack.performed += i => { ha_input = true; };
            _inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
            _inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
            _inputActions.PlayerActions.Interact.performed += i => { a_input = true; };
            _inputActions.PlayerActions.Jump.performed += i => { jump_Input = true; };
            _inputActions.PlayerActions.Inventory.performed += i => { inventory_Input = true; };
            _inputActions.PlayerMovement.LockOn.performed += i => { lockOnInput = true; };
            _inputActions.PlayerActions.Y.performed += i => y_Input = true;
            _inputActions.PlayerMovement.LockOnTargetLeft.performed += i => { right_Stick_Left_Input = true; };
            _inputActions.PlayerMovement.LockOnTargetLeftMouce.performed += i =>

            {
                if (_mouseX < -10)
                {
                    right_Stick_Left_Input = true;
                }
            };
            _inputActions.PlayerMovement.LockOnTargetRight.performed += i => { right_Stick_Right_Input = true; };
            _inputActions.PlayerMovement.LockOnTargetRightMouce.performed += i => 
            {
                if (_mouseX > 10)
                {
                    right_Stick_Right_Input = true;
                }
            };
        }

        _inputActions.Enable();
    }

    private void OnDisable() { _inputActions.Disable(); }

    // ������Ʈ�� �Լ����� ����
    public void TickInput(float delta)
    {
        HandleMoveInput(delta); 
        HandleRollInput(delta);
        HandleAttackInput(delta);
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
    }

    // �̵� �� ���콺 ������ ���� (TickInput���� ����)
    private void HandleMoveInput(float delta)
    {
        _horizontal = _movementInput.x;
        _vertical = _movementInput.y;
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
        _mouseX = _cameraInput.x;
        _mouseY = _cameraInput.y;
    }

    // ������, �޸��� ���� ���� (TickInput���� ����)
    private void HandleRollInput(float delta)
    {
        b_input = _inputActions.PlayerActions.Dodge.IsPressed();
        _sprintFlag = b_input;

        if (b_input)
        {
            _dodgeInputTimer += delta;
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

                _animatorHandler._anim.SetBool("isUsingRightHand", true);
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
        _inputActions.PlayerQuickSlots.DPadRight.performed += i => { d_Pad_Right = true; };
        _inputActions.PlayerQuickSlots.DPadLeft.performed += i => { d_Pad_Left = true; };

        if (d_Pad_Right)
        {
            _playerInventory.ChangeRightWeapon();
        }
        else if (d_Pad_Left)
        {
            _playerInventory.ChangeLeftWeapon();
        }
    }

    private void HandleInventoryInput()
    {
        if(inventory_Input)
        {
            _inventoryFlag = !_inventoryFlag;

            if(_inventoryFlag)
            {
                _uiManager.OpenSelectWindow();
                _uiManager.UpdateUI();
                _uiManager.hudWindow.SetActive(false);
            }
            else
            {
                _uiManager.CloseSelectWindow();
                _uiManager.CloseAllInventoryWindows();
                _uiManager.hudWindow.SetActive(true);
            }
        }
    }

    private void HandleLockOnInput()
    {
        if (lockOnInput && _lockOnFlag == false)
        {
            lockOnInput = false;
            _cameraHandler.HandleLockOn();
            
            if (_cameraHandler._nearestLockOnTarget != null)
            {
                _cameraHandler._currentLockOnTarget = _cameraHandler._nearestLockOnTarget;
                _canLockOnMove = false;
                _lockOnInputMoveTimer = 0;
                _lockOnFlag = true;
            }
        }
        else if (lockOnInput && _lockOnFlag)
        {
            lockOnInput = false;
            _lockOnFlag = false;
            _cameraHandler.ClearLockOnTargets();
        }

        if (_lockOnFlag && right_Stick_Left_Input && _canLockOnMove)
        {
            right_Stick_Left_Input = false;
            _cameraHandler.HandleLockOn();

            _canLockOnMove = false;
            _lockOnInputMoveTimer = 0;

            if (_cameraHandler._leftLockTaregt != null)
            {
                _cameraHandler._currentLockOnTarget = _cameraHandler._leftLockTaregt;
            }
        }

        if (_lockOnFlag && right_Stick_Right_Input && _canLockOnMove)
        {
            right_Stick_Right_Input = false;
            _cameraHandler.HandleLockOn();
            
            _canLockOnMove = false;
            _lockOnInputMoveTimer = 0;

            if (_cameraHandler._rightLockTaregt != null)
            {
                _cameraHandler._currentLockOnTarget = _cameraHandler._rightLockTaregt;
            }
        }

        _cameraHandler.SetCameraHeight();
    }

    private void HandleTwoHandInput()
    {
        if(y_Input)
        {
            _twoHandFlag = !_twoHandFlag;

            if(_twoHandFlag)
            {
                _weaponSlotManager.LoadWeaponOnSlot(_playerInventory._rightWeapon, false);
            }
            else
            {
                _weaponSlotManager.LoadWeaponOnSlot(_playerInventory._rightWeapon, false);
                _weaponSlotManager.LoadWeaponOnSlot(_playerInventory._leftWeapon, true);
            }
        }
    }
#endregion
}