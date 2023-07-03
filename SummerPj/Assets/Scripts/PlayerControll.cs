using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float CameraSensitivity; // 카매라 민감도에 대한 변수 
    [SerializeField]
    private float cameraRotationLimit; // 시아각을 제한하여 카매라가 회전하는 것을 막아줌
    private float currentCameraRotationX = 0f; // 지금 보고있는 각도
    private Vector3 PlyaerPosition;

    [SerializeField]
    private Camera theCamera; //유니티에서 플레이어에 종속된 카매라를 넣어서 활용

    private Rigidbody myRigid;

    void Start()
    {

        myRigid = GetComponent<Rigidbody>();
        PlyaerPosition = this.gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void Move()
    {

        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");


        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CameraRotation()
    {
        // 상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y"); // 마우스는 2차원이다., unity는 3차원
        float _cameraRotationX = _xRotation * CameraSensitivity; // 시아 움직임을 천천히 하게
        currentCameraRotationX -= _cameraRotationX; // -=, 와 += 둘 다 해보면서 카매라 움직임 확인 필요 +-에 따라 마우스 반전
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); // 범위내 시아각 차단 (변수, Min, Max ) 형태

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); //localEulerAngles -> transform의 3차원 좌표 변화
    }

    private void CharacterRotation()
    {
        // 좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * CameraSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}