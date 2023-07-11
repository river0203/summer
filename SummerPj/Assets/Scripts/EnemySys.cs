using UnityEngine;
using UnityEngine.AI;
using static LivingEntity;

public class EnemySys : MonoBehaviour
{
    Transform _player;
    NavMeshAgent _sysAgent;
    Rigidbody _sysRigid;

    State _state = State.Idle;

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
        _startingHP = _hp;
    }

    private void BossMove()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance < _attackRange)
        {
            _state = State.Attack;
            Debug.Log("attack");
        }
        else if (distance <= _checkingRange)
        {
            _state = State.Run;
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
        if (collision.collider.gameObject.CompareTag("Weapon"))
        {
            _hp -= _damage;
            Debug.Log(_hp);
            //_sysState = LivingEntity.State.Stage2;

            if(_hp <= 0)
            {
                //_sysState = LivingEntity.State.Dead;
                Destroy(this.gameObject);
                _state = State.Dead;
            }
        }

    }
}
