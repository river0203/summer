using SG;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    public Rigidbody enemyRigidBody;

    public NavMeshAgent navmeshAgent;

    public State currentState;
            public CharacterStats currentTarget;
    public bool isPreformingAction;
    public float distanceFromTarget;
    public float maximumAttackRange = 1.5f;

    public float rotationSpeed = 15;

    [Header("AI Setting")]
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float viewableAngle;
    public float currentRecoveryTime = 0;


    // Start is called before the first frame update
    void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        navmeshAgent.enabled = false;
    }
    private void Start()
    {
        enemyRigidBody.isKinematic = false;
    }
    // Update is called once per frame
    void Update()
    {
        HandleRecoveryTimer();
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
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

    private void HandleRecoveryTimer()
    {
        Debug.Log(currentRecoveryTime);

        if(currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }
        if(isPreformingAction)
        {
            if(currentRecoveryTime <= 0)
            {
                isPreformingAction = false;
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }
}
