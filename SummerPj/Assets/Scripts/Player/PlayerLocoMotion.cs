using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLocoMotion : MonoBehaviour
{
    Transform _cameraObject;
    InputHandler _inputHandler;
    Vector3 _moveDirection;

    [HideInInspector]
    public Transform _myTrnasform;
    [HideInInspector]
    public AnimatorHandler _animHandler;

    public Rigidbody _rigid;
    public GameObject _nomalCamera;

    [Header("Stats")]
    [SerializeField]
    float _movementSpeed = 5;
    [SerializeField]
    float _sprintSpeed = 7;
    [SerializeField]
    float _rotationSpeed = 10;

    public bool _isSprinting;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
        _animHandler = GetComponentInChildren<AnimatorHandler>();
        _cameraObject = Camera.main.transform;
        _myTrnasform = transform;
        _animHandler.Init();
    }

    public void Update()
    {
        float delta = Time.deltaTime;

        _isSprinting = _inputHandler.b_input;
        _inputHandler.TickInput(delta);
        HandleMovement(delta);
        HandleRollingAndSprinting(delta);
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = _inputHandler._moveAmount;

        targetDir = _cameraObject.forward * _inputHandler._vertical;
        targetDir += _cameraObject.right * _inputHandler._horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = _myTrnasform.forward;
        }

        float rs = _rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(_myTrnasform.rotation, tr, rs * delta);

        _myTrnasform.rotation = targetRotation;
    }

    public void HandleMovement(float delta)
    {
        if (_inputHandler._dodgeFlag)
            return;

        _moveDirection = _cameraObject.forward * _inputHandler._vertical;
        _moveDirection += _cameraObject.right * _inputHandler._horizontal;
        _moveDirection.y = 0;
        _moveDirection.Normalize();

        float speed = _movementSpeed;
        if (_inputHandler._sprintFlag)
        {
            speed = _sprintSpeed;
            _isSprinting = true;
            _moveDirection *= speed;
        }
        else
        {
            _moveDirection *= speed;
        }
        
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, normalVector);
        _rigid.velocity = projectedVelocity;

        _animHandler.UpdateAnimatorValues(_inputHandler._moveAmount, 0, _isSprinting);

        if (_animHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (_animHandler._anim.GetBool("isInteracting"))
            return;

        if (_inputHandler._dodgeFlag)
        {
            _moveDirection = _cameraObject.forward * _inputHandler._vertical;
            _moveDirection += _cameraObject.right * _inputHandler._horizontal;

            if (_inputHandler._moveAmount > 0)
            {
                _animHandler.PlayTargetAnimation("Dodge", true);

                _moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(_moveDirection);
                _myTrnasform.rotation = rollRotation;
            }
            else
            {
                _animHandler.PlayTargetAnimation("Backstep", true);
            }
        }
    }

    #endregion
}
