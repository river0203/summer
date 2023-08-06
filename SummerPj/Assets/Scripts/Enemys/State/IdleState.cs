using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public PursueTargetState pursueTargetState;
    public LayerMask detectionLayer;
    private EventColliderBeginBossFight _eventCollider;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {

        #region �� ����
        // ���� �ȿ� ���� ����(�迭�� ����)
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        // ����
        
        // ���� �ȿ� ��� ������Ʈ��
        for (int i = 0; i < colliders.Length; i++)
        {
            // CharacterStats�� ������ (ĳ�������� �ƴ��� �Ǻ�)
            CharacterStatsManager characterState = colliders[i].transform.GetComponent<CharacterStatsManager>();

            // -- 
            if (characterState != null)
            {
                // �÷��̾���� ĳ���ͱ��� ����Ǵ� ȭ��ǥ
                Vector3 targetDirection = characterState.transform.position - transform.position;
                // ī�޶�� ĳ������ ����
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                // �� ������ �ִ�ġ�� �ּ�ġ�� ����
                if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    // ���� Ÿ���� ���� (���� �ȿ� �ְ�, ĳ���� ������ ������ �ְ�(ĳ���� �̰�), ������ ���� ���� ĳ����)
                    enemyManager.currentTarget = characterState;
                }

            }
        }
        #endregion

        #region �� ���� ������ ���� ���� ���� ��õ
        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
        #endregion

    }
}