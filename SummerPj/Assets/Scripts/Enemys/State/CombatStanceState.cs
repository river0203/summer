using UnityEngine;

// 공격 가능 범위일때 잠깐 공격 판단
public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState; 
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        // 플레이어와의 거리 저장
        float distanceFromTarget = Vector3.Distance(enemyManager._currentTarget.transform.position, enemyManager.transform.position);

        // 적이 공격 하고 있었으면 이속을 0으로 밀어버림 (공격하고 달리는 애니메이션으로 넘어가는거 방지)
        if (enemyManager.isPreformingAction)
        {
            enemyAnimatorManger._anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        }

        if (!enemyManager.isPreformingAction// 공격 상태 체크
            && distanceFromTarget <= enemyManager.maximumAttackRange) // 공격 가능 범위 체크
        {
            return attackState; // 그때 공격 추천
        }
        else if(distanceFromTarget > enemyManager.maximumAttackRange) // 공격 가능 범위가 벗어남
        {
            return pursueTargetState; // 다시 쫓는거 추천
        }
        else
        {
            return this; // 멈추는거 추천
        }
    }
}
