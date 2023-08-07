using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebirthState : State
{
    private IdleState IdleState;
    public LayerMask _detectionLayer;
    private CharacterStatsManager _characterStatsManager;
    public string _inAnim;
    public bool Phase;
    public override State Tick(EnemyManager enemyManger, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManger.detectionRadius, _detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            MobState _mobState = colliders[i].transform.GetComponent<MobState>();

            if(_mobState != null && _characterStatsManager._currentHealth <= 40)
            {
                enemyAnimatorManger.PlayTargetAnimation(_inAnim, true);
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
