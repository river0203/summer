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
    float _rotationSpeed = 10;

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

        _inputHandler.TickInput(delta);

        _moveDirection = _cameraObject.forward * _inputHandler._vertical;
        _moveDirection += _cameraObject.right * _inputHandler._horizontal;
        _moveDirection.Normalize();

        _moveDirection *= _movementSpeed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, normalVector);
        _rigid.velocity = projectedVelocity;

        _animHandler.UpdateAnimatorValues(_inputHandler._moveAmount, 0);

        if (_animHandler.canRotate)
        {
            HandleRotation(delta);
        }
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

    #endregion
}
