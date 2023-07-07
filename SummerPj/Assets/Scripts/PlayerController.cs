using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerState;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions _input; 
    CharacterController _controller;
    PlayerInput _playerInput; 
    Animator _animator;

    public GameObject CameraTarget;
    public GameObject _mainCamera;
    State _playerState = State.Idle;

    #region  Status
    [Header("Status")]
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
/*    [Tooltip("현재 이동 속도")]
    [SerializeField]float Speed;

    [Tooltip("기본 이동 속도 m/s")]
    [SerializeField]float MoveSpeed = 2f;

    [Tooltip("달리기 속도 m/s")]
    [SerializeField]float SprintSpeed = 5f;

    [Tooltip("변경 이동속도 도달 시간")]
    [SerializeField]float SpeedChangeRate = 10.0f;*/

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

    #region  Attack
    [Header("공격")]
    [Tooltip("콤보 지속 시간")]
    [SerializeField] float comboTime = 3f;

    [Tooltip("최대 콤보")]
    [SerializeField] int Combo = 0;

    float comboTimeDelta;
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
    }
    private void Update()
    {
        Debug.Log(_playerState.ToString());
        // 점프 및 중력
        JumpAndGravity();
        // 바닥 탐지
        GroundedCheck();

        //이동 방향 결정
        Move();
        
        //구르기 방향 결정
        Dodge();

        //약공격
        WeakAttack();
        //강공격
        StrongAttack();
    } 
    private void LateUpdate()
    {
        CameraRotation();
    }

    void WeakAttack()
    {
        // 공격 및 콤보 관리
        if (_input.weakAttack)
        {
            //Idle, Walk, Sprint 상태일 때만 공격 가능
            if (_playerState == State.Idle || _playerState == State.Walk || _playerState == State.Sprint)
            {
                //콤보에 따라 플레이어 스테이트 변경
                switch (Combo)
                {
                    case 0: _playerState = State.WeakAttack_1; break;
                    case 1: _playerState = State.WeakAttack_2; break;
                    case 2: _playerState = State.WeakAttack_3; break;
                    default: break;
                }
                Combo++;
                StartCoroutine(ChangeState());
                StartCoroutine(ResetComboTime());
            }
            _input.weakAttack = false;
        }

        // 공격중이 아닐 때 콤보 지속 시간 감소
        if ((_playerState != State.WeakAttack_1) && (_playerState != State.WeakAttack_2) && (_playerState != State.WeakAttack_3) && (comboTimeDelta > 0))
        {
            comboTimeDelta -= Time.deltaTime;
        }

        // 최대 콤보 초과시 or 콤보 지속 시간 초과시 0로 초기화
        if (comboTimeDelta <= 0 || Combo > 3) Combo = 0;
    }
    void StrongAttack()
    {
        if (_input.strongAttack)
        {
            if (_playerState == State.Idle || _playerState == State.Walk || _playerState == State.Sprint) _playerState = State.StrongAttack;
            StartCoroutine(ChangeState());

            _input.strongAttack = false;
        }
    }
    void Dodge()
    {
        if (!_input.dodge) { dodgeTargetdirection = Move(); _dodgeCoolDownDelta -= Time.deltaTime; }
            
        if (_input.dodge) 
        {
            if (Grounded && _dodgeCoolDownDelta < 0.0f)
            {
            }
            else { _input.dodge = false; return; }
        }
    }
    Vector3 Move()
    {
        /*        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
                if (_input.move == Vector2.zero) targetSpeed = 0.0f;

                float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

                float speedOffset = 0.1f;

                if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);
                    Speed = Mathf.Round(Speed * 1000f) / 1000f;
                }
                else Speed = targetSpeed;*/
        if (_playerState == State.Idle && _input.move != Vector2.zero && _input.sprint) _playerState = State.Sprint;
        else if (_playerState == State.Idle && _input.move != Vector2.zero) _playerState = State.Walk; 
            
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        //if(!_input.dodge)_controller.Move(targetDirection.normalized * (Speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

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

            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                //_playerState = State.Jump;
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
        // 입력에 따라 카메라 이동
        _cinemachineTargetYaw += _input.look.x;
        _cinemachineTargetPitch += _input.look.y;

        // 카메라 앵글 제한
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

    IEnumerator ChangeState()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).length >= 1)
        {
            _playerState = State.Idle;
        }
        yield break;
    }

    IEnumerator ResetComboTime()
    {
        // 공격 애니메이션일 경우 애니메이션이 끝나면 콤보 시간 활성화
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("WeakAttack_1") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("WeakAttack_1") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("WeakAttack_3"))
        {
            if(_animator.GetCurrentAnimatorStateInfo(0).length >= 1f)comboTimeDelta = comboTime;
        }
        yield break;
    }
}
