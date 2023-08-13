using UnityEngine;

// ���� ���� �����϶� ��� ���� �Ǵ�
public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState; 
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        // �÷��̾���� �Ÿ� ����
        float distanceFromTarget = Vector3.Distance(enemyManager._currentTarget.transform.position, enemyManager.transform.position);

        // ���� ���� �ϰ� �־����� �̼��� 0���� �о���� (�����ϰ� �޸��� �ִϸ��̼����� �Ѿ�°� ����)
        if (enemyManager.isPreformingAction)
        {
            enemyAnimatorManger._anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        }

        if (!enemyManager.isPreformingAction// ���� ���� üũ
            && distanceFromTarget <= enemyManager.maximumAttackRange) // ���� ���� ���� üũ
        {
            return attackState; // �׶� ���� ��õ
        }
        else if(distanceFromTarget > enemyManager.maximumAttackRange) // ���� ���� ������ ���
        {
            return pursueTargetState; // �ٽ� �Ѵ°� ��õ
        }
        else
        {
            return this; // ���ߴ°� ��õ
        }
    }
}
