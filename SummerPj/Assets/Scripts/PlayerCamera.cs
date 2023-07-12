using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("ī�޶� �ϴ� �ִ� ����")]
    [SerializeField] float BottomClamp = -30.0f;

    [Tooltip("ī�޶� ��� �ִ� ����")]
    [SerializeField] float TopClamp = 70.0f;

    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;

    GameObject CameraTarget;
    GameObject _mainCamera;

    PlayerInputActions _input;

    void Start()
    {
        _input = GameObject.Find("Player").GetComponent<PlayerInputActions>();
        CameraTarget = GameObject.Find("PlayerCameraRoot");
        _mainCamera = GameObject.Find("PlayerCamera");
        _cinemachineTargetYaw = CameraTarget.transform.rotation.eulerAngles.y;
    }

    void LateUpdate()
    {
        CameraRotation();
    }
    void CameraRotation()
    {
        // �Է¿� ���� ī�޶� �̵�
        _cinemachineTargetYaw += _input.look.x;
        _cinemachineTargetPitch += _input.look.y;

        // ī�޶� �ޱ� ����
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0f);
    }
    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
