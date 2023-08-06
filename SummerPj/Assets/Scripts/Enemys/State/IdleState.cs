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

        #region 적 감지
        // 범위 안에 모든걸 감지(배열로 저장)
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        // 가공
        
        // 범위 안에 모든 오브젝트의
        for (int i = 0; i < colliders.Length; i++)
        {
            // CharacterStats를 가져옴 (캐릭터인지 아닌지 판별)
            CharacterStatsManager characterState = colliders[i].transform.GetComponent<CharacterStatsManager>();

            // -- 
            if (characterState != null)
            {
                // 플레이어부터 캐릭터까지 연결되는 화살표
                Vector3 targetDirection = characterState.transform.position - transform.position;
                // 카메라랑 캐릭터의 각도
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                // 그 각도의 최대치랑 최소치를 제한
                if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    // 현제 타겟을 설정 (범위 안에 있고, 캐릭터 스탯을 가지고 있고(캐릭터 이고), 설정한 각도 안의 캐릭터)
                    enemyManager.currentTarget = characterState;
                }

            }
        }
        #endregion

        #region 적 감지 유무에 따른 다음 상태 추천
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