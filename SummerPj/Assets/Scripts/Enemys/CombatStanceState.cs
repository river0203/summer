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

        if(enemyManger.currentRecoveryTime <= 0 && enemyManger.distanceFromTarget <= enemyManger.maximumAttackRange)
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
}
