using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;

    public Rigidbody enemyRigidBody;
    public NavMeshAgent navMeshAgent;
    public CharacterStats currentTarget;

    public State currentState;
    public bool isPreformingAction;
    public bool isInteracting;
    public float distanceFromTarget;
    public float rotationSpeed = 15f;
    public float maximumAttackRange = 1.5f;



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
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = false;
        enemyRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        enemyRigidBody.isKinematic = false; //¹°¸® x
    }
    // Update is called once per frame
    void Update()
    {
        HandleRecoveryTimer();

        isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
    }

    private void FixedUpdate()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if (currentState != null)
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

    private void HandleRecoveryTimer()
    {
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
}
