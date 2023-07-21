using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

// InputHandler���� '��ǲ�� ���� ���ϴ� ������'�� ������ �÷��̾ �̵���Ŵ
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

        // �÷��̾ �������ڸ��� �����ϴ� �ִϸ��̼��� ����ϴ� ���� ����
        _playerManager._isGrounded = true;
        _ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

    #region �̵�
    Vector3 normalVector;
    Vector3 _targetPosition;

    // �����̼� ����
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

    // ������ ����
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
            _animHandler.UpdateAnimatorValues(_inputHandler._vertical, _inputHandler._horizontal, _playerManager._isSprinting); // �ִϸ��̼ǿ��Ե� ������ �ѱ�
        }
        else
        {
            _animHandler.UpdateAnimatorValues(_inputHandler._moveAmount, 0, _playerManager._isSprinting); // �ִϸ��̼ǿ��Ե� ������ �ѱ�
        }
        
        if (_animHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    // ������ �Ǵ� �޸��� Ʈ����
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

    // ���� ���� �� ���� �̵�
    public void HandleFalling(float delta, Vector3 moveDirection) // moveDirection : �������� �̵��� ����
    {
        _playerManager._isGrounded = false;
        RaycastHit hit;
        // ��ĵ�� ���� ���
        Vector3 origin = _myTransform.position;
        origin.y += _groundDetectionRayStartPoint;

        // �������� ���� �տ� ���� �־ ����� �ʰ� ����
        if (Physics.Raycast(origin, _myTransform.forward, out hit, 0.6f))
        {
            moveDirection = Vector3.zero;
        }

        // ���� ���� �̵�
        if (_playerManager._isInAir)
        {
            _rigid.AddForce(-Vector3.up * _fallingSpeed);
            _rigid.AddForce(moveDirection * _fallingSpeed / 10); // �������� �������� ���� �ణ ��
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * _groundDirectionRayDistance;

        _targetPosition = _myTransform.position;

        Debug.DrawLine(origin, origin + (-Vector3.up * _minimumDistanceNeededToBeginFall), Color.green, 0.1f, false);
        // �ٴڿ� ��� ���� �� ó��
        if (Physics.Raycast(origin, -Vector3.up, out hit, _minimumDistanceNeededToBeginFall, _ignoreForGroundCheck))
        {
            normalVector = hit.normal; // �浹�� ������ ����
            Vector3 tp = hit.point; // �浹�� ����
            _playerManager._isGrounded = true;
            _targetPosition.y = tp.y;

            // ������
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
        // �ٴڿ� ������� ���� �� ó��
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
                    _animHandler.PlayTargetAnimation("Falling", true); // ���� �ִϸ��̼�
                }

                Vector3 vel = _rigid.velocity;
                vel.Normalize();
                _rigid.velocity = vel * (_movementSpeed / 3);
                _playerManager._isInAir = true;
            }
        }

        // ���� ����� ��� �� (�� �ϸ� �������� �� ������ �κп� ������)
        if (_playerManager._isInteracting || _inputHandler._moveAmount > 0)
        {
            _myTransform.position = Vector3.Lerp(_myTransform.position, _targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            _myTransform.position = _targetPosition;
        }
    }

    // ����
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

                // �������� ���� �ִ� �ڵ� �߰�

                Quaternion jumpRotation = Quaternion.LookRotation(_moveDirection);
                _myTransform.rotation = jumpRotation;
            }
        }
    }
    #endregion
}
