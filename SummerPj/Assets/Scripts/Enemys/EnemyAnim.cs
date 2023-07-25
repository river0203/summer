/*using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;
using static LivingEntity;
//Enemy2
//?�원(_agent=enemy)?�게 목적지�??�려줘서 목적지�??�동?�게 ?�다.
//?�태�?만들?�서 ?�어?�고 ?�다.
// Idle : Player�?찾는?? 찾았?�면 Run?�태�??�이?�고 ?�다.
//Run : ?�겟방?�으�??�동(?�원)
//Attack : ?�정 ?�간마다 공격
//attack -> run???�됨 ?�정
//?�킬 ?�덤 -> 가중치 ?�덤
//state가 dead�?코루??중�?

public class EnemyAnim : MonoBehaviour
{
    private static State _enemyState = State.Idle;
    private List<string> _skillList = new List<string>() { "skill_1", "skill_2", "skill_3", "skill_4", "skill_5" };
    private EnemySys _enemySys;

    [SerializeField]
    private float _checkingRange; //몬스???�식 범위
    [SerializeField]
    private float _attackRange; // 몬스??공격 범위 
    [SerializeField]
    Transform _target;

    static Animator _anim;

    public bool _isAttack = false;

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
            Debug.Log(_enemyState);

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
        Debug.Log(_isAttack);
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
}*/