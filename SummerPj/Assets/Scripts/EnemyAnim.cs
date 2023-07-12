using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;
using static LivingEntity;
//Enemy2
//요원(_agent=enemy)에게 목적지를 알려줘서 목적지로 이동하게 한다.
//상태를 만들어서 제어하고 싶다.
// Idle : Player를 찾는다, 찾았으면 Run상태로 전이하고 싶다.
//Run : 타겟방향으로 이동(요원)
//Attack : 일정 시간마다 공격
//attack -> run이 안됨 수정
//스킬 랜덤 -> 가중치 랜덤
//state가 dead면 코루틴 중지

public class EnemyAnim : MonoBehaviour
{
    private static State _enemyState = State.Idle;
    private List<string> _skillList = new List<string>() { "skill_1", "skill_2", "skill_3", "skill_4", "skill_5" };
    private EnemySys _enemySys;

    [SerializeField]
    private float _checkingRange; //몬스터 인식 범위
    [SerializeField]
    private float _attackRange; // 몬스터 공격 범위 
    [SerializeField]
    Transform _target;

    static Animator _anim;

    bool _isAttack = false;

    public static State EnemyState
    {
        get
        {
            return _enemyState;
        }
        set
        {
            if (_enemyState == value)
                return;

            _enemyState = value;
            //Debug.Log(_enemyState);

            string currentState = Enum.GetName(typeof(State), _enemyState);
            _anim.CrossFade(currentState, 0.1f);
        }
    }

    void Start()
    {
        _anim = GetComponent<Animator>();
        _enemySys = new EnemySys();
    }

    private void UpdateIdle()
    {
        EnemyState = State.Idle;
    }
    private void UpdateRun()
    {
        _checkingRange = 50f;
        EnemyState = State.Run;
    }
    private void UpdateAttack()
    {
        if (!_isAttack)
        {
            _isAttack = true;
            int rand = UnityEngine.Random.Range(0, _skillList.Count);

            if (_skillList[rand] == "skill_1")
            {
                EnemyState = State.Skill1;
            }
            else if (_skillList[rand] == "skill_2")
            {
                EnemyState = State.Skill2;

            }
            else if (_skillList[rand] == "skill_3")
            {
                EnemyState = State.Skill3;

            }
            else if (_skillList[rand] == "skill_4")
            {
                EnemyState = State.Skill4;

            }
            else if (_skillList[rand] == "skill_5")
            {
                EnemyState = State.Skill5;
            }
        }
    }
    public void AttackFinish()
    {
        _isAttack = false;
    }


    void Update()
    {
        float distance = Vector3.Distance(transform.position, _target.transform.position);
        if (_isAttack)
            return;

        if (distance < _attackRange)
        {
            UpdateAttack();
        }
        else if (distance < _checkingRange)
        {
            UpdateRun();
        }
        else if (distance > _checkingRange)
        {
            UpdateIdle();
        }

    }
}