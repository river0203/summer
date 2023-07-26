using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    public EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    public Rigidbody enemyRigidBody;

    public NavMeshAgent navmeshAgent;

    public State currentState;
    public CharacterStats currentTarget;
    public bool isPreformingAction;
    public float maximumAttackRange = 1.5f;
    public bool isInteracting;

    public float rotationSpeed = 15;

    [Header("AI Setting")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        _backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        navmeshAgent.enabled = false;
    }

    private void Start()
    {
        navmeshAgent.enabled = false;
        enemyRigidBody.isKinematic = false;
    }

    void Update()
    {
        isInteracting = enemyAnimatorManager._anim.GetBool("isInteracting");
        isPreformingAction = enemyAnimatorManager._anim.GetBool("isPreformingAction");
        enemyAnimatorManager._anim.SetBool("isDead", enemyStats._isDead);
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
        // 네비게이션이 혼자서 튀어 나가는거 막기
        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {
       if(currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if(nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }
}
