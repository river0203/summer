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
    public bool critical_Attack_Input;
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

    public Transform criticalAttackRayCastStartPoint;
    #endregion

    PlayerInputAction _inputActions;
    PlayerAttacker _playerAttacker;
    PlayerInventory _playerInventory;
    PlayerManager _playerManager;
    WeaponSlotManager _weaponSlotManager;
    PlayerStats _playerStats;
    CameraHandler _cameraHandler;
    BlockingCollider _blockingCollider;
    UIManager _uiManager;
    PlayerAnimatorManager _animatorHandler;

    Vector2 _movementInput;
    Vector2 _cameraInput;

    private void Awake()
    {
        _blockingCollider = GetComponentInChildren<BlockingCollider>();
        _playerAttacker = GetComponentInChildren<PlayerAttacker>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerManager = GetComponent<PlayerManager>();
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _uiManager = FindObjectOfType<UIManager>();
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        _animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        _playerStats = GetComponent<PlayerStats>();
    }

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

        Debug.Log(_animatorHandler._anim.GetCurrentAnimatorStateInfo(0).length);
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
            _inputActions.PlayerActions.Block.performed += i => { lb_Input = true; };
            _inputActions.PlayerActions.Block.canceled += i => { lb_Input = false; };
            _inputActions.PlayerActions.Parry.performed += i => { lt_Input = true; };
            _inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
            _inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
            _inputActions.PlayerActions.Interact.performed += i => { a_input = true; };
            _inputActions.PlayerActions.Dodge.performed += i => { b_input = true; }; 
            _inputActions.PlayerActions.Dodge.canceled += i => { b_input = false; };
            _inputActions.PlayerActions.X.performed += i => { x_Input = true; };
            _inputActions.PlayerActions.Jump.performed += i => { jump_Input = true; };
            _inputActions.PlayerActions.Inventory.performed += i => { inventory_Input = true; };
            _inputActions.PlayerMovement.LockOn.performed += i => { lockOnInput = true; };
            _inputActions.PlayerActions.Y.performed += i => y_Input = true;
            _inputActions.PlayerMovement.LockOnTargetLeft.performed += i => { right_Stick_Left_Input = true; };
            _inputActions.PlayerMovement.LockOnTargetLeftMouce.performed += i =>
            _inputActions.PlayerActions.CriticalAttack.performed += i => critical_Attack_Input = true;

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
        _mouseX = _cameraInput.x;
        _mouseY = _cameraInput.y;
    }

    // ������, �޸��� ���� ���� (TickInput���� ����)
    private void HandleRollInput(float delta)
    {
        b_input = _inputActions.PlayerActions.Dodge.IsPressed();

        if (b_input)
        {
            _dodgeInputTimer += delta;

            if(_playerStats._currentStamina <= 0)
            {
                b_input = false;
                _sprintFlag = false;
            }

            if(_moveAmount> 0.5f && _playerStats._currentStamina > 0)
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
            _playerAttacker.HandleRBAction();
        }
        if (ha_input)
        {
            _playerAttacker.HandleHeavyAttack(_playerInventory._rightWeapon);
        }

        if(lb_Input)
        {
            _playerAttacker.HandleLBAtcion();
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
            if(_twoHandFlag)
            {

            }
            else
            {
                _playerAttacker.HandleLTAction();
            }
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

    private void HandleCriticalAttackInput()
    {
        critical_Attack_Input = false;
        _playerAttacker.AttemptBackStabOrRiposte(); 
    }

    void HandleUseConsumableInput()
    {
        if(x_Input)
        {
            x_Input = false;
        }
    }
#endregion
}