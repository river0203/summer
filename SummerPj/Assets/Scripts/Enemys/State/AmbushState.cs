using System.Collections;
using UnityEngine;

public class AmbushState : State
{
    public float detectionRadius = 2;
    public LayerMask detectionLayer;
    public PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManger, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        // ���� ���� �÷��̾� ����
        Collider[] colliders = Physics.OverlapSphere(enemyManger.transform.position, detectionRadius, detectionLayer);

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

            if(characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - enemyManger.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, enemyManger.transform.forward);

                if(viewableAngle > enemyManger.minimumDetectionAngle 
                    && viewableAngle < enemyManger.maximumDetectionAngle)
                {
                    // ������ �Ǹ�
                    enemyAnimatorManger._anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    enemyAnimatorManger._anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                    // StartCoroutine(WaitSeconds());
                    
                    enemyManger._currentTarget = characterStats; // Ÿ�� ����
                }
            }
        }

        // Ÿ���� ������ �߰� ����
        if (enemyManger._currentTarget != null && !enemyAnimatorManger._anim.GetBool("isPreformingAction"))
            return pursueTargetState;
        else
            return this;
    }
}
