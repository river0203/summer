using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void freeze_velocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        freeze_velocity();
    }

    /*IEnumerator play_wakeUp()
    {
        yield return new WaitForSeconds(3);//�ʴ� in �ð��� ���� �����Ͻø� �˴ϴ�.
        //play IN anim
        Debug.Log("WakeUP");
        enemy_state = State.Run;
    }*/

    
}
