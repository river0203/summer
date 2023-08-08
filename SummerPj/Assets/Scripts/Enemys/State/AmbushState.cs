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

        // �ڰ� ������ �ڴ� �ִϸ��̼� ����
        if(isSleeping && enemyManger.isInteracting == false)
            enemyAnimatorManger.PlayTargetAnimation(sleepAnimation, true);

        // ���� ���� �÷��̾� ����
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
                    // ������ �Ǹ�
                    enemyAnimatorManger._anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    enemyAnimatorManger._anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                    enemyAnimatorManger._anim.SetBool("isPreformingAction", true);
                    enemyAnimatorManger.PlayTargetAnimation(wakeAnimation, true);
                    StartCoroutine(WaitSeconds());

                    enemyManger.currentTarget = characterStats; // Ÿ�� ����
                }
            }
        }

        // Ÿ���� ������ �߰� ����
        if (enemyManger.currentTarget != null && isSleeping == false)
            return pursueTargetState;
        else
            return this;
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(10f);

        isSleeping = false; // �� ���� ����
    }
}
