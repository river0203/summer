using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ���� �׼ǿ� ���� ����
[CreateAssetMenu(menuName ="A.I/Enemy Action/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    public int attackScore = 3; // ���� ����

    public float maximumAttackAngle = 35; // ���� ������ ����
    public float minimumAttackAngle = -35; // ''

    public float minimumDistanceNeededToAttack = 0; // ���� ���� �ּ� �Ÿ�
    public float maximumDistanceNeededToAttack = 3; // ���� ���� �ִ� �Ÿ�
}

