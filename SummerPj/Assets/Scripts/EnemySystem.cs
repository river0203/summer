/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [SerializeField]
    float _startingHp;

    float hp;
    float damage = 10;
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

    IEnumerator play_wakeUp()
    {
        yield return new WaitForSeconds(3);//�ʴ� in �ð��� ���� �����Ͻø� �˴ϴ�.
        //play IN anim
        Debug.Log("WakeUP");
        enemy_state = State.Run;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //���� �±׿� �浹 �Ͽ��� �� �ߵ�
        if (collision.collider.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("onDamage");
            hp -= damage;
            Debug.Log(hp);
            EnemyState = State.Stage2;
            if (EnemyState == State.Stage2)
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
        _agent.GetComponent<CapsuleCollider>().isTrigger = true; // <= ���̷��� ����
        yield return new WaitForSeconds(3f);
        EnemyState = State.Run;
        _agent.GetComponent<CapsuleCollider>().isTrigger = false;
        StopCoroutine(hitting_delay());
    }


}*/
