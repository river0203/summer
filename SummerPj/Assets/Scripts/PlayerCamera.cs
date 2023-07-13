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

    void LateUpdate()
    {
        CameraRotation();
    }

    void CameraRotation()
    {
        // 입력에 따라 카메라 이동
        _seta += _input.look.y * Time.deltaTime;
        _pi += _input.look.x * Time.deltaTime;

        _pi = Mathf.Repeat(_pi, Mathf.Deg2Rad * 360);
        _seta = Mathf.Clamp(_seta, Mathf.Deg2Rad * (BottomClamp + 90), Mathf.Deg2Rad * (TopClamp + 90));

        Debug.Log(_seta);
        Debug.Log(_pi);

        float x = _interval.z * Mathf.Sin(_seta) * Mathf.Cos(_pi);
        float y = _interval.z * Mathf.Cos(_seta);
        float z = _interval.z * Mathf.Sin(_seta) * Mathf.Sin(_pi);

        // 위치
        transform.position = _player.transform.position + new Vector3(-x, _collider.height + y, z);

        // 각도
        Vector3 lookPos = _player.transform.position + new Vector3(0, _collider.height);
        transform.LookAt(lookPos);
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
}
