using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent _navmeshAgent;
    [HideInInspector] public EnemyStats _enemyStats;
    [HideInInspector] public Rigidbody _enemyRigidBody;

    EnemyAnimatorManager _enemyAnimatorManager;
    CharacterStatsManager _characterState;

    public Transform _lockOnTransform;
    public State _currentState;
    public CharacterStatsManager _currentTarget;
    public bool isPreformingAction;

    [Header("AI Setting")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 10;
    public float minimumDetectionAngle = -10;
    public float maximumAttackRange = 1.5f;
    public float rotationSpeed = 3;

    public LayerMask _MobLayer;

    void Awake()
    {   
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _enemyStats = GetComponent<EnemyStats>();
        _enemyRigidBody = GetComponent<Rigidbody>();
        _navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        _characterState = GetComponent<CharacterStatsManager>();
    }

    private void Start()
    {
        _navmeshAgent.enabled = false;
    }

    void Update()
    {
        isPreformingAction = _enemyAnimatorManager._anim.GetBool("isPreformingAction");
        _enemyAnimatorManager._anim.SetBool("isDead", _enemyStats._isDead);
        _enemyRigidBody.velocity = Vector3.zero;
        
        LookTarget();
    }

    private void FixedUpdate()
    {
        HandleStateMachine();

        // 네비게이션이 혼자서 튀어 나가는거 막기
        _navmeshAgent.transform.localPosition = Vector3.zero;
        _navmeshAgent.transform.localRotation = Quaternion.identity;
    }

    // 적 스테이트 관리
    public void HandleStateMachine()
    {
       if(_currentState != null)
        {
            State nextState = _currentState.Tick(this, _enemyStats, _enemyAnimatorManager);

            if(nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        _currentState = state;
    }

    // 공격중에 적이 회전하도록 하는 코드
    private void LookTarget()
    {
        if(isPreformingAction)
        {
            if (!_characterState._isDead && _currentTarget != null)
            {
                Vector3 _targetDirection = _currentTarget.transform.position - this.transform.position;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_targetDirection), rotationSpeed * Time.deltaTime);
            }
        }
    }

}
