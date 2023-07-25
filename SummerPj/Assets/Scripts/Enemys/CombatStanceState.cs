using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState; 
    public override State Tick(EnemyManager enemyManger, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        enemyManger.distanceFromTarget = Vector3.Distance(enemyManger.currentTarget.transform.position, enemyManger.transform.position);

        HandleRotateTarget(enemyManger);
        if (enemyManger.currentRecoveryTime <= 0 && enemyManger.distanceFromTarget <= enemyManger.maximumAttackRange)
        {
            return attackState;
        }
        else if(enemyManger.distanceFromTarget > enemyManger.maximumAttackRange)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }

    private void HandleRotateTarget(EnemyManager enemyManger)
    {
        if (enemyManger.isPreformingAction)
        {
            Vector3 direction = enemyManger.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManger.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManger.rotationSpeed / Time.deltaTime);

        }
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManger.navmeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManger.navmeshAgent.velocity;

            enemyManger.navmeshAgent.enabled = true;
            enemyManger.navmeshAgent.SetDestination(enemyManger.currentTarget.transform.position);
            enemyManger.enemyRigidBody.velocity = targetVelocity;
            enemyManger.transform.rotation = Quaternion.Slerp(transform.rotation, enemyManger.navmeshAgent.transform.rotation, enemyManger.rotationSpeed / Time.deltaTime);
        }

    }
}
