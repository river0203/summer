using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static LivingEntity;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class EnemySystem : MonoBehaviour
{
    public State _sysState;
    Transform _player;
    NavMeshAgent _sysAgent;
    Rigidbody _sysRigid;
    EnemyAnim State;
    EnemyAnim Target;
    
    

    private void Start()
    {
        _sysRigid = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _sysAgent = gameObject.GetComponent<NavMeshAgent>();
        State = GetComponent<EnemyAnim>();
        Target = GetComponent<EnemyAnim>();
    }

    private void BossMOve()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        _sysState = State.EnemyState;
        
       
        
    }
    private void Update()
    {
        _sysAgent.destination = _player.position;
        BossMOve();
        //_sysState = State.EnemyState;
        //Debug.Log(_sysState);
    }
    void freeze_velocity()
    {
        _sysRigid.velocity = Vector3.zero;
        _sysRigid.angularVelocity = Vector3.zero;
        
    }

    private void FixedUpdate()
    {
        freeze_velocity();
    }
    
    /*
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
