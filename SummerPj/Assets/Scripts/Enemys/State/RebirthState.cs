using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebirthState : State
{
    private IdleState IdleState;
    private LayerMask _detectionLayer;
    private CharacterStatsManager _characterStatsManager;
    public bool Phase;
    public override State Tick(EnemyManager enemyManger, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManger.detectionRadius, _detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            // CharacterStats�� ������ (ĳ�������� �ƴ��� �Ǻ�)
            MobState _mobState = colliders[i].transform.GetComponent<MobState>();

            if(_mobState != null && _characterStatsManager._isDead == true)
            {
                _characterStatsManager._currentHealth += 50;
                Phase = true;
                return IdleState;
            }
            else
            {
                Phase = false;
                return this;
            }

        }

        return IdleState;
    }
}
