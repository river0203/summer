/*using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;
using static LivingEntity;
//Enemy2
//?”ì›(_agent=enemy)?ê²Œ ëª©ì ì§€ë¥??Œë ¤ì¤˜ì„œ ëª©ì ì§€ë¡??´ë™?˜ê²Œ ?œë‹¤.
//?íƒœë¥?ë§Œë“¤?´ì„œ ?œì–´?˜ê³  ?¶ë‹¤.
// Idle : Playerë¥?ì°¾ëŠ”?? ì°¾ì•˜?¼ë©´ Run?íƒœë¡??„ì´?˜ê³  ?¶ë‹¤.
//Run : ?€ê²Ÿë°©?¥ìœ¼ë¡??´ë™(?”ì›)
//Attack : ?¼ì • ?œê°„ë§ˆë‹¤ ê³µê²©
//attack -> run???ˆë¨ ?˜ì •
//?¤í‚¬ ?œë¤ -> ê°€ì¤‘ì¹˜ ?œë¤
//stateê°€ deadë©?ì½”ë£¨??ì¤‘ì?

public class EnemyAnim : MonoBehaviour
{
    private static State _enemyState = State.Idle;
    private List<string> _skillList = new List<string>() { "skill_1", "skill_2", "skill_3", "skill_4", "skill_5" };
    private EnemySys _enemySys;

    [SerializeField]
    private float _checkingRange; //ëª¬ìŠ¤???¸ì‹ ë²”ìœ„
    [SerializeField]
    private float _attackRange; // ëª¬ìŠ¤??ê³µê²© ë²”ìœ„ 
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