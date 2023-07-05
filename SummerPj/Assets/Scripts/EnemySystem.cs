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
        yield return new WaitForSeconds(3);//초는 in 시간에 맞춰 변경하시면 됩니다.
        //play IN anim
        Debug.Log("WakeUP");
        enemy_state = State.Run;
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        //무기 태그와 충돌 하였을 때 발동
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
                    Destroy(this.gameObject); //즉시 삭제
                }
            }

        }

    }

    IEnumerator hitting_delay()
    {
        agent.GetComponent<CapsuleCollider>().isTrigger = true; // <= 스켈레톤 전용
        yield return new WaitForSeconds(3f);
        enemy_state = State.Run;
        agent.GetComponent<CapsuleCollider>().isTrigger = false;
        StopCoroutine(hitting_delay());
    }
}
