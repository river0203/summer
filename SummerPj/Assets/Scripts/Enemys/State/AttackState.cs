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
        float distanceFromTarget = Vector3.Distance(enemyManger.currentTarget.transform.position, enemyManger.transform.position); ;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);



        // 뭘 하고 있으면 공격하지 말고 돌아가라
        if (enemyManger.isPreformingAction)
            return combatStanceState;

        // 현재 공격한게 있으면
        if (currentAttack != null)
        {
            // 너무 가까워서 공격 범위가 벗어나면 공격 멈추고 다시 생각함
            if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                return this;
            //사거리 안에 들어오면
            else if (distanceFromTarget < currentAttack.maximumAttackAngle)
            {
                //시아각 내에 들어오면
                if (viewableAngle <= currentAttack.maximumAttackAngle
                    && viewableAngle >= currentAttack.minimumAttackAngle)
                {

                    if (!enemyManger.isPreformingAction && enemyManger.isPreformingAction == false)
                    {
                        enemyAnimatorManger._anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManger._anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManger.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyAnimatorManger._anim.SetBool("isPreformingAction", true);
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

    // 조건에 맞는 공격 정보를 뽑아오는 함수
    void GetNewAttack(EnemyManager enemyManger)
    {
        #region 공격에 참고할 수치 가져오기
        // 나와 적을 가로지르는 화살표
        Vector3 targetDirection = enemyManger.currentTarget.transform.position - transform.position;
        // 적과 나의 거리
        float distanceFromTarget = Vector3.Distance(enemyManger.currentTarget.transform.position, enemyManger.transform.position);
        // 자신이 보는 방향과 타겟의 위치간의 각도
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        #endregion


        #region 최대 점수를 정함
        int maxScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            // 거리 조건에 맞고
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                // 각도 조건에 맞으면
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    // 조건에 맞는 공격들의 점수들을 모음 
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }
        #endregion

        #region 렌덤을 렌덤으로 뽑음
        // 점수중에 숫자 하나를 뽑음
        int randomValue = Random.Range(0, maxScore);
        // 임시 점수?
        int temporaryScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            // 거리 조건이 맞고
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                // 각도 조건이 맞으면
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    // 방금 공격한게 있으면 아무것도 안 함
                    if (currentAttack != null)
                    {
                        // 아무것도 안 하고
                        return;
                    }

                    // 임시 점수에 점수 추가
                    temporaryScore += enemyAttackAction.attackScore;

                    // 임시 점수가 랜덤 점수보다 높으면
                    if (temporaryScore > randomValue)
                    {
                        // 지금 공격을 함
                        currentAttack = enemyAttackAction;
                    }
                }
            }
        }
        #endregion

    }
}
