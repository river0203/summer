using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("카메라 하단 최대 범위")]
    [SerializeField] float BottomClamp = -30.0f;

    [Tooltip("카메라 상단 최대 범위")]
    [SerializeField] float TopClamp = 70.0f;

    [SerializeField]
    Vector3 _interval;

    [SerializeField]
    Define.CameraType _cameraType = Define.CameraType.Normal;

    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;

    GameObject _player;
    PlayerInputActions _input;
    CharacterController _collider;

    void Start()
    {
        _player = GameObject.Find("Player");
        _input = _player.GetComponent<PlayerInputActions>();
        _collider = _player.GetComponent<CharacterController>();
    }

    void LateUpdate()
    {
        CameraRotation();
    }

    void CameraRotation()
    {
        // 입력에 따라 카메라 이동
        _cinemachineTargetYaw += _input.look.x * Time.deltaTime;
        _cinemachineTargetPitch += _input.look.y * Time.deltaTime;
        _cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, -1, 1);

        float x = Mathf.Cos(_cinemachineTargetYaw) * _interval.z;
        float y = Mathf.Sin(_cinemachineTargetPitch) * _interval.z;
        float z = Mathf.Sin(_cinemachineTargetYaw) * _interval.z;

        // 위치
        transform.position = _player.transform.position + new Vector3(-x, _collider.height + y, z);

        // 각도
        Vector3 lookPos = _player.transform.position + new Vector3(0, _collider.height);
        transform.LookAt(lookPos);
    }

    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
