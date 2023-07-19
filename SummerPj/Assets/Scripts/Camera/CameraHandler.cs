using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    InputHandler _inputHandler;

    public Transform _targetTransform;
    public Transform _cameraTransform;
    public Transform _cameraPivotTransform;
    private Transform _myTransform;
    public LayerMask _ignoreLayers;
    private Vector3 _cameraTransformPosition;
    private Vector3 _cameraFollowVelocity = Vector3.zero;

    public float _lookSpeed = 0.1f;
    public float _followSpeed = 0.1f;
    public float _pivotSpeed = 0.03f;

    private float _targetPosition;
    private float _defaultPosition;
    private float _lookAngle;
    private float _pivotAngle;
    public float _minimumPivot = -35;
    public float _maximumPivot = 35;

    public float _cameraShereRadius = 0.2f;
    public float _cameraCollisionOffset = 0.2f;
    public float _minimumCollisionOffset = 0.2f;
    public float _lockedPivotPosition = 2.25f;
    public float _unlockedPivotPosition = 1.65f;

    public Transform _currentLockOnTarget;

    List<CharacterManager> _availableTargets = new List<CharacterManager>();
    public Transform _nearestLockOnTarget;
    public Transform _leftLockTaregt;
    public Transform _rightLockTaregt;
    public float _maximumLockOnDistance = 30f;

    private void Awake()
    {
        _myTransform = transform;
        _defaultPosition = _cameraTransform.localPosition.z;
        _ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        _targetTransform = FindObjectOfType<PlayerManager>().transform;
        
        _inputHandler = _targetTransform.GetComponent<InputHandler>();
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(_myTransform.position, _targetTransform.position, ref _cameraFollowVelocity, delta / _followSpeed);
        _myTransform.position = targetPosition;

        HandleCameraCollisions(delta);
    }

    public void HandlerCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        if (_inputHandler._lockOnFlag == false && _currentLockOnTarget == null)
        {
            _lookAngle += (mouseXInput * _lookSpeed) / delta;
            _pivotAngle -= (mouseYInput * _pivotSpeed) / delta;
            _pivotAngle = Mathf.Clamp(_pivotAngle, _minimumPivot, _maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = _lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            _myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = _pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            _cameraPivotTransform.localRotation = targetRotation;
        }
        else
        {
            float velocity = 0;

            Vector3 diraction = _currentLockOnTarget.position - transform.position;

            diraction.Normalize();
            diraction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(diraction);
            transform.rotation = targetRotation;

            diraction = _currentLockOnTarget.position - _cameraPivotTransform.position;
            diraction.Normalize();

            targetRotation = Quaternion.LookRotation(diraction);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            _cameraPivotTransform.localEulerAngles = eulerAngle; 
        }
        
    }

    void HandleCameraCollisions(float delta)
    {
        _targetPosition = _defaultPosition;
        RaycastHit hit;
        Vector3 direction = _cameraTransform.position - _cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast
            (_cameraPivotTransform.position, _cameraShereRadius, direction, out hit, 
            Mathf.Abs(_targetPosition), _ignoreLayers))
        {
            float dis = Vector3.Distance(_cameraPivotTransform.position, hit.point);
            _targetPosition = -(dis - _cameraCollisionOffset);
        }

        if (Mathf.Abs(_targetPosition) < _minimumCollisionOffset)
        {
            _targetPosition = -_minimumCollisionOffset;
        }

        _cameraTransformPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, _targetPosition, delta / 0.2f);
        _cameraTransform.localPosition = _cameraTransformPosition;
    }

    public void HandleLockOn()
    {
        float shortestDistance = Mathf.Infinity;
        float shortesDistanceLeftTaret = Mathf.Infinity;
        float shortesDistanceRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(_targetTransform.position, 26);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if (character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - _targetTransform.position;
                float distanceFromTarget = Vector3.Distance(_targetTransform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDirection, _cameraTransform.forward);

                if (character.transform.root != _targetTransform.root &&
                    viewableAngle > -50 && viewableAngle < 50 &&
                    distanceFromTarget <= _maximumLockOnDistance)
                {
                    _availableTargets.Add(character); 
                }
            }
        }

        for (int i = 0; i < _availableTargets.Count; ++i)
        {
            float distanceFromTarget = Vector3.Distance(_targetTransform.position, _availableTargets[i].transform.position);

            if (distanceFromTarget < shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                _nearestLockOnTarget = _availableTargets[i]._lockOnTransform;
            }

            if (_inputHandler._lockOnFlag)
            {
                Vector3 relativeEnemyPosition = _currentLockOnTarget.InverseTransformPoint(_availableTargets[i].transform.position);
                var distanceFromLeftTarget = _currentLockOnTarget.transform.position.x - _availableTargets[i].transform.position.x;
                var distanceFromRightTarget = _currentLockOnTarget.transform.position.x + _availableTargets[i].transform.position.x;

                if (relativeEnemyPosition.x > 0f && distanceFromLeftTarget < shortesDistanceLeftTaret)
                {
                    shortesDistanceLeftTaret = distanceFromLeftTarget;
                    _leftLockTaregt = _availableTargets[i]._lockOnTransform;
                }

                if (relativeEnemyPosition.x < 0f && distanceFromRightTarget < shortesDistanceRightTarget)
                {
                    shortesDistanceRightTarget = distanceFromRightTarget;
                    _rightLockTaregt = _availableTargets[i]._lockOnTransform;
                }
            }
        }
    }

    public void ClearLockOnTargets()
    {
        _availableTargets.Clear();
        _nearestLockOnTarget = null;
        _currentLockOnTarget = null;
    }

    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, _lockedPivotPosition);
        Vector3 newUnlockedPosition = new Vector3(0, _unlockedPivotPosition);

        if (_currentLockOnTarget != null)
        {
            _cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(_cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        else
        {
            _cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(_cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }
    }
}
