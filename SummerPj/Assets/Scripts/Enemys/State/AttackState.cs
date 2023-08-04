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



        // �� �ϰ� ������ �������� ���� ���ư���
        if (enemyManger.isPreformingAction)
            return combatStanceState;

        // ���� �����Ѱ� ������
        if (currentAttack != null)
        {
            // �ʹ� ������� ���� ������ ����� ���� ���߰� �ٽ� ������
            if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                return this;
            //��Ÿ� �ȿ� ������
            else if (distanceFromTarget < currentAttack.maximumAttackAngle)
            {
                //�þư� ���� ������
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

    // ���ǿ� �´� ���� ������ �̾ƿ��� �Լ�
    void GetNewAttack(EnemyManager enemyManger)
    {
        #region ���ݿ� ������ ��ġ ��������
        // ���� ���� ���������� ȭ��ǥ
        Vector3 targetDirection = enemyManger.currentTarget.transform.position - transform.position;
        // ���� ���� �Ÿ�
        float distanceFromTarget = Vector3.Distance(enemyManger.currentTarget.transform.position, enemyManger.transform.position);
        // �ڽ��� ���� ����� Ÿ���� ��ġ���� ����
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        #endregion


        #region �ִ� ������ ����
        int maxScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            // �Ÿ� ���ǿ� �°�
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                // ���� ���ǿ� ������
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    // ���ǿ� �´� ���ݵ��� �������� ���� 
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }
        #endregion

        #region ������ �������� ����
        // �����߿� ���� �ϳ��� ����
        int randomValue = Random.Range(0, maxScore);
        // �ӽ� ����?
        int temporaryScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            // �Ÿ� ������ �°�
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                // ���� ������ ������
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    // ��� �����Ѱ� ������ �ƹ��͵� �� ��
                    if (currentAttack != null)
                    {
                        // �ƹ��͵� �� �ϰ�
                        return;
                    }

                    // �ӽ� ������ ���� �߰�
                    temporaryScore += enemyAttackAction.attackScore;

                    // �ӽ� ������ ���� �������� ������
                    if (temporaryScore > randomValue)
                    {
                        // ���� ������ ��
                        currentAttack = enemyAttackAction;
                    }
                }
            }
        }
        #endregion

    }
}
