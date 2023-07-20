using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public CombatStanceState combatStanceState;
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public override State Tick(EnemyManager enemyManger, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        Vector3 targetDirection = enemyManger.currentTarget.transform.position - transform.position;
        float veiwableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManger.isPreformingAction)
            return combatStanceState;
        
        if(currentAttack != null)
        {
            if(enemyManger.distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
            {
                return this;
            }
            else if(enemyManger.distanceFromTarget < currentAttack.maximumAttackAngle)
            {
                if(enemyManger.viewableAngle <= currentAttack.maximumAttackAngle && enemyManger.viewableAngle >= currentAttack.minimumAttackAngle)
                {
                    if (enemyManger.currentRecoveryTime <= 0 && enemyManger.isPreformingAction == false)
                    {
                        enemyAnimatorManger._anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManger._anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManger.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyManger.isPreformingAction = true;
                        enemyManger.currentRecoveryTime = currentAttack.recoveryTime;
                        currentAttack = null;
                        return combatStanceState;
                    }
                }
            }
        }
        else
        {
            GetNewAttack(enemyManger);
        }

        return combatStanceState;
    }
    void GetNewAttack(EnemyManager enemyManger)
    {
        Vector3 targetDirection = enemyManger.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        enemyManger.distanceFromTarget = Vector3.Distance(enemyManger.currentTarget.transform.position, transform.position);

        int maxScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];
            if (enemyManger.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && enemyManger.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemyManger.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && enemyManger.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (currentAttack != null)
                    {
                        return;
                    }
                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        currentAttack = enemyAttackAction;
                    }
                }
            }
        }
    }
}
