using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : State
{
    private EventColliderBeginBossFight _eventCollider;

    public bool isSleeping;
    public float detectionRadius = 2;
    public string sleepAnimation;
    public string wakeAnimation;

    public LayerMask detectionLayer;

    public PursueTargetState pursueTargetState; 

    public override State Tick(EnemyManager enemyManger, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {

        // 자고 있으면 자는 애니메이션 실행
        if(isSleeping && enemyManger.isInteracting == false)
            enemyAnimatorManger.PlayTargetAnimation(sleepAnimation, true);

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
                    enemyAnimatorManger._anim.SetBool("isPreformingAction", true);
                    enemyAnimatorManger.PlayTargetAnimation(wakeAnimation, true);
                    StartCoroutine(WaitSeconds());

                    enemyManger.currentTarget = characterStats; // 타겟 설정
                }
            }
        }

        // 타겟이 있으면 추격 시작
        if (enemyManger.currentTarget != null && isSleeping == false)
            return pursueTargetState;
        else
            return this;
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(10f);

        isSleeping = false; // 잠 상태 해제
    }
}
