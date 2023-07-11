using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;
using static LivingEntity;

public class EnemySys : MonoBehaviour
{
    Transform _player;
    Rigidbody _sysRigid;
    NavMeshAgent _sysAgent;

    State _state = State.Idle;
    EnemyAnim _myEnemy;

    [SerializeField]
    float _startingHP;
    [SerializeField]
    float _damage;
    [SerializeField]
    private float _checkingRange; //몬스터 인식 범위
    [SerializeField]
    private float _attackRange; // 몬스터 공격 범위 

    float _hp;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _sysAgent = gameObject.GetComponent<NavMeshAgent>();
        _sysRigid  = gameObject.GetComponent<Rigidbody>();
        _myEnemy = new EnemyAnim();
        _hp = _startingHP;
    }

    private void BossMove()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance < _attackRange)
        {  
            if(_myEnemy.EnemyState == State.Skill1)
            {
                _sysAgent.isStopped = true;
            }
            else if(_myEnemy.EnemyState == State.Skill2)
            {
                _sysAgent.isStopped = true;
            }
            else if (_myEnemy.EnemyState == State.Skill3)
            {
                _sysAgent.isStopped = true;
            }
            else if (_myEnemy.EnemyState == State.Skill4)
            {
                _sysAgent.isStopped = true;
            }
            else if (_myEnemy.EnemyState == State.Skill5)
            {
                _sysAgent.isStopped = true;
            }
        }
        else if (distance <= _checkingRange)
        {
            _state = State.Run;
            _sysAgent.isStopped = false;
            _sysAgent.transform.LookAt(_player.position);
            _sysAgent.destination = _player.position;

        }
        else if (distance > _checkingRange)
        {
            _state = State.Idle;
        }
       
        
    }
    // Update is called once per frame
    void Update()
    {
        BossMove();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _myEnemy.EnemyState = State.Stage2;
        }
    }
}
