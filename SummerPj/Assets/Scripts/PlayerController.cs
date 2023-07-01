using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions _input; 
    CharacterController _controller;
    PlayerInput _playerInput; 
    Animator _animator;

    public GameObject CameraTarget;
    public GameObject _mainCamera;
    
    #region  Status
    [Header("Status")]
    public float MaxHp = 100;
    public float MaxStamina = 50;

    float _hp;
    float _stamina;
    #endregion

    #region  Camera
    [Header("Camera")]
    [Tooltip("카메라 하단 최대 범위")]
    [SerializeField]float BottomClamp = -30.0f;

    [Tooltip("카메라 상단 최대 범위")]
    [SerializeField]float TopClamp = 70.0f;

    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;
    #endregion

    #region  Move
    [Header("Move")]
    [Tooltip("현재 이동 속도")]
    [SerializeField]float Speed;

    [Tooltip("기본 이동 속도 m/s")]
    [SerializeField]float MoveSpeed = 2f;

    [Tooltip("달리기 속도 m/s")]
    [SerializeField]float SprintSpeed = 5f;

    [Tooltip("변경 이동속도 도달 시간")]
    [SerializeField]float SpeedChangeRate = 10.0f;

    [Tooltip("방향 전환 시간")]
    [SerializeField]float RotationSmoothTime = 0.12f;
    

    float _targetRotation;
    float _rotationVelocity;
    float _verticalVelocity;
    #endregion

    #region  Jump
    [Header("Jump")]
    [Tooltip("점프 재사용 대기시간")]
    [SerializeField]float JumpTimeout = 0.5f;

    [Tooltip("낙하 상태에 도달하기까지 시간")]
    [SerializeField] float FallTimeout = 0.15f;

    [Tooltip("점프 높이")]
    [SerializeField] float JumpHeight = 1.2f;

    [Tooltip("캐릭터 자체 중력")]
    [SerializeField] float Gravity = -15f;

    [Tooltip("바닥으로 사용할 레이어")]
    [SerializeField] LayerMask GroundLayers;
    
    bool Grounded = true;
    float GroundedOffset = -0.14f;
    float GroundedRadius = 0.28f;
    
    float _fallTimeoutDelta;
    float _jumpTimeoutDelta;
    float _terminalVelocity = 53.0f;

    #endregion

    #region  Dodge
    [Header("Dodge")]
    [Tooltip("구르기 지속시간")]
    [SerializeField]float dodgeTime = 3f;

    [Tooltip("구르기 쿨타임")]
    [SerializeField]float DodgeCoolDown = 10f;

    [Tooltip("구르기 속도 증가 시간")]
    [SerializeField] float DodgeLerpTime = 3f;
 
    [Tooltip("구르기 시작 속도")]
    float DodgeStartSpeed = 8f;
    
    [Tooltip("구르기 목표 속도")]
    float DodgetargetSpeed = 8f;
    
    float _dodgeCoolDownDelta;
    float DodgeSpeedChangeRate;
    float DodgecurrentTime;
    float DodgeSpeed;
    Vector3 dodgeTargetdirection;
    #endregion
    
    private void Awake()
    {
        if(DodgeLerpTime > dodgeTime)
        {
            DodgeLerpTime = dodgeTime;
        }
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<PlayerInputActions>();
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;

        _cinemachineTargetYaw = CameraTarget.transform.rotation.eulerAngles.y;
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        _hp = MaxHp;
        _stamina = MaxStamina;
    }
    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
        Dodge();
        Debug.Log(Grounded);
    }
    private void LateUpdate()
    {
        CameraRotation();
    }

    void Dodge()
    {
        if (!_input.dodge) { dodgeTargetdirection = Move(); _dodgeCoolDownDelta -= Time.deltaTime; }
            
        if (_input.dodge) 
        {
            if (Grounded && _dodgeCoolDownDelta < 0.0f)
            {
                _controller.Move(dodgeTargetdirection.normalized * (DodgeSpeed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

                DodgecurrentTime += Time.deltaTime;

                if (DodgecurrentTime >= DodgeLerpTime)
                {
                    DodgecurrentTime = DodgeLerpTime;
                }
                DodgeSpeedChangeRate = DodgecurrentTime / DodgeLerpTime;

                DodgeSpeed = Mathf.Lerp(DodgeStartSpeed, DodgetargetSpeed, DodgeSpeedChangeRate);
                DodgeSpeed = Mathf.Round(DodgeSpeed * 1000f) / 1000f;
                StartCoroutine(StopDodge());
            }
            else { _input.dodge = false; return; }
        }
    }
    Vector3 Move()
    {
        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);
            Speed = Mathf.Round(Speed * 1000f) / 1000f;
        }
        else Speed = targetSpeed;

        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        if(!_input.dodge)_controller.Move(targetDirection.normalized * (Speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        return targetDirection;
    }
    void JumpAndGravity()
    {
        if (Grounded)
        {
            _fallTimeoutDelta = FallTimeout;

            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if (_input.jump && _jumpTimeoutDelta <= 0.0f && !_input.dodge)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeoutDelta = JumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }

            _input.jump = false;
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
    void CameraRotation()
    {
        _cinemachineTargetYaw += _input.look.x;
        _cinemachineTargetPitch += _input.look.y;

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0f);
    }

    void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }
    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    IEnumerator StopDodge()
    {
        yield return new WaitForSeconds(dodgeTime);
        _input.dodge = false;
        _dodgeCoolDownDelta = DodgeCoolDown;
        yield break;
    }
}
