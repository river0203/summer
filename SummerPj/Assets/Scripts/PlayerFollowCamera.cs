using UnityEngine;
using Cinemachine;

public class PlayerFollowCamera : MonoBehaviour
{
    CinemachineVirtualCamera vCam;
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        vCam.Follow = GameObject.Find("PlayerCameraRoot").transform;
    }
}
