using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        // 무언가 하고 있으면 제자리에 멈추고 그냥 현재 상태 유지하는걸 추천
        if (enemyManager.isPreformingAction)
        {
            enemyAnimatorManger._anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }
            

        #region 판단할 정보를 저장
        // 내가 적까지의 화살표
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        // 적과 나의 거리 저장
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        // 적의 위치와 내가 바라보는 방향 사이의 각도
        float veiwableAngle = Vector3.Angle(targetDirection, transform.forward);
        #endregion

        if (distanceFromTarget > enemyManager.maximumAttackRange)
        {
            enemyAnimatorManger._anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        // 회전 및 이동
        HandleRotateTarget(enemyManager);

        // 다음 상황 추천
        if(distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return combatStanceState;
        }
        else
        {
            return this;
        }
    }

    // 얘가 뭐하는지 확인해서 바라보기만 할지, 따라가면서 바라봄
    private void HandleRotateTarget(EnemyManager enemyManager)
    {
        // 뭔가 하고 있다면 타겟을 바라보기만 함
        if (enemyManager.isPreformingAction)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position; // 나와 적의 화살표
            direction.y = 0; // 높낮이는 뺌
            direction.Normalize(); // 정규화

            // 타겟이 없으면 그냥 정면 봐
            if (direction == Vector3.zero) // 0,0,0
            {
                direction = transform.forward;
            }

            // 무지성으로 다이렉션 위치를 쳐다보는 코드
            // 타겟 방향의 회전값을 추출 (각도만 추출, 회전 안 함)
            Quaternion targetRotation = Quaternion.LookRotation(direction); // 화살표를 바라보는 회전값을 추출하는 함수

            // 추출한 각도로 회전 (근데 천천히)
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        // 아무것도 안 하고 있으면 타겟을 따라가면서 바라봄
        else
        {
            // 적의 방향을 월드 -> 로컬로 변경
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);

            enemyManager.navmeshAgent.enabled = true; // 네비게이션 활성화

            // 위치 변환
            enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position); // 목적지 지정
            enemyManager.enemyRigidBody.velocity = enemyManager.navmeshAgent.velocity; // 방향과 속도 지정

            // 회전 변환
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
