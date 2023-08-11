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
    public bool x_Input;
    public bool y_Input;
    public bool la_input;
    public bool ha_input;
    public bool lt_Input;
    public bool lb_Input;
    public bool up_Arrow_Input;
    public bool critical_Attack_Input;
    public bool jump_Input;
    public bool inventory_Input;
    public bool lockOnInput;
    public bool right_Stick_Right_Input;
    public bool right_Stick_Left_Input;
    public bool _canLockOnMove = false;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;

    public bool _dodgeFlag;
    public bool _sprintFlag;
    public bool _comboFlag;
    public bool _lockOnFlag;
    public bool _inventoryFlag;
    public float _dodgeInputTimer;

    public Transform criticalAttackRayCastStartPoint;
    #endregion

    float _lockOnCool;
    public float _sensitivity_x;
    public float _sensitivity_y;
    PlayerInputAction _inputActions;
    PlayerLocomotionManager _playerLocomotionManager;
    PlayerCombatManager _playerCombatManager;
    PlayerInventoryManager _playerInventoryManager;
    PlayerManager _playerManager;
    PlayerWeaponSlotManager _weaponSlotManager;
    PlayerStatsManager _playerStatsManager;
    CameraHandler _cameraHandler;
    PlayerEffectsManager _playerEffectsManager;
    BlockingCollider _blockingCollider;
    UIManager _uiManager;
    PlayerAnimatorManager _playerAnimatorManager;

    Vector2 _movementInput;
    Vector2 _cameraInput;

    private void Awake()
    {
        _blockingCollider = GetComponentInChildren<BlockingCollider>();
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _uiManager = FindObjectOfType<UIManager>();

        _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        _playerCombatManager = GetComponent<PlayerCombatManager>();
        _playerInventoryManager = GetComponent<PlayerInventoryManager>();
        _playerManager = GetComponent<PlayerManager>();
        _weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    public void Update()
    {
        if (_canLockOnMove == false)
        {
            _lockOnCool += Time.deltaTime;
            if(_lockOnCool > 1.5f)
            {
                _canLockOnMove = true;
            }
        }
        else { _lockOnCool = 0f; }

        if (_mouseX < -50) right_Stick_Left_Input = true;
        if (_mouseX > 50) right_Stick_Right_Input = true;
    }

    #region '��ǲ�� ���� ���ϴ� ����'�� ��ȯ�����ִ� �Լ���
    private void OnEnable()
    {
        // Move�� Look ��ǲ�� ���������� ������ �ٲ��ֵ��� �̺�Ʈ�� �Լ� ���ε�
        if (_inputActions == null)
        {
            _inputActions = new PlayerInputAction();
            _inputActions.PlayerMovement.Move.performed += _inputActions => { _movementInput = _inputActions.ReadValue<Vector2>(); };
            _inputActions.PlayerMovement.Look.performed += i => { _cameraInput = i.ReadValue<Vector2>(); };
            _inputActions.PlayerActions.LightAttack.performed += i => { la_input = true; };
            _inputActions.PlayerActions.HeavyAttack.performed += i => { ha_input = true; };
            _inputActions.PlayerActions.Ultimate.performed += i => { up_Arrow_Input = true; };
            _inputActions.PlayerActions.Block.performed += i => { lb_Input = true; };
            _inputActions.PlayerActions.Block.canceled += i => { lb_Input = false; };
            _inputActions.PlayerActions.Parry.performed += i => { lt_Input = true; };
            _inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
            _inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
            _inputActions.PlayerActions.Interact.performed += i => { a_input = true; };
            _inputActions.PlayerActions.Dodge.performed += i => { b_input = true; };
            _inputActions.PlayerActions.Dodge.canceled += i => { b_input = false; };
            _inputActions.PlayerActions.UseItem.performed += i => { x_Input = true; };
            _inputActions.PlayerActions.Jump.performed += i => { jump_Input = true; };
            _inputActions.PlayerActions.Inventory.performed += i => { inventory_Input = true; };
            _inputActions.PlayerMovement.LockOn.performed += i => { lockOnInput = true; };
            _inputActions.PlayerMovement.LockOnTargetLeft.performed += i => { right_Stick_Left_Input = true; };
            _inputActions.PlayerMovement.LockOnTargetRight.performed += i => { right_Stick_Right_Input = true; };
            _inputActions.PlayerMovement.LockOnTargetLeftMouce.performed += i =>
            _inputActions.PlayerActions.CriticalAttack.performed += i => critical_Attack_Input = true;

            _inputActions.Enable();
        }
        #endregion

    }

    private void OnDisable() { _inputActions.Disable(); }

    // ������Ʈ�� �Լ����� ����
    public void TickInput(float delta)
    {
        HandleMoveInput(delta); 
        HandleRollInput(delta);
        HandleCombatInput(delta);
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleCriticalAttackInput();
        HandleUseConsumableInput();
    }

    // �̵� �� ���콺 ������ ���� (TickInput���� ����)
    private void HandleMoveInput(float delta)
    {
        _horizontal = _movementInput.x;
        _vertical = _movementInput.y;
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
        _mouseX = _cameraInput.x * _sensitivity_x;
        _mouseY = _cameraInput.y * _sensitivity_y;
    }

    // ������, �޸��� ���� ���� (TickInput���� ����)
    private void HandleRollInput(float delta)
    {
        b_input = _inputActions.PlayerActions.Dodge.IsPressed();

        if (b_input)
        {
            _dodgeInputTimer += delta;

            if(_playerStatsManager._currentStamina <= 0)
            {
                b_input = false;
                _sprintFlag = false;
            }

            if(_moveAmount> 0.5f && _playerStatsManager._currentStamina > 0)
            {
                _sprintFlag = true;
            }
        }
        else
        {
            _sprintFlag = false;

            if (_dodgeInputTimer > 0 && _dodgeInputTimer < 0.5f)
            {
                _dodgeFlag = true;
            }

            _dodgeInputTimer = 0;
        }    
    }

    private void HandleCombatInput(float delta)
    {
        if (la_input)
        {
            _playerCombatManager.HandleRBAction();
        }
        if (ha_input)
        {
            _playerCombatManager.HandleHeavyAttack(_playerInventoryManager._currentWeapon);
        }
        if(up_Arrow_Input)
        {
            _playerCombatManager.HandleUltimateAction();
        }
        if(lb_Input)
        {
            _playerCombatManager.HandleLBAtcion();
        }
        else
        {
            _playerManager.isBlocking = false;

            if(_blockingCollider.blockingCollider.enabled)
            {
                _blockingCollider.DisableBlockingCollider();
            }
        }

        if(lt_Input)
        {
            _playerCombatManager.HandleLTAction();
        }
    }

    private void HandleQuickSlotsInput()
    {
        _inputActions.PlayerQuickSlots.DPadRight.performed += i => { d_Pad_Right = true; };

        if (!_playerManager._isInteracting)
        {
            if (d_Pad_Right)
            {
                _playerInventoryManager.ChangeWeapon();
            }
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
                _uiManager._hudWindow.SetActive(false);
            }
            else
            {
                _uiManager.CloseSelectWindow();
                _uiManager.CloseAllInventoryWindows();
                _uiManager._hudWindow.SetActive(true);
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
            _canLockOnMove = false;

            _cameraHandler.HandleLockOn();
            if (_cameraHandler._leftLockTaregt != null)
            {
                _cameraHandler._currentLockOnTarget = _cameraHandler._leftLockTaregt;
            }
        }

        if (_lockOnFlag && right_Stick_Right_Input && _canLockOnMove)
        {
            right_Stick_Right_Input = false;
            _canLockOnMove = false;
                        
            _cameraHandler.HandleLockOn();
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
            _weaponSlotManager.LoadWeaponOnSlot(_playerInventoryManager._currentWeapon);
        }
    }

    private void HandleCriticalAttackInput()
    {
        critical_Attack_Input = false;
        _playerCombatManager.AttemptBackStabOrRiposte(); 
    }

    private void HandleUseConsumableInput()
    {
        if (!_playerManager._isInteracting)
        {
            if (x_Input)
            {
                x_Input = false;
                _playerInventoryManager.currentConsumable.AttemptToConsumeItem(_playerAnimatorManager, _weaponSlotManager, _playerEffectsManager);
            }
        }
    }
}