using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class MobState : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    Animation anim;
   

    //열거형으로 정해진 상태값을 사용
    enum State
    {
        Idle,
        Run,
        Attack
    }
    //상태 처리
    State state;

    // Start is called before the first frame update
    void Start()
    {
        //생성시 상태를 Idle로 한다.
        state = State.Idle;
        anim = GetComponent<Animation>();
        //요원을 정의해줘서
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //만약 state가 idle이라면
        if (state == State.Idle)
        {
            UpdateIdle();
        }
        else if (state == State.Run)
        {
            UpdateRun();
        }
        else if (state == State.Attack)
        {
            UpdateAttack();
        }

    }

    private void UpdateAttack()
    {
        agent.speed = 0;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > 2)
        {
            state = State.Run;
            anim.Play("walk");
        }
    }

    private void UpdateRun()
    {


        //남은 거리가 2미터라면 공격한다.
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= 2)
        {
            state = State.Attack;
            anim.Play("attack02");
        }

        //타겟 방향으로 이동하다가
        agent.speed = 3.5f;
        //요원에게 목적지를 알려준다.
        agent.destination = target.transform.position;

    }

    private void UpdateIdle()
    {
        agent.speed = 0;
        //생성될때 목적지(Player)를 찿는다.
        target = GameObject.Find("Player").transform;
        //target을 찾으면 Run상태로 전이하고 싶다.
        if (target != null)
        {
            state = State.Run;
            //이렇게 state값을 바꿨다고 animation까지 바뀔까? no! 동기화를 해줘야한다.
            anim.Play("walk");
        }
    }
}