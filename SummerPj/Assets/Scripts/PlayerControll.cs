using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float CameraSensitivity; // ī�Ŷ� �ΰ����� ���� ���� 
    [SerializeField]
    private float cameraRotationLimit; // �þư��� �����Ͽ� ī�Ŷ� ȸ���ϴ� ���� ������
    private float currentCameraRotationX = 0f; // ���� �����ִ� ����
    private Vector3 PlyaerPosition;

    [SerializeField]
    private Camera theCamera; //����Ƽ���� �÷��̾ ���ӵ� ī�Ŷ� �־ Ȱ��

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
        // ���� ī�޶� ȸ��
        float _xRotation = Input.GetAxisRaw("Mouse Y"); // ���콺�� 2�����̴�., unity�� 3����
        float _cameraRotationX = _xRotation * CameraSensitivity; // �þ� �������� õõ�� �ϰ�
        currentCameraRotationX -= _cameraRotationX; // -=, �� += �� �� �غ��鼭 ī�Ŷ� ������ Ȯ�� �ʿ� +-�� ���� ���콺 ����
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); // ������ �þư� ���� (����, Min, Max ) ����

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); //localEulerAngles -> transform�� 3���� ��ǥ ��ȭ
    }

    private void CharacterRotation()
    {
        // �¿� ĳ���� ȸ��
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * CameraSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}