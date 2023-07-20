using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : State
{
    public bool isSleeping;
    public float detectionRadius = 2;
    public string sleepAnimation;
    public string wakeAnimation;

    public LayerMask detectionLayer;

    public PursueTargetState pursueTargetState; 

    public override State Tick(EnemyManager enemyManger, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManger)
    {
        if(isSleeping && enemyManger.isInteracting == false)
        {
            enemyAnimatorManger.PlayTargetAnimation(sleepAnimation, true);
        }

        #region Handle target detection
        Collider[] colliders = Physics.OverlapSphere(enemyManger.transform.position, detectionRadius, detectionLayer);

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if(characterStats != null )
            {
                Vector3 targetDirection = characterStats.transform.position - enemyManger.transform.position;
                enemyManger.viewableAngle = Vector3.Angle(targetDirection, enemyManger.transform.forward);

                if(enemyManger.viewableAngle > enemyManger.minimumDetectionAngle && enemyManger.viewableAngle < enemyManger.maximumDetectionAngle)
                {
                    enemyManger.currentTarget = characterStats;
                    isSleeping = false;

                    enemyAnimatorManger.PlayTargetAnimation(wakeAnimation, true);

                }
            }
        }

        #endregion

        #region Handle State Change

        if (enemyManger.currentTarget != null)
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
