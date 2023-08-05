using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotionManager : MonoBehaviour
{
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;

    public CapsuleCollider _characterCollider;
    public CapsuleCollider _characterCollisionBlockerCollider;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
    }

    private void Start()
    {
        Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);
    }

}