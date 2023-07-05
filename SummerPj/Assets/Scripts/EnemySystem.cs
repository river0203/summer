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

    private void OnCollisionEnter(Collision collision)
    {
        //���� �±׿� �浹 �Ͽ��� �� �ߵ�
        if (collision.collider.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("onDamage");
            hp -= damage;
            Debug.Log(hp);
            enemy_state = State.IsHitting;
            if (enemy_state == State.IsHitting)
            {
                //play stage2 anim
                StartCoroutine("hitting_delay");

                if (hp <= 0)
                {
                    //play anim dead
                    Destroy(this.gameObject); //��� ����
                }
            }

        }

    }

    IEnumerator hitting_delay()
    {
        agent.GetComponent<CapsuleCollider>().isTrigger = true; // <= ���̷��� ����
        yield return new WaitForSeconds(3f);
        enemy_state = State.Run;
        agent.GetComponent<CapsuleCollider>().isTrigger = false;
        StopCoroutine(hitting_delay());
    }
}
