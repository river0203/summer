using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

// InputHandler에서 '인풋에 따라 변하는 변수들'을 가져와 플레이어를 이동시킴
public class PlayerLocomotion : MonoBehaviour
{
    CameraHandler _cameraHandler;
    PlayerManager _playerManager;
    Transform _cameraObject;
    InputHandler _inputHandler;
    public Vector3 _moveDirection;

    [HideInInspector]
    public Transform _myTransform;
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
    float _fallingSpeed = 1000;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    private void Awake()
    {
        _cameraHandler = FindObjectOfType<CameraHandler>();

    }

    private void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        _rigid = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
        _animHandler = GetComponentInChildren<AnimatorHandler>();
        _cameraObject = Camera.main.transform;
        _myTransform = transform;
        _animHandler.Init();

        // 플레이어가 시작하자마자 낙하하는 애니메이션이 재생하는 것을 방지
        _playerManager._isGrounded = true;
        _ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

    #region 이동
    Vector3 normalVector;
    Vector3 _targetPosition;

    // 로테이션 변경
    private void HandleRotation(float delta)
    {
        if (_inputHandler._lockOnFlag )
        {
            if (_inputHandler._sprintFlag || _inputHandler._dodgeFlag)
            {
                Vector3 targetDirection = Vector3.zero;
                targetDirection = _cameraHandler._cameraTransform.forward * _inputHandler._vertical;
                targetDirection = _cameraHandler._cameraTransform.right * _inputHandler._horizontal;
                targetDirection.Normalize();
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero)
                {
                    targetDirection = transform.forward;
                }

                Quaternion tr = Quaternion.LookRotation(targetDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, _rotationSpeed * Time.deltaTime);

                transform.rotation = targetRotation;
            }
            else
            {
                Vector3 rotationDirection = _moveDirection;
                rotationDirection = _cameraHandler._currentLockOnTarget.position - transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, _rotationSpeed * Time.deltaTime);
                transform.rotation = targetRotation;
            }
        }
        else
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = _inputHandler._moveAmount;

            targetDir = _cameraObject.forward * _inputHandler._vertical;
            targetDir += _cameraObject.right * _inputHandler._horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = _myTransform.forward;
            }

            float rs = _rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(_myTransform.rotation, tr, rs * delta);

            _myTransform.rotation = targetRotation;
        }
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
                _playerManager._isSprinting = false;
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, normalVector);
        _rigid.velocity = projectedVelocity;

        if (_inputHandler.lockOnInput && _inputHandler._sprintFlag == false)
        {
            _animHandler.UpdateAnimatorValues(_inputHandler._vertical, _inputHandler._horizontal, _playerManager._isSprinting); // 애니메이션에게도 정보를 넘김
        }
        else
        {
            _animHandler.UpdateAnimatorValues(_inputHandler._moveAmount, 0, _playerManager._isSprinting); // 애니메이션에게도 정보를 넘김
        }
        
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
                _myTransform.rotation = rollRotation;
            }
            else
            {
                _animHandler.PlayTargetAnimation("BackStep", true);
            }
        }
    }

    // 낙하 판정 및 물리 이동
    public void HandleFalling(float delta, Vector3 moveDirection) // moveDirection : 떨어질때 이동할 방향
    {
        _playerManager._isGrounded = false;
        RaycastHit hit;
        // 스캔할 원점 잡기
        Vector3 origin = _myTransform.position;
        origin.y += _groundDetectionRayStartPoint;

        // 떨어지는 도중 앞에 벽이 있어도 비비지 않게 막음
        if (Physics.Raycast(origin, _myTransform.forward, out hit, 0.6f))
        {
            moveDirection = Vector3.zero;
        }

        // 낙하 물리 이동
        if (_playerManager._isInAir)
        {
            _rigid.AddForce(-Vector3.up * _fallingSpeed);
            _rigid.AddForce(moveDirection * _fallingSpeed / 10); // 떨어지는 방향으로 힘을 약간 줌
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * _groundDirectionRayDistance;

        _targetPosition = _myTransform.position;

        Debug.DrawLine(origin, origin + (-Vector3.up * _minimumDistanceNeededToBeginFall), Color.green, 0.1f, false);
        // 바닥에 닿아 있을 때 처리
        if (Physics.Raycast(origin, -Vector3.up, out hit, _minimumDistanceNeededToBeginFall, _ignoreForGroundCheck))
        {
            normalVector = hit.normal; // 충돌한 지점의 방향
            Vector3 tp = hit.point; // 충돌한 지점
            _playerManager._isGrounded = true;
            _targetPosition.y = tp.y;

            // 착지시
            if (_playerManager._isInAir)
            {
                if (_inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air  for " + _inAirTimer);
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
        // 바닥에 닿아있지 않을 때 처리
        else
        {
            if (_playerManager._isGrounded)
            {
                _playerManager._isGrounded = false;
            }

            if (!_playerManager._isInAir)
            {
                if (_playerManager._isInteracting == false)
                {
                    _animHandler.PlayTargetAnimation("Falling", true); // 낙하 애니메이션
                }

                Vector3 vel = _rigid.velocity;
                vel.Normalize();
                _rigid.velocity = vel * (_movementSpeed / 3);
                _playerManager._isInAir = true;
            }
        }

        // 땅에 제대로 닿게 함 (안 하면 레이저가 딱 감지한 부분에 떠있음)
        if (_playerManager._isInteracting || _inputHandler._moveAmount > 0)
        {
            _myTransform.position = Vector3.Lerp(_myTransform.position, _targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            _myTransform.position = _targetPosition;
        }
    }

    // 점프
    public void HandleJumping()
    {
        if (_playerManager._isInteracting)
            return;

        if(_inputHandler.jump_Input)
        {
            if(_inputHandler._moveAmount > 0)
            {
                _moveDirection = _cameraObject.forward * _inputHandler._vertical;
                _moveDirection += _cameraObject.right * _inputHandler._horizontal;
                _animHandler.PlayTargetAnimation("Jump", true);
                _moveDirection.y = 0;

                // 위쪽으로 힘을 주는 코드 추가

                Quaternion jumpRotation = Quaternion.LookRotation(_moveDirection);
                _myTransform.rotation = jumpRotation;
            }
        }
    }
    #endregion
}
