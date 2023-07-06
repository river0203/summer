using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static LivingEntity;

public class EnemySystem : MonoBehaviour
{
    Transform _monster;
    Transform _plyaer;
    NavMeshAgent _agent;
    Rigidbody _rigid;
    State _enemyState;
    

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _monster = gameObject.GetComponent<Transform>();
        _plyaer = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _agent.destination = _plyaer.position;
    }
    void freeze_velocity()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
        
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        //무기 태그와 충돌 하였을 때 발동
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
                    Destroy(this.gameObject); //즉시 삭제
                }
            }

        }

    }

    IEnumerator hitting_delay()
    {
        _agent.GetComponent<CapsuleCollider>().isTrigger = true; // <= 스켈레톤 전용
        yield return new WaitForSeconds(3f);
        EnemyState = State.Run;
        _agent.GetComponent<CapsuleCollider>().isTrigger = false;
        StopCoroutine(hitting_delay());
    }*/


}
