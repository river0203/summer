using System.Collections;
using UnityEngine;

public class AmbushState : State
{
    public float detectionRadius = 2;
    public LayerMask detectionLayer;
    public PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManger, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        // 범위 안의 플레이어 감지
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
                    // 감지가 되면
                    enemyAnimatorManger._anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    enemyAnimatorManger._anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                    // StartCoroutine(WaitSeconds());
                    
                    enemyManger._currentTarget = characterStats; // 타겟 설정
                }
            }
        }

        // 타겟이 있으면 추격 시작
        if (enemyManger._currentTarget != null && !enemyAnimatorManger._anim.GetBool("isPreformingAction"))
            return pursueTargetState;
        else
            return this;
    }
}
