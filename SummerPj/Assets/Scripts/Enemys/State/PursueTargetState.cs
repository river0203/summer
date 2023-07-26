using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        // ���� �ϰ� ������ ���ڸ��� ���߰� �׳� ���� ���� �����ϴ°� ��õ
        if (enemyManager.isPreformingAction)
        {
            enemyAnimatorManger._anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }
            

        #region �Ǵ��� ������ ����
        // ���� �������� ȭ��ǥ
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        // ���� ���� �Ÿ� ����
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        // ���� ��ġ�� ���� �ٶ󺸴� ���� ������ ����
        float veiwableAngle = Vector3.Angle(targetDirection, transform.forward);
        #endregion

        if (distanceFromTarget > enemyManager.maximumAttackRange)
        {
            enemyAnimatorManger._anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        // ȸ�� �� �̵�
        HandleRotateTarget(enemyManager);

        // ���� ��Ȳ ��õ
        if(distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return combatStanceState;
        }
        else
        {
            return this;
        }
    }

    // �갡 ���ϴ��� Ȯ���ؼ� �ٶ󺸱⸸ ����, ���󰡸鼭 �ٶ�
    private void HandleRotateTarget(EnemyManager enemyManager)
    {
        // ���� �ϰ� �ִٸ� Ÿ���� �ٶ󺸱⸸ ��
        if (enemyManager.isPreformingAction)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position; // ���� ���� ȭ��ǥ
            direction.y = 0; // �����̴� ��
            direction.Normalize(); // ����ȭ

            // Ÿ���� ������ �׳� ���� ��
            if (direction == Vector3.zero) // 0,0,0
            {
                direction = transform.forward;
            }

            // ���������� ���̷��� ��ġ�� �Ĵٺ��� �ڵ�
            // Ÿ�� ������ ȸ������ ���� (������ ����, ȸ�� �� ��)
            Quaternion targetRotation = Quaternion.LookRotation(direction); // ȭ��ǥ�� �ٶ󺸴� ȸ������ �����ϴ� �Լ�

            // ������ ������ ȸ�� (�ٵ� õõ��)
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        // �ƹ��͵� �� �ϰ� ������ Ÿ���� ���󰡸鼭 �ٶ�
        else
        {
            // ���� ������ ���� -> ���÷� ����
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);

            enemyManager.navmeshAgent.enabled = true; // �׺���̼� Ȱ��ȭ

            // ��ġ ��ȯ
            enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position); // ������ ����
            enemyManager.enemyRigidBody.velocity = enemyManager.navmeshAgent.velocity; // ����� �ӵ� ����

            // ȸ�� ��ȯ
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
