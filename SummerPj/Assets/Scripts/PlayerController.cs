using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerStates;

public class PlayerController : MonoBehaviour
{
    private Gamepad gamepad;

    PlayerInputActions _input;
    CharacterController _controller;
    PlayerInput _playerInput;
    Animator _anim;

    State _playerState = State.Idle;
    State PlayerState
    {
        get { return _playerState; }
        set
        {
            if (_playerState == value)
                return;

            _playerState = value;
            string currentState = Enum.GetName(typeof(State), _playerState);
            _anim.CrossFade(currentState, 0.1f);
        }
    }

    #region  Status
    [Header("Status")]
    [Tooltip("플레이어 최대 체력")]
    [SerializeField] public float maxHP = 200;

    [Tooltip("플레이어 최대 스태미나")]
    [SerializeField] public float maxStamina = 200;

    public float _hp;
    public float _stamina;
    #endregion

    #region  Move
    [Header("Move")]
    [Tooltip("방향 전환 시간")]
    [SerializeField] float RotationSmoothTime = 0.12f;

    Quaternion _playerRotation;
    float _targetRotation;
    float _rotationVelocity;
    float _verticalVelocity;
    #endregion

    #region  Jump
    [Header("Jump")]
    [Tooltip("점프 재사용 대기시간")]
    [SerializeField] float JumpTimeout = 0.5f;

    [Tooltip("낙하 상태에 도달하기까지 시간")]
    [SerializeField] float FallTimeout = 0.15f;

    [Tooltip("점프 높이")]
    [SerializeField] float JumpHeight = 1.2f;

    [Tooltip("캐릭터 자체 중력")]
    [SerializeField] float _Gravity = -15f;

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
    //[Header("Dodge")]
    #endregion

    #region  Attack
    [Header("공격")]
    [Tooltip("콤보 지속 시간")]
    [SerializeField] float comboTime = 3f;

    int Combo = 0;
    float comboTimeDelta = 0f;
    #endregion

    #region  Heal
    [Header("회복")]
    [Tooltip("회복 아이템 수치")]
    [SerializeField] float HealAmount = 30;

    [Tooltip("회복 아이템 개수")]
    [SerializeField] float Heal_Count = 5;
    #endregion

    #region  Ultimate
    [Header("Ultimate")]
    [Tooltip("최대 충전량")]
    float Ult_Gage;

    float Ult;
    #endregion

    private void Start()
    { 
        _hp = maxHP;
        _stamina = maxStamina;
        gamepad = Gamepad.current;

        _anim = GetComponent<Animator>();
        _input = GetComponent<PlayerInputActions>();
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;

        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }
    private void Update()
    {

        #region  Vibration
        /*        if (gamepad != null && gamepad.device != null)
                {
                    // 왼쪽 모터의 진동 설정 (0.0 ~ 1.0 사이 값)
                    float leftVibration = 0.5f;

                    // 오른쪽 모터의 진동 설정 (0.0 ~ 1.0 사이 값)
                    float rightVibration = 0.8f;

                    // 진동 적용
                    gamepad.SetMotorSpeeds(leftVibration, rightVibration);
                }*/
        #endregion
        // 중력
        Gravity();
        // 바닥 탐지
        GroundedCheck();
        // 플레이어 회전
        moveRotation();
        // 콤보 관리
        Combo_Manage();

        if (PlayerState == State.Idle || PlayerState == State.Walk || PlayerState == State.Sprint)
        {
            // 이동
            Move();
            // 약공격
            WeakAttack();
            // 강공격
            StrongAttack();
            // 구르기 방향 결정
            Dodge();
            // 점프
            Jump();
            // 회복
            Heal();
            // 궁극기
            Ultimate();
            // 상호작용, 흘리기, 기모으기
        }

        // 점프 중 행동
        if (PlayerState == State.Jump || PlayerState == State.Fall)
        {
            JumpAttack();
        }

        // 착지
        if (PlayerState == State.Fall || PlayerState == State.Jump)
        {
            if (_verticalVelocity < 0 && Grounded)
            {
                StopCoroutine("JumpState");
                PlayerState = State.Land;
                StartCoroutine(ChangeState());
            }
        }
    }

