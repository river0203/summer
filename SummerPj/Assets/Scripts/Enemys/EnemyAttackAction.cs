using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 공격 액션에 대한 정보
[CreateAssetMenu(menuName ="A.I/Enemy Action/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    public int attackScore = 3; // 공격 점수

    public float maximumAttackAngle = 35; // 공격 가능한 각도
    public float minimumAttackAngle = -35; // ''

    public float maximumDistanceNeededToAttack = 3; // 공격 가능 최대 거리
    public float minimumDistanceNeededToAttack = 0; // 공격 가능 최소 거리
}

