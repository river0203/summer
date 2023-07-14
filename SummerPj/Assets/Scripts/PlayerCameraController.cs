using Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("카메라 하단 최대 범위")]
    [SerializeField] float BottomClamp = -30.0f;

    [Tooltip("카메라 상단 최대 범위")]
    [SerializeField] float TopClamp = 70.0f;

    [SerializeField]
    float _interval;

    [SerializeField]
    Define.CameraType _cameraType = Define.CameraType.Normal;

    [Header("Lock On")]
    [Tooltip("락온 탐지 범위")]
    [SerializeField] float noticeZone = 10f;

    [Tooltip("최대 탐지 범위")]
    [SerializeField] float maxNoticeAngle = 60f;

    [Tooltip("락온 UI")]
    [SerializeField] Transform LockOn_UI;

    [Tooltip("락온 대상과의 최대 거리")]
    [SerializeField] float maxDistance = 10f;

    [Tooltip("락온 카메라 위치")]
    [SerializeField] Transform LockOn_CameraTarget;

    Transform _target;

    float _seta;
    float _pi;

    GameObject _player;
    PlayerInputActions _input;
    CharacterController _collider;

    void Start()
    {
        _player = GameObject.Find("Player");
        _input = _player.GetComponent<PlayerInputActions>();
        _collider = _player.GetComponent<CharacterController>();
    }

    void NormalMode()
    {
        // 입력에 따라 카메라 이동
        _seta += _input.look.y * Time.deltaTime;
        _pi += _input.look.x * Time.deltaTime;

        _pi = Mathf.Repeat(_pi, Mathf.Deg2Rad * 360);
        _seta = Mathf.Clamp(_seta, Mathf.Deg2Rad * (-TopClamp + 90), Mathf.Deg2Rad * (-BottomClamp + 90));

        float x = _interval * Mathf.Sin(_seta) * Mathf.Cos(_pi);
        float y = _interval * Mathf.Cos(_seta);
        float z = _interval * Mathf.Sin(_seta) * Mathf.Sin(_pi);

        // 위치
        transform.position = _player.transform.position + new Vector3(-x, _collider.height + y, z);

        // 각도
        Vector3 lookPos = _player.transform.position + new Vector3(0, _collider.height);
        transform.LookAt(lookPos);
    }
    void LockOnMode()
    {
        // 위치
        Vector3 dir = _target.transform.position - _player.transform.position;
        dir = -dir.normalized * _interval;

        dir = Quaternion.AngleAxis(50, Vector3.right) * dir;
        Vector3 pos = _player.transform.position + new Vector3(0, _collider.height, 0) + dir;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 3);
        
        // 방향
        Vector3 lookPos = _target.transform.position;
        transform.LookAt(lookPos);
    }
    void LateUpdate()
    {
        if (_cameraType == Define.CameraType.Normal)
        {
            NormalMode();
        }
        else if (_cameraType == Define.CameraType.LockOn)
        {
            LockOnMode();
        }
    }

    public void TogleLockOnMode()
    {
        if (_cameraType != Define.CameraType.LockOn)
        {
            Transform target = Scan();
            if (target != null)
            {
                _target = target;
                _cameraType = Define.CameraType.LockOn;
            }
        }
        else
        {
            _cameraType = Define.CameraType.Normal;
        }
    }

    static float PitchClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    float YawClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < lfMin)
            lfAngle = lfMax;

        if (lfAngle > lfMax)
            lfAngle = lfMin;

        return lfAngle;
    }
    Transform Scan()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, LayerMask.GetMask("Enemy"));
        float closestAngle = maxNoticeAngle;
        Transform currentTarget = null;
        if (nearbyTargets.Length <= 0) return null;

        for (int i = 0; i < nearbyTargets.Length; i++)
        {
            Vector3 CurrentCalculationTarget = nearbyTargets[i].transform.position - Camera.main.transform.position;
            CurrentCalculationTarget.y = 0;
            float _angle = Vector3.Angle(Camera.main.transform.forward, CurrentCalculationTarget);

            if (_angle < closestAngle)
            {
                currentTarget = nearbyTargets[i].transform;
                closestAngle = _angle;
            }
        }
        if (!currentTarget) return null;

        return currentTarget;
    }
}