    void Ultimate()
    {
        if (_input.ultimate)
        {
            PlayerState = State.Ultimate;
        }
        else { Ult = 0; }
    }
    void Heal()
    {
        if (_input.heal)
        {
            PlayerState = State.Heal;
            StartCoroutine(ChangeState());

            _input.heal = false;
        }
        else _input.heal = false;
    }
    void JumpAttack()
    {
        if (_input.weakAttack || _input.strongAttack)
        {
            PlayerState = State.JumpAttack;
            StartCoroutine(ChangeState());

            _input.weakAttack = false;
            _input.strongAttack = false;
        }
        else { _input.weakAttack = false; _input.strongAttack = false; }
    }
    void Jump()
    {
        if (_input.jump && _jumpTimeoutDelta <= 0.0f)
        {
            _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * _Gravity);
            PlayerState = State.Jump;
            StartCoroutine("JumpState");

            _input.jump = false;
        }
        else _input.jump = false;

    }
    void moveRotation()
    {
        // 플레이어 회전
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        if (_input.move != Vector2.zero && (PlayerState != State.Dodge))
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // 특정 상황 플레이어 회전 제한
        if (PlayerState != State.WeakAttack_1 && PlayerState != State.WeakAttack_2 && PlayerState != State.WeakAttack_3 &&
        PlayerState != State.WeakAttack_4 && PlayerState != State.WeakAttack_5 && PlayerState != State.WeakAttack_6 &&
        PlayerState != State.StrongAttack && PlayerState != State.Jump && PlayerState != State.JumpAttack && PlayerState != State.Dodge && PlayerState != State.Land)
        {
            _playerRotation = transform.rotation;
        }
        else transform.rotation = _playerRotation;
    }
    void WeakAttack()
    {
        // 공격 및 콤보 관리
        if (_input.weakAttack)
        {
            //콤보에 따라 플레이어 스테이트 변경
            switch (Combo)
            {
                case 0: PlayerState = State.WeakAttack_1; break;
                case 1: PlayerState = State.WeakAttack_2; break;
                case 2: PlayerState = State.WeakAttack_3; break;
                default: break;
            }
            StartCoroutine(AttackState());
            StartCoroutine(ResetComboTime());
            Combo++;

            _input.weakAttack = false;
        }
        else _input.weakAttack = false;
    }
    void StrongAttack()
    {
        if (_input.strongAttack)
        {
            PlayerState = State.StrongAttack;
            StartCoroutine(AttackState());

            _input.strongAttack = false;
        }
        else _input.weakAttack = false;
    }
    void Dodge()
    {
        if (_input.dodge)
        {
            PlayerState = State.Dodge;
            StartCoroutine(ChangeState());

            _input.dodge = false;
        }
        else _input.dodge = false;
    }
    void Move()
    {
        // 걷거나 뛰는 상태에서 입력이 없을 시 Idle
        if (PlayerState == State.Walk || PlayerState == State.Sprint)
        {
            if (_input.move == Vector2.zero) PlayerState = State.Idle;
        }
        // Idle상태에서 Sprint를 누르고 이동하면 Sprint
        else if (PlayerState == State.Idle && _input.move != Vector2.zero && _input.sprint) PlayerState = State.Sprint;
        // Idle상태에서 이동하면 Walk
        else if (PlayerState == State.Idle && _input.move != Vector2.zero) PlayerState = State.Walk;

        // 걷는 상태에서 Sprint를 누르면 달리기
        if (PlayerState == State.Walk) if (_input.sprint) PlayerState = State.Sprint;
    }
    void Gravity()
    {
        if (Grounded)
        {
            _fallTimeoutDelta = FallTimeout;

            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
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
            _verticalVelocity += _Gravity * Time.deltaTime;
        }

        _controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    void Combo_Manage()
    {
        // 공격중이 아닐 때 콤보 지속 시간 감소
        if ((PlayerState != State.WeakAttack_1) && (PlayerState != State.WeakAttack_2) && (PlayerState != State.WeakAttack_3) && (comboTimeDelta > 0))
        {
            comboTimeDelta -= Time.deltaTime;
        }

        // 최대 콤보 초과시 or 콤보 지속 시간 초과시 0로 초기화
        if (comboTimeDelta < 0 || Combo > 2) Combo = 0;
    }
    void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }
    
    IEnumerator ChangeState()
    {
        if (PlayerState == State.Heal || PlayerState == State.Heal_Walk)
        {
            yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length);
            _hp += HealAmount;
            Heal_Count--;
        }
        else if(PlayerState == State.Land)
        {
            yield return new WaitForSeconds(0.833f);

        }
        else yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length);


        PlayerState = State.Idle;
        yield break;
    }
    IEnumerator ResetComboTime()
    {
        // 공격 애니메이션일 경우 애니메이션이 끝나면 콤보 시간 활성화
        yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length);
        comboTimeDelta = comboTime;
        yield break;
    }
    IEnumerator AttackState()
    {
        if (PlayerState == State.StrongAttack)
        {
            yield return new WaitForSeconds(2.267f);
        }
        else yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length * 0.6f);
        PlayerState = State.Idle;
    }
    IEnumerator JumpState()
    {
        yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length);
        PlayerState = State.Fall;
    }

    //피격
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyAttack")
        {
            //플레이어 체력 감소
            // _hp -= 

            // 플레이어 피격 상태
            if (_hp > 0)
            {
                PlayerState = State.Damaged;
                if (PlayerState != State.Jump && PlayerState != State.Dodge && PlayerState != State.Fall && PlayerState != State.Land)
                {
                    StopAllCoroutines();
                }
                StartCoroutine(ChangeState());
            }
            // 플레이어 사망
            else PlayerState = State.Dead;
        }
    }
}