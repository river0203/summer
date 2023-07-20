using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;
    public override State Tick(EnemyManager enemyManger, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        if (enemyManger.isPreformingAction)
        {
            return this;
        }

        Vector3 targetDirection = enemyManger.currentTarget.transform.position - transform.position;
        enemyManger.distanceFromTarget = Vector3.Distance(enemyManger.currentTarget.transform.position, transform.position);
        float veiwableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManger.distanceFromTarget > enemyManger.maximumAttackRange)
        {
            enemyAnimatorManger._anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        HandleRotateTarget(enemyManger);

        enemyManger.navmeshAgent.transform.localPosition = Vector3.zero;
        enemyManger.navmeshAgent.transform.localRotation = Quaternion.identity;

        if(enemyManger.distanceFromTarget <= enemyManger.maximumAttackRange)
        {
            return combatStanceState;
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
