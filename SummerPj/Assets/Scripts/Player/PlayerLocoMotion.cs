using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

// InputHandler에서 '인풋에 따라 변하는 변수들'을 가져와 플레이어를 이동시킴
public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager _playerManager;
    Transform _cameraObject;
    InputHandler _inputHandler;
    public Vector3 _moveDirection;

    [HideInInspector]
    public Transform _myTrnasform;
    [HideInInspector]
    public AnimatorHandler _animHandler;

    public Rigidbody _rigid;
    public GameObject _nomalCamera;

    [Header("Ground & Ari Detection Stats")]
    [SerializeField]
    float _groundDetectionRayStartPoint = 0.5f;
    [SerializeField]
    float _minimumDistanceNeededToBeginFall = 1f;
    [SerializeField]
    float _groundDirectionRayDistance = 0.2f;
    LayerMask _ignoreForGroundCheck;
    public float _inAirTimer;

    [Header("Movement Stats")]
    [SerializeField]
    float _movementSpeed = 5;
    [SerializeField]
    float _sprintSpeed = 7;
    [SerializeField]
    float _rotationSpeed = 10;
    [SerializeField]
    float _fallingSpeed = 80;
    

    private void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        _rigid = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
        _animHandler = GetComponentInChildren<AnimatorHandler>();
        _cameraObject = Camera.main.transform;
        _myTrnasform = transform;
        _animHandler.Init();

        _playerManager._isGrounded = true;
        _ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }

    #region 이동
    Vector3 normalVector;
    Vector3 _targetPosition;

    // 로테이션 변경
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

    // 포지션 변경
    public void HandleMovement(float delta)
    {
        if (_inputHandler._dodgeFlag)
            return;

        if (_playerManager._isInteracting)
            return;

        _moveDirection = _cameraObject.forward * _inputHandler._vertical;
        _moveDirection += _cameraObject.right * _inputHandler._horizontal;
        _moveDirection.y = 0;
        _moveDirection.Normalize();

        float speed = _movementSpeed;

        if (_inputHandler._sprintFlag && _inputHandler._moveAmount > 0.5f)
        {
            speed = _sprintSpeed;
            _playerManager._isSprinting = true;
            _moveDirection *= speed;
        }
        else
        {
            if (_inputHandler._moveAmount < 0.5f)
            {
                _moveDirection *= _movementSpeed;
                _playerManager._isSprinting = false;
            }
            else
            {
                _moveDirection *= speed;
                _playerManager._isSprinting = true;
            }
        }
        
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, normalVector);
        _rigid.velocity = projectedVelocity;

        _animHandler.UpdateAnimatorValues(_inputHandler._moveAmount, 0, _playerManager._isSprinting); // 애니메이션에게도 정보를 넘김

        if (_animHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    // 구르기 또는 달리기 트리거
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

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        _playerManager._isGrounded = false;
        RaycastHit hit;
        Vector3 origin = _myTrnasform.position;
        origin.y += _groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, _myTrnasform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (_playerManager._isInAir)
        {
            _rigid.AddForce(-Vector3.up * _fallingSpeed);
            _rigid.AddForce(moveDirection * _fallingSpeed / 10);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * _groundDirectionRayDistance;

        _targetPosition = _myTrnasform.position;

        Debug.DrawLine(origin, -Vector3.up * _minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, _minimumDistanceNeededToBeginFall, _ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            _playerManager._isGrounded = true;
            _targetPosition.y = tp.y;

            if (_playerManager._isInAir)
            {
                if (_inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air  for" + _inAirTimer);
                    _animHandler.PlayTargetAnimation("Land", true);
                    _inAirTimer = 0;
                }
                else
                {
                    _animHandler.PlayTargetAnimation("Locomotion", false);
                    _inAirTimer = 0;
                }

                _playerManager._isInAir = false;
            }
        }
        else
        {
            if (_playerManager._isGrounded)
            {
                _playerManager._isGrounded = false;
            }

            if (_playerManager._isInAir == false)
            {
                if (_playerManager._isInteracting == false)
                {
                    _animHandler.PlayTargetAnimation("Falling", true);
                }

                Vector3 vel = _rigid.velocity;
                vel.Normalize();
                _rigid.velocity = vel * (_movementSpeed / 2);
                _playerManager._isInAir = true;
            }
        }

        if (_playerManager._isGrounded)
        {
            if (_playerManager._isInteracting || _inputHandler._moveAmount > 0)
            {
                _myTrnasform.position = Vector3.Lerp(_myTrnasform.position, _targetPosition, Time.deltaTime);
            }
            else
            {
                _myTrnasform.position = _targetPosition;
            }
        }
    }

    #endregion
}
